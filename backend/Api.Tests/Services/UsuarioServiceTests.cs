using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Services;
using Moq;
using Xunit;

namespace Api.Tests.Services;

public class UsuarioServiceTests
{
    private readonly Mock<IUsuarioRepository> _mockRepository;
    private readonly Mock<IAuthService> _mockAuthService;
    private readonly UsuarioService _service;

    public UsuarioServiceTests()
    {
        _mockRepository = new Mock<IUsuarioRepository>();
        _mockAuthService = new Mock<IAuthService>();
        _service = new UsuarioService(_mockRepository.Object, _mockAuthService.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsuarios()
    {
        var usuarios = new List<Usuario>
        {
            new() { Id = 1, Nome = "User1", Email = "user1@test.com" },
            new() { Id = 2, Nome = "User2", Email = "user2@test.com" }
        };
        _mockRepository.Setup(r => r.GetAllAsync(1, 10, null)).ReturnsAsync(usuarios);

        var result = await _service.GetAllAsync(1, 10, null);

        Assert.Equal(2, result.Count());
        _mockRepository.Verify(r => r.GetAllAsync(1, 10, null), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnUsuario()
    {
        var usuario = new Usuario { Id = 1, Nome = "Test", Email = "test@test.com" };
        _mockRepository.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(usuario);

        var result = await _service.GetByIdAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Test", result.Nome);
        _mockRepository.Verify(r => r.GetByIdAsync(1), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldHashPasswordAndCreateUsuario()
    {
        var usuario = new Usuario 
        { 
            Nome = "New User", 
            Email = "new@test.com", 
            Senha = "plaintext123",
            Role = Role.Client 
        };

        _mockAuthService.Setup(a => a.HashPassword("plaintext123")).Returns("hashedpassword");
        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Usuario>()))
                      .ReturnsAsync((Usuario u) => { u.Id = 1; return u; });

        var result = await _service.CreateAsync(usuario);

        Assert.Equal("hashedpassword", result.Senha);
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Usuario>()), Times.Once);
        _mockAuthService.Verify(a => a.HashPassword("plaintext123"), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUsuario()
    {
        var usuario = new Usuario { Id = 1, Nome = "Updated", Email = "updated@test.com" };
        _mockRepository.Setup(r => r.UpdateAsync(It.IsAny<Usuario>())).ReturnsAsync(usuario);

        var result = await _service.UpdateAsync(1, usuario);

        Assert.Equal("Updated", result.Nome);
        _mockRepository.Verify(r => r.UpdateAsync(It.IsAny<Usuario>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldCallRepository()
    {
        _mockRepository.Setup(r => r.DeleteAsync(1)).Returns(Task.CompletedTask);

        await _service.DeleteAsync(1);

        _mockRepository.Verify(r => r.DeleteAsync(1), Times.Once);
    }
}

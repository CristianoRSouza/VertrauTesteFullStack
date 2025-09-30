using Api.Controllers;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Api.Tests.Controllers;

public class UsuariosControllerTests
{
    private readonly Mock<IUsuarioService> _mockService;
    private readonly UsuariosController _controller;

    public UsuariosControllerTests()
    {
        _mockService = new Mock<IUsuarioService>();
        _controller = new UsuariosController(_mockService.Object);
    }

    [Fact]
    public async Task GetUsuarios_ShouldReturnOkWithUsuarios()
    {
        var usuarios = new List<Usuario>
        {
            new() { Id = 1, Nome = "User1", Email = "user1@test.com" },
            new() { Id = 2, Nome = "User2", Email = "user2@test.com" }
        };
        _mockService.Setup(s => s.GetAllAsync(1, 10, null)).ReturnsAsync(usuarios);

        var result = await _controller.GetUsuarios(1, 10, null);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsuarios = Assert.IsAssignableFrom<IEnumerable<Usuario>>(okResult.Value);
        Assert.Equal(2, returnedUsuarios.Count());
    }

    [Fact]
    public async Task GetUsuario_WithValidId_ShouldReturnOkWithUsuario()
    {
        var usuario = new Usuario { Id = 1, Nome = "Test", Email = "test@test.com" };
        _mockService.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(usuario);

        var result = await _controller.GetUsuario(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsuario = Assert.IsType<Usuario>(okResult.Value);
        Assert.Equal("Test", returnedUsuario.Nome);
    }

    [Fact]
    public async Task GetUsuario_WithInvalidId_ShouldReturnNotFound()
    {
        _mockService.Setup(s => s.GetByIdAsync(999)).ReturnsAsync((Usuario?)null);

        var result = await _controller.GetUsuario(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task DeleteUsuario_WithValidId_ShouldReturnNoContent()
    {
        _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteUsuario(1);

        Assert.IsType<NoContentResult>(result);
        _mockService.Verify(s => s.DeleteAsync(1), Times.Once);
    }
}

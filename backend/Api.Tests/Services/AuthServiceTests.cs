using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace Api.Tests.Services;

public class AuthServiceTests
{
    private readonly Mock<IUsuarioRepository> _mockRepository;
    private readonly Mock<IConfiguration> _mockConfig;
    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _mockRepository = new Mock<IUsuarioRepository>();
        _mockConfig = new Mock<IConfiguration>();
        
        _mockConfig.Setup(c => c["Jwt:Key"]).Returns("MySecretKeyForJWTTokenGeneration123456789");
        _mockConfig.Setup(c => c["Jwt:Issuer"]).Returns("VertrauApi");
        _mockConfig.Setup(c => c["Jwt:Audience"]).Returns("VertrauClient");
        
        _service = new AuthService(_mockRepository.Object, _mockConfig.Object);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnToken()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("password123");
        var usuario = new Usuario 
        { 
            Id = 1, 
            Email = "test@test.com", 
            Senha = hashedPassword,
            Role = Role.Client 
        };

        _mockRepository.Setup(r => r.GetByEmailAsync("test@test.com"))
                      .ReturnsAsync(usuario);

        var result = await _service.LoginAsync("test@test.com", "password123");

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains(".", result); 
    }

    [Fact]
    public async Task LoginAsync_WithInvalidEmail_ShouldReturnNull()
    {
        _mockRepository.Setup(r => r.GetByEmailAsync("invalid@test.com"))
                      .ReturnsAsync((Usuario?)null);

        var result = await _service.LoginAsync("invalid@test.com", "password123");

        Assert.Null(result);
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldReturnNull()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("correctpassword");
        var usuario = new Usuario 
        { 
            Id = 1, 
            Email = "test@test.com", 
            Senha = hashedPassword,
            Role = Role.Client 
        };

        _mockRepository.Setup(r => r.GetByEmailAsync("test@test.com"))
                      .ReturnsAsync(usuario);

        var result = await _service.LoginAsync("test@test.com", "wrongpassword");

        Assert.Null(result);
    }

    [Fact]
    public void GenerateToken_ShouldReturnValidJwtToken()
    {
        var usuario = new Usuario 
        { 
            Id = 1, 
            Email = "test@test.com", 
            Role = Role.Client 
        };

        var result = _service.GenerateToken(usuario);

        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Contains(".", result); 
    }

    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        var result = _service.HashPassword("password123");

        Assert.NotNull(result);
        Assert.NotEqual("password123", result);
        Assert.True(result.StartsWith("$2")); 
    }

    [Fact]
    public void VerifyPassword_WithCorrectPassword_ShouldReturnTrue()
    {
        var password = "password123";
        var hash = _service.HashPassword(password);

        var result = _service.VerifyPassword(password, hash);

        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        var hash = _service.HashPassword("correctpassword");

        var result = _service.VerifyPassword("wrongpassword", hash);

        Assert.False(result);
    }
}

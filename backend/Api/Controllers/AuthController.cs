using System;
using System.Threading.Tasks;
using Api.DTOs.Requests;
using Api.DTOs.Responses;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUsuarioRepository _repository;

    public AuthController(IAuthService authService, IUsuarioRepository repository)
    {
        _authService = authService;
        _repository = repository;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
    {
        var token = await _authService.LoginAsync(request.Email, request.Senha);
        
        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Email ou senha inválidos" });
        }
        
        var usuario = await _repository.GetByEmailAsync(request.Email);
        
        return Ok(new LoginResponse
        {
            Token = token,
            Email = request.Email,
            Role = usuario?.Role.ToString() ?? "Client"
        });
    }
}

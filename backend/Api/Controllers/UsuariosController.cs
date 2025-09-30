using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.DTOs.Requests;
using Api.Mappers;
using Domain.Entities;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        [FromQuery] string? filtro = null)
    {
        var usuarios = await _usuarioService.GetAllAsync(page, pageSize, filtro);
        return Ok(usuarios);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(long id)
    {
        var usuario = await _usuarioService.GetByIdAsync(id);
        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<ActionResult<Usuario>> CreateUsuario(CreateUsuarioRequest request)
    {
        var usuario = UsuarioMapper.ToEntity(request);
        var created = await _usuarioService.CreateAsync(usuario);
        return CreatedAtAction(nameof(GetUsuario), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Usuario>> UpdateUsuario(long id, UpdateUsuarioRequest request)
    {
        if (id != request.Id)
            return BadRequest();

        var usuario = UsuarioMapper.ToEntity(request);
        var updated = await _usuarioService.UpdateAsync(id, usuario);
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUsuario(long id)
    {
        await _usuarioService.DeleteAsync(id);
        return NoContent();
    }
}

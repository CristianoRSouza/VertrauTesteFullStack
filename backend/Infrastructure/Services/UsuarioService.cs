using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Infrastructure.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;
    private readonly IAuthService _authService;

    public UsuarioService(IUsuarioRepository repository, IAuthService authService)
    {
        _repository = repository;
        _authService = authService;
    }

    public async Task<IEnumerable<Usuario>> GetAllAsync(int page, int pageSize, string? filtro)
    {
        return await _repository.GetAllAsync(page, pageSize, filtro);
    }

    public async Task<Usuario?> GetByIdAsync(long id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Usuario> CreateAsync(Usuario usuario)
    {
        ValidateUsuario(usuario);
        
        if (await _repository.EmailExistsAsync(usuario.Email))
            throw new ConflictException("Email já está em uso");

        usuario.Senha = _authService.HashPassword(usuario.Senha);
        return await _repository.CreateAsync(usuario);
    }

    public async Task<Usuario> UpdateAsync(long id, Usuario usuario)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            throw new NotFoundException("Usuário não encontrado");

        ValidateUsuario(usuario);

        if (await _repository.EmailExistsAsync(usuario.Email, id))
            throw new ConflictException("Email já está em uso");

        existing.Nome = usuario.Nome;
        existing.Sobrenome = usuario.Sobrenome;
        existing.Email = usuario.Email;
        existing.Senha = _authService.HashPassword(usuario.Senha);
        existing.Role = usuario.Role;
        existing.Genero = usuario.Genero;
        existing.DataNascimento = usuario.DataNascimento;
        existing.Endereco = usuario.Endereco;

        return await _repository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(long id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario == null)
            throw new NotFoundException("Usuário não encontrado");

        await _repository.DeleteAsync(id);
    }

    private static void ValidateUsuario(Usuario usuario)
    {
        if (usuario.DataNascimento > DateTime.Now)
            throw new ValidationException("Data de nascimento não pode ser no futuro");
    }
}

using Api.DTOs.Requests;
using Domain.Entities;

namespace Api.Mappers;

public static class UsuarioMapper
{
    public static Usuario ToEntity(CreateUsuarioRequest request)
    {
        var usuario = new Usuario
        {
            Nome = request.Nome,
            Sobrenome = request.Sobrenome,
            Email = request.Email,
            Senha = request.Senha,
            Role = request.Role,
            Genero = request.Genero,
            DataNascimento = request.DataNascimento
        };

        if (request.Endereco != null)
        {
            usuario.Endereco = request.Endereco.ToEntity();
        }

        return usuario;
    }

    public static Usuario ToEntity(UpdateUsuarioRequest request)
    {
        var usuario = new Usuario
        {
            Id = request.Id,
            Nome = request.Nome,
            Sobrenome = request.Sobrenome,
            Email = request.Email,
            Senha = request.Senha,
            Role = request.Role,
            Genero = request.Genero,
            DataNascimento = request.DataNascimento
        };

        if (request.Endereco != null)
        {
            usuario.Endereco = request.Endereco.ToEntity();
        }

        return usuario;
    }
}

using System;
using Domain.Enums;

namespace Api.DTOs.Requests;

public class CreateUsuarioRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.Client;
    public Genero Genero { get; set; }
    public DateTime? DataNascimento { get; set; }
    public EnderecoRequest? Endereco { get; set; }
}

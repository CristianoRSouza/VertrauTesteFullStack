using System;
using Domain.Enums;

namespace Domain.Entities;

public class Usuario
{
    public long Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Sobrenome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
    public Role Role { get; set; } = Role.Client;
    public Genero Genero { get; set; }
    public DateTime? DataNascimento { get; set; }
    public Endereco? Endereco { get; set; }
}

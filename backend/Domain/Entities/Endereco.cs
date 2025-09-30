namespace Domain.Entities;

public class Endereco
{
    public string Cep { get; private set; } = string.Empty;
    public string Estado { get; private set; } = string.Empty;
    public string Logradouro { get; private set; } = string.Empty;
    public string Bairro { get; private set; } = string.Empty;
    public string Cidade { get; private set; } = string.Empty;
    public string Numero { get; private set; } = string.Empty;
    public string? Complemento { get; private set; }

    private Endereco() { } 

    public Endereco(string cep, string estado, string logradouro, string bairro, 
                   string cidade, string numero, string? complemento = null)
    {
        Cep = cep;
        Estado = estado;
        Logradouro = logradouro;
        Bairro = bairro;
        Cidade = cidade;
        Numero = numero;
        Complemento = complemento;
    }
}

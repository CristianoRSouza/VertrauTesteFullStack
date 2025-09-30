using Api.DTOs.Requests;
using Domain.Entities;

namespace Api.Mappers;

public static class EnderecoMapper
{
    public static Endereco ToEntity(this EnderecoRequest request)
    {
        return new Endereco(
            request.Cep,
            request.Estado,
            request.Logradouro,
            request.Bairro,
            request.Cidade,
            request.Numero,
            request.Complemento
        );
    }
}

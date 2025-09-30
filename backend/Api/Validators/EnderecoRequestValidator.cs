using Api.DTOs.Requests;
using FluentValidation;

namespace Api.Validators;

public class EnderecoRequestValidator : AbstractValidator<EnderecoRequest>
{
    public EnderecoRequestValidator()
    {
        RuleFor(x => x.Cep)
            .NotEmpty().WithMessage("CEP é obrigatório")
            .Matches(@"^\d{5}-?\d{3}$").WithMessage("CEP deve ter o formato 00000-000");

        RuleFor(x => x.Estado)
            .NotEmpty().WithMessage("Estado é obrigatório")
            .Length(2, 2).WithMessage("Estado deve ter exatamente 2 caracteres");

        RuleFor(x => x.Logradouro)
            .NotEmpty().WithMessage("Logradouro é obrigatório")
            .Length(5, 100).WithMessage("Logradouro deve ter entre 5 e 100 caracteres");

        RuleFor(x => x.Bairro)
            .NotEmpty().WithMessage("Bairro é obrigatório")
            .Length(2, 50).WithMessage("Bairro deve ter entre 2 e 50 caracteres");

        RuleFor(x => x.Cidade)
            .NotEmpty().WithMessage("Cidade é obrigatória")
            .Length(2, 50).WithMessage("Cidade deve ter entre 2 e 50 caracteres");

        RuleFor(x => x.Numero)
            .NotEmpty().WithMessage("Número é obrigatório")
            .Length(1, 10).WithMessage("Número deve ter entre 1 e 10 caracteres");
    }
}

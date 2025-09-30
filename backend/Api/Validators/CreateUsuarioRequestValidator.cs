using System;
using Api.DTOs.Requests;
using FluentValidation;

namespace Api.Validators;

public class CreateUsuarioRequestValidator : AbstractValidator<CreateUsuarioRequest>
{
    public CreateUsuarioRequestValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .Length(2, 50).WithMessage("Nome deve ter entre 2 e 50 caracteres");

        RuleFor(x => x.Sobrenome)
            .NotEmpty().WithMessage("Sobrenome é obrigatório")
            .Length(2, 50).WithMessage("Sobrenome deve ter entre 2 e 50 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email deve ter um formato válido");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória")
            .MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres");

        RuleFor(x => x.Genero)
            .IsInEnum().WithMessage("Gênero deve ser válido");

        RuleFor(x => x.DataNascimento)
            .LessThan(DateTime.Now).WithMessage("Data de nascimento deve ser anterior à data atual")
            .When(x => x.DataNascimento.HasValue);

        RuleFor(x => x.Endereco)
            .SetValidator(new EnderecoRequestValidator()!)
            .When(x => x.Endereco != null);
    }
}

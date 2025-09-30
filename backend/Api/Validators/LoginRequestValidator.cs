using Api.DTOs.Requests;
using FluentValidation;

namespace Api.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório")
            .EmailAddress().WithMessage("Email deve ter um formato válido");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha é obrigatória");
    }
}

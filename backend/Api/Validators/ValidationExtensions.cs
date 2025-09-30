using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace Api.Validators;

public static class ValidationExtensions
{
    public static async Task<ValidationResult> ValidateAsync<T>(this IValidator<T> validator, T instance)
    {
        return await validator.ValidateAsync(instance);
    }

    public static void ThrowIfInvalid<T>(this IValidator<T> validator, T instance)
    {
        var result = validator.Validate(instance);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }
}

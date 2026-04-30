using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Presentation.Validation;

public class StartsWithUppercaseAttribute : ValidationAttribute
{
    public StartsWithUppercaseAttribute()
    {
        ErrorMessage = "Назва має починатися з великої літери.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not string text || string.IsNullOrWhiteSpace(text))
            return ValidationResult.Success;

        return char.IsUpper(text[0])
            ? ValidationResult.Success
            : new ValidationResult(ErrorMessage);
    }
}

using Components.VisualEditor.Parsers;
using System.ComponentModel.DataAnnotations;
namespace Components.VisualEditor.Validation;

public static class NumberValidation
{
    /// <summary>
    ///     Validates that the given string is a valid <see cref="double"/>.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> containing the validation result.</returns>
    public static ValidationResult? ValidateDouble (string? value)
    {
        if (string.IsNullOrEmpty (value))
            return new ValidationResult ("Value cannot be empty.");

        if (!value.TryParseDouble (out _))
            return new ValidationResult ("Value must be a valid number.");

        return ValidationResult.Success;
    }

}

using FluentValidation.Results;

namespace Reservation.App.Application.Exceptions;

public class ValidationException : Exception
{
    public List<string> ValidationErrors { get; set; }

    public ValidationException(ValidationResult validationResult)
    {
        ValidationErrors = [];

        foreach (var ValidationError in validationResult.Errors)
        {
            ValidationErrors.Add(ValidationError.ErrorMessage);
        }
    }
}

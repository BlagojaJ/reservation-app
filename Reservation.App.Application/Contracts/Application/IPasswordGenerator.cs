namespace Reservation.App.Application.Contracts.Application;

/// <summary>
/// Defines a contract for generating passwords.
/// </summary>
public interface IPasswordGenerator
{
    /// <summary>
    /// Generates a password of the specified length.
    /// </summary>
    /// <param name="length">The length of the password to be generated. Must be greater than or equal to 4.</param>
    /// <returns>A string representing the generated password.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the length is less than 4.</exception>
    string GeneratePassword(int length);
}

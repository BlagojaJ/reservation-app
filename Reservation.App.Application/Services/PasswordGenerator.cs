using System.Security.Cryptography;
using System.Text;
using Reservation.App.Application.Contracts.Application;

namespace Reservation.App.Application.Services;

public class PasswordGenerator : IPasswordGenerator
{
    private const string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string LowerCase = "abcdefghijklmnopqrstuvwxyz";
    private const string Digits = "0123456789";
    private const string SpecialCharacters = "!@#$%^&*()_+<>?";
    private static readonly string AllCharacters =
        UpperCase + LowerCase + Digits + SpecialCharacters;

    public string GeneratePassword(int length)
    {
        if (length < 4)
        {
            throw new ArgumentException("Password length must be at least 4 to ensure complexity.");
        }

        var password = new StringBuilder();
        var rng = RandomNumberGenerator.Create();

        // Ensure the password contains at least one character from each group
        password.Append(GetRandomCharacter(UpperCase, rng));
        password.Append(GetRandomCharacter(LowerCase, rng));
        password.Append(GetRandomCharacter(Digits, rng));
        password.Append(GetRandomCharacter(SpecialCharacters, rng));

        // Fill the rest of the password with random characters from all groups
        for (int i = password.Length; i < length; i++)
        {
            password.Append(GetRandomCharacter(AllCharacters, rng));
        }

        // Shuffle the password to avoid predictable patterns
        return ShufflePassword(password.ToString(), rng);
    }

    private static char GetRandomCharacter(string characterSet, RandomNumberGenerator rng)
    {
        byte[] randomByte = new byte[1];
        do
        {
            rng.GetBytes(randomByte);
        } while (randomByte[0] >= characterSet.Length * (256 / characterSet.Length)); // Avoid bias

        return characterSet[randomByte[0] % characterSet.Length];
    }

    private static string ShufflePassword(string password, RandomNumberGenerator rng)
    {
        char[] array = password.ToCharArray();
        byte[] randomBytes = new byte[array.Length];

        rng.GetBytes(randomBytes);

        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = randomBytes[i] % (i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }

        return new string(array);
    }
}

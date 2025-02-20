using Reservation.App.Application.Models.Email;

namespace Reservation.App.Application.Contracts.Infrastructure;

public interface IEmailService
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="from">The email address of the sender.</param>
    /// <param name="to">The email address of the recipient.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="body">The body content of the email.</param>
    /// <param name="replyTo">Optional. The email address to which replies should be sent. If not set, the reply-to address will default to the sender's address.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a string indicating the unique email ID.</returns>
    /// <exception cref="Exception">Thrown when an error occurs during the sending of the email.</exception>
    Task<string> SendEmailAsync(
        EmailAddress from,
        EmailAddress to,
        string subject,
        string body,
        EmailAddress? replyTo = null
    );
}

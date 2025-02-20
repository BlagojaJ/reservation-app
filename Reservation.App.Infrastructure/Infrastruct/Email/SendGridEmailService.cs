using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reservation.App.Application.Contracts.Infrastructure;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Reservation.App.Infrastructure.Infrastruct.Email;

public class SendGridEmailService(
    ISendGridClient sendGridClient,
    ILogger<SendGridEmailService> logger,
    IOptions<SendGridSettings> sendGridSettings
) : IEmailService
{
    private static readonly string MESSAGE_ID_HEADER = "X-Message-Id";

    private readonly ISendGridClient _sendGridClient = sendGridClient;
    private readonly ILogger<SendGridEmailService> _logger = logger;
    private readonly SendGridSettings _sendGridSettings = sendGridSettings.Value;

    public async Task<string> SendEmailAsync(
        Application.Models.Email.EmailAddress from,
        Application.Models.Email.EmailAddress to,
        string subject,
        string body,
        Application.Models.Email.EmailAddress? replyTo = null
    )
    {
        var sender = new EmailAddress(from.Email, from.Name);

        var msg = new SendGridMessage() { From = sender, Subject = subject };

        msg.AddTo(new EmailAddress(to.Email, to.Name));

        if (replyTo != null)
        {
            msg.AddReplyTo(new EmailAddress(replyTo.Email, replyTo.Name));
        }
        else
        {
            msg.AddReplyTo(sender);
        }

        msg.AddContent(MimeType.Text, body);

        // 👉 Enable sandbox mode according according to external settings
        if (_sendGridSettings.IsSandboxModeEnabled)
        {
            msg.MailSettings = new MailSettings { SandboxMode = new SandboxMode { Enable = true } };
        }

        Response? response;
        try
        {
            response = await _sendGridClient.SendEmailAsync(msg);
        }
        catch (Exception ex)
        {
            // Catch network or other unexpected errors
            _logger.LogError(ex, "Failed to send email");
            throw new Exception("Failed to send email.", ex);
        }

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = await response.Body.ReadAsStringAsync();
            _logger.LogError(
                "Failed to send email. Status code: {StatusCode}, Response body: {ResponseBody}, Message: {@Message}",
                response.StatusCode,
                responseBody,
                msg
            );
            throw new Exception(
                $"Failed to send email. Status code: {response.StatusCode}, Response body: {responseBody}"
            );
        }

        var messageId = response
            .Headers.FirstOrDefault(h => h.Key == MESSAGE_ID_HEADER)
            .Value?.FirstOrDefault();

        return messageId ?? string.Empty;
    }
}

using EmployeesManagement.MailFunctionApp.Models;
using EmployeesManagement.MailFunctionApp.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmployeesManagement.MailFunctionApp.Services;

public sealed class SmtpMailSender(IOptions<MailTriggerOptions> options) : ISmtpMailSender
{
    private readonly MailTriggerOptions _options = options.Value;

    public async Task<MailDispatchResult> SendAsync(MailTriggerRequest request, CancellationToken cancellationToken)
    {
        ValidateSettings();

        var recipientEmail = FirstNotEmpty(request.ToEmail, _options.DefaultToEmail);
        if (string.IsNullOrWhiteSpace(recipientEmail))
        {
            throw new InvalidOperationException("A recipient email address is required either in the request body or local.settings.json.");
        }

        var recipientName = FirstNotEmpty(request.ToName, _options.DefaultToName, recipientEmail);
        var subject = FirstNotEmpty(request.Subject, _options.DefaultSubject, "Employees Portal test mail");
        var htmlBody = FirstNotEmpty(request.HtmlBody, _options.DefaultHtmlBody, "<p>This is a local Employees Portal mail test.</p>");
        var plainTextBody = FirstNotEmpty(request.PlainTextBody, "Employees Portal local mail test.");

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = plainTextBody
        };

        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
        mailMessage.To.Add(new MailboxAddress(recipientName, recipientEmail));
        mailMessage.Subject = subject;
        mailMessage.Body = bodyBuilder.ToMessageBody();

        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(_options.SmtpHost, _options.SmtpPort, _options.UseSsl, cancellationToken);

        if (!string.IsNullOrWhiteSpace(_options.Username))
        {
            await smtpClient.AuthenticateAsync(_options.Username, _options.Password, cancellationToken);
        }

        await smtpClient.SendAsync(mailMessage, cancellationToken);
        await smtpClient.DisconnectAsync(true, cancellationToken);

        return new MailDispatchResult
        {
            RecipientName = recipientName,
            RecipientEmail = recipientEmail,
            Subject = subject
        };
    }

    private void ValidateSettings()
    {
        if (string.IsNullOrWhiteSpace(_options.SmtpHost))
        {
            throw new InvalidOperationException("MailSmtpHost is missing from local.settings.json.");
        }

        if (string.IsNullOrWhiteSpace(_options.FromEmail))
        {
            throw new InvalidOperationException("MailFromEmail is missing from local.settings.json.");
        }
    }

    private static string FirstNotEmpty(params string?[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }
        }

        return string.Empty;
    }
}

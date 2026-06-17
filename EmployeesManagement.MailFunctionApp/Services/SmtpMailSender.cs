using EmployeesManagement.MailFunctionApp.Models;
using EmployeesManagement.MailFunctionApp.Options;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmployeesManagement.MailFunctionApp.Services;

public sealed class SmtpMailSender(IOptions<MailTriggerOptions> options) : ISmtpMailSender
***REMOVED***
    private readonly MailTriggerOptions _options = options.Value;

    public async Task<MailDispatchResult> SendAsync(MailTriggerRequest request, CancellationToken cancellationToken)
    ***REMOVED***
        ValidateSettings();

        var recipientEmail = FirstNotEmpty(request.ToEmail, _options.DefaultToEmail);
        if (string.IsNullOrWhiteSpace(recipientEmail))
        ***REMOVED***
            throw new InvalidOperationException("A recipient email address is required either in the request body or local.settings.json.");
      ***REMOVED***

        var recipientName = FirstNotEmpty(request.ToName, _options.DefaultToName, recipientEmail);
        var subject = FirstNotEmpty(request.Subject, _options.DefaultSubject, "Employees Portal test mail");
        var htmlBody = FirstNotEmpty(request.HtmlBody, _options.DefaultHtmlBody, "<p>This is a local Employees Portal mail test.</p>");
        var plainTextBody = FirstNotEmpty(request.PlainTextBody, "Employees Portal local mail test.");

        var bodyBuilder = new BodyBuilder
        ***REMOVED***
            HtmlBody = htmlBody,
            TextBody = plainTextBody
      ***REMOVED***;

        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(_options.FromName, _options.FromEmail));
        mailMessage.To.Add(new MailboxAddress(recipientName, recipientEmail));
        mailMessage.Subject = subject;
        mailMessage.Body = bodyBuilder.ToMessageBody();

        using var smtpClient = new SmtpClient();
        await smtpClient.ConnectAsync(_options.SmtpHost, _options.SmtpPort, _options.UseSsl, cancellationToken);

        if (!string.IsNullOrWhiteSpace(_options.Username))
        ***REMOVED***
            await smtpClient.AuthenticateAsync(_options.Username, _options.Password, cancellationToken);
      ***REMOVED***

        await smtpClient.SendAsync(mailMessage, cancellationToken);
        await smtpClient.DisconnectAsync(true, cancellationToken);

        return new MailDispatchResult
        ***REMOVED***
            RecipientName = recipientName,
            RecipientEmail = recipientEmail,
            Subject = subject
      ***REMOVED***;
  ***REMOVED***

    private void ValidateSettings()
    ***REMOVED***
        if (string.IsNullOrWhiteSpace(_options.SmtpHost))
        ***REMOVED***
            throw new InvalidOperationException("MailSmtpHost is missing from local.settings.json.");
      ***REMOVED***

        if (string.IsNullOrWhiteSpace(_options.FromEmail))
        ***REMOVED***
            throw new InvalidOperationException("MailFromEmail is missing from local.settings.json.");
      ***REMOVED***
  ***REMOVED***

    private static string FirstNotEmpty(params string?[] values)
    ***REMOVED***
        foreach (var value in values)
        ***REMOVED***
            if (!string.IsNullOrWhiteSpace(value))
            ***REMOVED***
                return value;
          ***REMOVED***
      ***REMOVED***

        return string.Empty;
  ***REMOVED***
***REMOVED***

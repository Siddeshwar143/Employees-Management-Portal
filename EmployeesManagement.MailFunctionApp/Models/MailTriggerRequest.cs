namespace EmployeesManagement.MailFunctionApp.Models;

public sealed class MailTriggerRequest
{
    public string? ToName { get; init; }

    public string? ToEmail { get; init; }

    public string? Subject { get; init; }

    public string? HtmlBody { get; init; }

    public string? PlainTextBody { get; init; }
}

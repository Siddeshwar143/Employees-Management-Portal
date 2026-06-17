namespace EmployeesManagement.MailFunctionApp.Models;

public sealed class MailDispatchResult
{
    public string RecipientName { get; init; } = string.Empty;

    public string RecipientEmail { get; init; } = string.Empty;

    public string Subject { get; init; } = string.Empty;
}

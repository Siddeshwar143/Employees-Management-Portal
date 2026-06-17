namespace EmployeesManagement.MailFunctionApp.Models;

public sealed class SecurityRemediationDrillRequest
{
    public string? RequestedBy { get; init; }

    public string? RecipientName { get; init; }

    public string? RecipientEmail { get; init; }

    public string? RepositoryScope { get; init; }
}

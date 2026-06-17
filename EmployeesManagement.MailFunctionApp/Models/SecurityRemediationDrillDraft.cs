namespace EmployeesManagement.MailFunctionApp.Models;

public sealed class SecurityRemediationDrillDraft
{
    public string StoryId { get; init; } = string.Empty;

    public string TrackingId { get; init; } = string.Empty;

    public string RepositoryScope { get; init; } = string.Empty;

    public int SecretCategoryCount { get; init; }

    public IReadOnlyList<string> SecretTypes { get; init; } = Array.Empty<string>();

    public MailTriggerRequest MailRequest { get; init; } = new();
}

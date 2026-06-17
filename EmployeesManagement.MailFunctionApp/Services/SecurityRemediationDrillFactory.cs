using System.Text;
using EmployeesManagement.MailFunctionApp.Models;

namespace EmployeesManagement.MailFunctionApp.Services;

public sealed class SecurityRemediationDrillFactory : ISecurityRemediationDrillFactory
***REMOVED***
    private static readonly string[] SecretTypes =
    [
        "SQL credentials",
        "Blob Storage keys",
        "Service Bus keys",
        "Azure Maps keys",
        "Google Maps keys",
        "Parative tokens"
    ];

    private static readonly string[] AffectedFiles =
    [
        "BrandControl/tools/BCIDPhoneNumbersSync/local.settings.json",
        "ATLaaS.UI/wwwroot/appsettings.Development.json",
        "ATLaaS.UI/wwwroot/appsettings.Production.json",
        "ATLaaS.UI/wwwroot/appsettings.Testing.json"
    ];

    private static readonly string[] AuditCommands =
    [
        "git log --all --full-history",
        "trufflehog",
        "detect-secrets"
    ];

    public SecurityRemediationDrillDraft Create(SecurityRemediationDrillRequest request)
    ***REMOVED***
        var repositoryScope = string.IsNullOrWhiteSpace(request.RepositoryScope)
            ? "BrandControl + ATLaaS"
            : request.RepositoryScope.Trim();

        var requestedBy = string.IsNullOrWhiteSpace(request.RequestedBy)
            ? "Security QA"
            : request.RequestedBy.Trim();

        var subject = $"Security remediation dummy drill - ***REMOVED***repositoryScope***REMOVED***";
        var htmlBody = BuildHtmlBody(repositoryScope, requestedBy);
        var plainTextBody = BuildPlainTextBody(repositoryScope, requestedBy);

        return new SecurityRemediationDrillDraft
        ***REMOVED***
            StoryId = "SEC-REM-HISTORY-001",
            TrackingId = "SEC-EVID-2026-REWRITE-001",
            RepositoryScope = repositoryScope,
            SecretCategoryCount = SecretTypes.Length,
            SecretTypes = SecretTypes,
            MailRequest = new MailTriggerRequest
            ***REMOVED***
                ToName = request.RecipientName,
                ToEmail = request.RecipientEmail,
                Subject = subject,
                HtmlBody = htmlBody,
                PlainTextBody = plainTextBody
          ***REMOVED***
      ***REMOVED***;
  ***REMOVED***

    private static string BuildHtmlBody(string repositoryScope, string requestedBy)
    ***REMOVED***
        var builder = new StringBuilder();
        builder.Append("<h2>Security remediation dummy drill</h2>");
        builder.Append("<p>This is a scan-only dummy mail used to validate the remediation workflow for rotated credentials.</p>");
        builder.Append($"<p><strong>Repository scope:</strong> ***REMOVED***repositoryScope***REMOVED***<br />");
        builder.Append("<strong>Story:</strong> SEC-REM-HISTORY-001<br />");
        builder.Append("<strong>Evidence:</strong> SEC-EVID-2026-REWRITE-001<br />");
        builder.Append($"<strong>Requested by:</strong> ***REMOVED***requestedBy***REMOVED***</p>");

        builder.Append("<p><strong>Affected files</strong></p><ul>");
        foreach (var file in AffectedFiles)
        ***REMOVED***
            builder.Append($"<li>***REMOVED***file***REMOVED***</li>");
      ***REMOVED***

        builder.Append("</ul><p><strong>Secret categories</strong></p><ul>");
        foreach (var secretType in SecretTypes)
        ***REMOVED***
            builder.Append($"<li>***REMOVED***secretType***REMOVED***</li>");
      ***REMOVED***

        builder.Append("</ul><p><strong>Audit commands</strong></p><ul>");
        foreach (var command in AuditCommands)
        ***REMOVED***
            builder.Append($"<li>***REMOVED***command***REMOVED***</li>");
      ***REMOVED***

        builder.Append("</ul><p><strong>Expected validation steps</strong></p><ol>");
        builder.Append("<li>Rewrite history on a release branch clone.</li>");
        builder.Append("<li>Replace appsettings secrets with placeholders.</li>");
        builder.Append("<li>Remove local.settings.json from Git history.</li>");
        builder.Append("<li>Notify every developer to re-clone after the force push.</li>");
        builder.Append("<li>Run a post-cleanup security scan and capture the evidence.</li>");
        builder.Append("</ol>");

        return builder.ToString();
  ***REMOVED***

    private static string BuildPlainTextBody(string repositoryScope, string requestedBy)
    ***REMOVED***
        var builder = new StringBuilder();
        builder.AppendLine("Security remediation dummy drill");
        builder.AppendLine("This is a scan-only dummy mail used to validate the remediation workflow for rotated credentials.");
        builder.AppendLine($"Repository scope: ***REMOVED***repositoryScope***REMOVED***");
        builder.AppendLine("Story: SEC-REM-HISTORY-001");
        builder.AppendLine("Evidence: SEC-EVID-2026-REWRITE-001");
        builder.AppendLine($"Requested by: ***REMOVED***requestedBy***REMOVED***");
        builder.AppendLine();
        builder.AppendLine("Affected files:");
        foreach (var file in AffectedFiles)
        ***REMOVED***
            builder.AppendLine($"- ***REMOVED***file***REMOVED***");
      ***REMOVED***

        builder.AppendLine();
        builder.AppendLine("Secret categories:");
        foreach (var secretType in SecretTypes)
        ***REMOVED***
            builder.AppendLine($"- ***REMOVED***secretType***REMOVED***");
      ***REMOVED***

        builder.AppendLine();
        builder.AppendLine("Audit commands:");
        foreach (var command in AuditCommands)
        ***REMOVED***
            builder.AppendLine($"- ***REMOVED***command***REMOVED***");
      ***REMOVED***

        builder.AppendLine();
        builder.AppendLine("Expected validation steps:");
        builder.AppendLine("1. Rewrite history on a release branch clone.");
        builder.AppendLine("2. Replace appsettings secrets with placeholders.");
        builder.AppendLine("3. Remove local.settings.json from Git history.");
        builder.AppendLine("4. Notify every developer to re-clone after the force push.");
        builder.AppendLine("5. Run a post-cleanup security scan and capture the evidence.");

        return builder.ToString();
  ***REMOVED***
***REMOVED***

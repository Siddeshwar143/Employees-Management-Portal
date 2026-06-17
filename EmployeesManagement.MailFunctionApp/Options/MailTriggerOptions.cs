namespace EmployeesManagement.MailFunctionApp.Options;

public sealed class MailTriggerOptions
{
    public string SmtpHost { get; set; } = string.Empty;

    public int SmtpPort { get; set; } = 465;

    public bool UseSsl { get; set; } = true;

    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FromName { get; set; } = string.Empty;

    public string FromEmail { get; set; } = string.Empty;

    public string DefaultToName { get; set; } = string.Empty;

    public string DefaultToEmail { get; set; } = string.Empty;

    public string DefaultSubject { get; set; } = string.Empty;

    public string DefaultHtmlBody { get; set; } = string.Empty;
}

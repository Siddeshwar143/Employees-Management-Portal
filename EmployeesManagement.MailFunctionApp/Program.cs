using EmployeesManagement.MailFunctionApp.Options;
using EmployeesManagement.MailFunctionApp.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    ***REMOVED***
        services.Configure<MailTriggerOptions>(options =>
        ***REMOVED***
            options.SmtpHost = context.Configuration["MailSmtpHost"] ?? string.Empty;
            options.SmtpPort = ParseInt(context.Configuration["MailSmtpPort"], 465);
            options.UseSsl = ParseBool(context.Configuration["MailUseSsl"], true);
            options.Username = context.Configuration["MailUsername"] ?? string.Empty;
            options.Password = context.Configuration["MailPassword"] ?? string.Empty;
            options.FromName = context.Configuration["MailFromName"] ?? "Employees Portal QA";
            options.FromEmail = context.Configuration["MailFromEmail"] ?? string.Empty;
            options.DefaultToName = context.Configuration["MailDefaultToName"] ?? "QA Team";
            options.DefaultToEmail = context.Configuration["MailDefaultToEmail"] ?? string.Empty;
            options.DefaultSubject = context.Configuration["MailDefaultSubject"] ?? "Employees Portal Function mail test";
            options.DefaultHtmlBody = context.Configuration["MailDefaultHtmlBody"] ?? "<p>This is a local Employees Portal Function mail test.</p>";
      ***REMOVED***);

        services.AddSingleton<ISmtpMailSender, SmtpMailSender>();
        services.AddSingleton<ISecurityRemediationDrillFactory, SecurityRemediationDrillFactory>();
  ***REMOVED***)
    .Build();

await host.RunAsync();

static int ParseInt(string? value, int fallback)
***REMOVED***
    return int.TryParse(value, out var parsed) ? parsed : fallback;
***REMOVED***

static bool ParseBool(string? value, bool fallback)
***REMOVED***
    return bool.TryParse(value, out var parsed) ? parsed : fallback;
***REMOVED***

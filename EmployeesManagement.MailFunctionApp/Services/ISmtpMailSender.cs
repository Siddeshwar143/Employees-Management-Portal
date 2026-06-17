using EmployeesManagement.MailFunctionApp.Models;

namespace EmployeesManagement.MailFunctionApp.Services;

public interface ISmtpMailSender
{
    Task<MailDispatchResult> SendAsync(MailTriggerRequest request, CancellationToken cancellationToken);
}

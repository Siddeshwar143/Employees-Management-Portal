using EmployeesManagement.MailFunctionApp.Models;

namespace EmployeesManagement.MailFunctionApp.Services;

public interface ISmtpMailSender
***REMOVED***
    Task<MailDispatchResult> SendAsync(MailTriggerRequest request, CancellationToken cancellationToken);
***REMOVED***

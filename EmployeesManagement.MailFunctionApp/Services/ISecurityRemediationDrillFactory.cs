using EmployeesManagement.MailFunctionApp.Models;

namespace EmployeesManagement.MailFunctionApp.Services;

public interface ISecurityRemediationDrillFactory
{
    SecurityRemediationDrillDraft Create(SecurityRemediationDrillRequest request);
}

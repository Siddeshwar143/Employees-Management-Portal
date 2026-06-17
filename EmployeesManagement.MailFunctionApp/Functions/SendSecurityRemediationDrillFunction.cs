using System.Net;
using System.Text.Json;
using EmployeesManagement.MailFunctionApp.Models;
using EmployeesManagement.MailFunctionApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EmployeesManagement.MailFunctionApp.Functions;

public sealed class SendSecurityRemediationDrillFunction(
    ISecurityRemediationDrillFactory drillFactory,
    ISmtpMailSender smtpMailSender,
    ILogger<SendSecurityRemediationDrillFunction> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    private readonly ISecurityRemediationDrillFactory _drillFactory = drillFactory;
    private readonly ISmtpMailSender _smtpMailSender = smtpMailSender;
    private readonly ILogger<SendSecurityRemediationDrillFunction> _logger = logger;

    [Function("SendSecurityRemediationDrillFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "mail/security-remediation-drill")] HttpRequestData request,
        CancellationToken cancellationToken)
    {
        SecurityRemediationDrillRequest payload;

        using (var reader = new StreamReader(request.Body))
        {
            var requestBody = await reader.ReadToEndAsync();
            payload = string.IsNullOrWhiteSpace(requestBody)
                ? new SecurityRemediationDrillRequest()
                : JsonSerializer.Deserialize<SecurityRemediationDrillRequest>(requestBody, JsonOptions) ?? new SecurityRemediationDrillRequest();
        }

        try
        {
            var drill = _drillFactory.Create(payload);
            var dispatchResult = await _smtpMailSender.SendAsync(drill.MailRequest, cancellationToken);

            var acceptedResponse = request.CreateResponse(HttpStatusCode.Accepted);
            acceptedResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await acceptedResponse.WriteStringAsync(JsonSerializer.Serialize(new
            {
                message = "Security remediation dummy drill mail executed successfully.",
                storyId = drill.StoryId,
                trackingId = drill.TrackingId,
                repositoryScope = drill.RepositoryScope,
                secretCategoryCount = drill.SecretCategoryCount,
                secretTypes = drill.SecretTypes,
                to = dispatchResult.RecipientEmail,
                subject = dispatchResult.Subject
            }, JsonOptions));

            return acceptedResponse;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Security remediation dummy drill was rejected because the configuration was incomplete.");
            return await CreateErrorResponseAsync(request, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Security remediation dummy drill failed while sending the message.");
            return await CreateErrorResponseAsync(request, HttpStatusCode.InternalServerError, "Security remediation dummy drill failed.");
        }
    }

    private static async Task<HttpResponseData> CreateErrorResponseAsync(HttpRequestData request, HttpStatusCode statusCode, string message)
    {
        var errorResponse = request.CreateResponse(statusCode);
        errorResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await errorResponse.WriteStringAsync(JsonSerializer.Serialize(new
        {
            error = message
        }, JsonOptions));

        return errorResponse;
    }
}

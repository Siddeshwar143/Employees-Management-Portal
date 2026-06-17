using System.Net;
using System.Text.Json;
using EmployeesManagement.MailFunctionApp.Models;
using EmployeesManagement.MailFunctionApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EmployeesManagement.MailFunctionApp.Functions;

public sealed class SendMailFunction(ISmtpMailSender smtpMailSender, ILogger<SendMailFunction> logger)
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true
    };

    private readonly ISmtpMailSender _smtpMailSender = smtpMailSender;
    private readonly ILogger<SendMailFunction> _logger = logger;

    [Function("SendMailFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "mail/send")] HttpRequestData request,
        CancellationToken cancellationToken)
    {
        MailTriggerRequest payload;

        using (var reader = new StreamReader(request.Body))
        {
            var requestBody = await reader.ReadToEndAsync();
            payload = string.IsNullOrWhiteSpace(requestBody)
                ? new MailTriggerRequest()
                : JsonSerializer.Deserialize<MailTriggerRequest>(requestBody, JsonOptions) ?? new MailTriggerRequest();
        }

        try
        {
            var result = await _smtpMailSender.SendAsync(payload, cancellationToken);
            var acceptedResponse = request.CreateResponse(HttpStatusCode.Accepted);
            acceptedResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await acceptedResponse.WriteStringAsync(JsonSerializer.Serialize(new
            {
                message = "Mail trigger executed successfully.",
                to = result.RecipientEmail,
                subject = result.Subject
            }, JsonOptions));

            return acceptedResponse;
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Mail trigger rejected because the configuration or request was incomplete.");
            return await CreateErrorResponseAsync(request, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Mail trigger failed while sending the message.");
            return await CreateErrorResponseAsync(request, HttpStatusCode.InternalServerError, "Mail trigger failed.");
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

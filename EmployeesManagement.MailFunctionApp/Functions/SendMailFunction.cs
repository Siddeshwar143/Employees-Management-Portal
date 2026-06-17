using System.Net;
using System.Text.Json;
using EmployeesManagement.MailFunctionApp.Models;
using EmployeesManagement.MailFunctionApp.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EmployeesManagement.MailFunctionApp.Functions;

public sealed class SendMailFunction(ISmtpMailSender smtpMailSender, ILogger<SendMailFunction> logger)
***REMOVED***
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    ***REMOVED***
        WriteIndented = true
  ***REMOVED***;

    private readonly ISmtpMailSender _smtpMailSender = smtpMailSender;
    private readonly ILogger<SendMailFunction> _logger = logger;

    [Function("SendMailFunction")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "mail/send")] HttpRequestData request,
        CancellationToken cancellationToken)
    ***REMOVED***
        MailTriggerRequest payload;

        using (var reader = new StreamReader(request.Body))
        ***REMOVED***
            var requestBody = await reader.ReadToEndAsync();
            payload = string.IsNullOrWhiteSpace(requestBody)
                ? new MailTriggerRequest()
                : JsonSerializer.Deserialize<MailTriggerRequest>(requestBody, JsonOptions) ?? new MailTriggerRequest();
      ***REMOVED***

        try
        ***REMOVED***
            var result = await _smtpMailSender.SendAsync(payload, cancellationToken);
            var acceptedResponse = request.CreateResponse(HttpStatusCode.Accepted);
            acceptedResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
            await acceptedResponse.WriteStringAsync(JsonSerializer.Serialize(new
            ***REMOVED***
                message = "Mail trigger executed successfully.",
                to = result.RecipientEmail,
                subject = result.Subject
          ***REMOVED*** JsonOptions));

            return acceptedResponse;
      ***REMOVED***
        catch (InvalidOperationException ex)
        ***REMOVED***
            _logger.LogWarning(ex, "Mail trigger rejected because the configuration or request was incomplete.");
            return await CreateErrorResponseAsync(request, HttpStatusCode.BadRequest, ex.Message);
      ***REMOVED***
        catch (Exception ex)
        ***REMOVED***
            _logger.LogError(ex, "Mail trigger failed while sending the message.");
            return await CreateErrorResponseAsync(request, HttpStatusCode.InternalServerError, "Mail trigger failed.");
      ***REMOVED***
  ***REMOVED***

    private static async Task<HttpResponseData> CreateErrorResponseAsync(HttpRequestData request, HttpStatusCode statusCode, string message)
    ***REMOVED***
        var errorResponse = request.CreateResponse(statusCode);
        errorResponse.Headers.Add("Content-Type", "application/json; charset=utf-8");
        await errorResponse.WriteStringAsync(JsonSerializer.Serialize(new
        ***REMOVED***
            error = message
      ***REMOVED*** JsonOptions));

        return errorResponse;
  ***REMOVED***
***REMOVED***

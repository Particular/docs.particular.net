namespace AzureFunctions.ServiceBus;

using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;

#region service-bus-http-sender
public class HttpSender([FromKeyedServices("client")] IMessageSession session)
{
    [Function(nameof(HttpSender))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData request)
    {
        await session.Send(new TriggerMessage());

        var response = request.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync($"{nameof(TriggerMessage)} sent.");
        return response;
    }
}
#endregion

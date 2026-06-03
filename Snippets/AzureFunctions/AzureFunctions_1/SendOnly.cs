namespace AzureFunctions_1;

using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

class SendOnlyRegistration
{
    #region azure-functions-sendonly-registration
    [NServiceBusSendOnlyFunction("client", Connection = "AzureWebJobsServiceBus")]
    public static void ConfigureClient(EndpointConfiguration endpointConfiguration, IServiceCollection services)
    {
        services.AddSingleton(new MyComponent("client"));

        var transport = new AzureServiceBusServerlessTransport(TopicTopology.Default);
        var routing = endpointConfiguration.UseTransport(transport);
        routing.RouteToEndpoint(typeof(SubmitOrder), "sales");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    }
    #endregion

    record MyComponent(string EndpointName);
}

#region azure-functions-sendonly-usage
class SalesApi([FromKeyedServices("client")] IMessageSession session, [FromKeyedServices("client")] MyComponent component)
{
    [Function("SalesApi")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request)
    {
        await session.Send(new SubmitOrder());
        var response = request.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync(component.EndpointName);
        return response;
    }
}
#endregion

class SubmitOrder : ICommand;

namespace AzureFunctions_1;

using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

class SendOnlyRegistration
{
    #region azure-functions-sendonly-registration
    static void ConfigureSendOnly(FunctionsApplicationBuilder builder)
    {
        builder.AddSendOnlyNServiceBusEndpoint("client", (configuration, services) =>
        {
            var transport = new AzureServiceBusServerlessTransport(TopicTopology.Default)
            {
                ConnectionName = "ServiceBusConnection"
            };
            var routing = configuration.UseTransport(transport);
            routing.RouteToEndpoint(typeof(SubmitOrder), "sales");
            configuration.UseSerialization<SystemJsonSerializer>();
        });
    }
    #endregion
}

#region azure-functions-sendonly-usage
class SalesApi([FromKeyedServices("client")] IMessageSession session)
{
    [Function("SalesApi")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData request)
    {
        await session.Send(new SubmitOrder());
        return request.CreateResponse(HttpStatusCode.OK);
    }
}
#endregion

class SubmitOrder : ICommand;

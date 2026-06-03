namespace AzureFunctions.ServiceBus;

using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

public static class ClientEndpoint
{
    [NServiceBusSendOnlyFunction("client", Connection = "AzureWebJobsServiceBus")]
    public static void ConfigureClient(EndpointConfiguration endpointConfiguration)
    {
        var transport = new AzureServiceBusServerlessTransport(TopicTopology.Default);

        var routing = endpointConfiguration.UseTransport(transport);
        routing.RouteToEndpoint(typeof(TriggerMessage), "Orders");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    }
}

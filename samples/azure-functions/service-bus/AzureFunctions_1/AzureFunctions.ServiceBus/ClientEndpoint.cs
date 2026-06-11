namespace AzureFunctions.ServiceBus;

using NServiceBus;

public static class ClientEndpoint
{
    [NServiceBusSendOnlyFunction("client")]
    public static void ConfigureClient(EndpointConfiguration endpointConfiguration)
    {
        var transport = new AzureServiceBusServerlessTransport(TopicTopology.Default);

        var routing = endpointConfiguration.UseTransport(transport);

        routing.RouteToEndpoint(typeof(TriggerMessage), "Orders");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    }
}
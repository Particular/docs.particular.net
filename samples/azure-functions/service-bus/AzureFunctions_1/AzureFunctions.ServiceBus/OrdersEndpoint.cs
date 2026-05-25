namespace AzureFunctions.ServiceBus;

using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

#region service-bus-endpoint
[NServiceBusFunction]
public partial class OrdersEndpoint
{
    [Function(nameof(Orders))]
    public partial Task Orders(
        [ServiceBusTrigger("orders", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureOrders(EndpointConfiguration endpoint)
    {
        endpoint.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        endpoint.UseSerialization<SystemJsonSerializer>();
        endpoint.AddHandler<TriggerMessageHandler>();
        endpoint.AddHandler<FollowupMessageHandler>();
        endpoint.EnableInstallers();
    }
}
#endregion

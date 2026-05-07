namespace AzureFunctions_1;

using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

#region azure-functions-basic-endpoint
[NServiceBusFunction]
public partial class OrdersEndpoint
{
    [Function(nameof(Orders))]
    public partial Task Orders(
        [ServiceBusTrigger("orders", Connection = "ServiceBusConnection", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureOrders(EndpointConfiguration endpoint)
    {
        endpoint.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        endpoint.UseSerialization<SystemJsonSerializer>();
        endpoint.AddHandler<PlaceOrderHandler>();
    }
}
#endregion

class PlaceOrder : ICommand;

class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context) => Task.CompletedTask;
}

namespace AzureFunctions_1;

using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using NServiceBus;

#region azure-functions-basic-endpoint
public partial class OrdersEndpoint
{
    [Function("Orders")]
    [NServiceBusFunction]
    public partial Task Orders(
        [ServiceBusTrigger("orders", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureOrders(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.AddHandler<PlaceOrderHandler>();
    }
}
#endregion

class PlaceOrder : ICommand;

class PlaceOrderHandler : IHandleMessages<PlaceOrder>
{
    public Task Handle(PlaceOrder message, IMessageHandlerContext context) => Task.CompletedTask;
}

namespace AzureFunctions_1;

using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

#region azure-functions-configure-with-services
[NServiceBusFunction]
public partial class ShippingEndpoint
{
    [Function("Shipping")]
    public partial Task Shipping(
        [ServiceBusTrigger("shipping", Connection = "ServiceBusConnection", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureShipping(EndpointConfiguration configuration, IServiceCollection services)
    {
        configuration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        configuration.UseSerialization<SystemJsonSerializer>();
        services.AddSingleton(new MyComponent("Shipping"));
        configuration.AddHandler<ShipOrderHandler>();
    }
}
#endregion

class ShipOrder : ICommand;

class ShipOrderHandler : IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context) => Task.CompletedTask;
}

record MyComponent(string EndpointName);

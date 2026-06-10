namespace AzureFunctions_1;

using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

#region azure-functions-configure-with-services
public partial class ShippingEndpoint
{
    [Function("Shipping")]
    [NServiceBusFunction]
    public partial Task Shipping(
        [ServiceBusTrigger("shipping", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureShipping(
        EndpointConfiguration endpointConfiguration,
        IServiceCollection services,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        endpointConfiguration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        services.AddSingleton(new MyComponent("Shipping"));
        endpointConfiguration.AddHandler<ShipOrderHandler>();

        if (environment.IsProduction())
        {
            endpointConfiguration.AuditProcessedMessagesTo(configuration["audit-queue"] ?? "audit");
        }
    }
}
#endregion

class ShipOrder : ICommand;

class ShipOrderHandler : IHandleMessages<ShipOrder>
{
    public Task Handle(ShipOrder message, IMessageHandlerContext context) => Task.CompletedTask;
}

record MyComponent(string EndpointName);

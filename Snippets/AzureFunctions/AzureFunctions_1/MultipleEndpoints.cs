namespace AzureFunctions_1;

using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

#region azure-functions-multiple-endpoints
public partial class BillingFunctions
{
    [Function("BillingApi")]
    [NServiceBusFunction]
    public partial Task BillingApi(
        [ServiceBusTrigger("billing-api", Connection = "ServiceBusConnection", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureBillingApi(EndpointConfiguration configuration)
    {
        configuration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        configuration.UseSerialization<SystemJsonSerializer>();
        configuration.AddHandler<ProcessPaymentHandler>();
    }

    [Function("BillingBackend")]
    [NServiceBusFunction]
    public partial Task BillingBackend(
        [ServiceBusTrigger("billing-backend", Connection = "ServiceBusConnection", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureBillingBackend(
        EndpointConfiguration endpointConfiguration,
        IConfiguration configuration,
        IHostEnvironment environment)
    {
        endpointConfiguration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.AddHandler<PaymentChargedHandler>();

        if (environment.IsProduction())
        {
            endpointConfiguration.AuditProcessedMessagesTo(configuration["audit-queue"] ?? "audit");
        }
    }
}
#endregion

class ProcessPayment : ICommand;

class PaymentCharged : ICommand;

class ProcessPaymentHandler : IHandleMessages<ProcessPayment>
{
    public Task Handle(ProcessPayment message, IMessageHandlerContext context) => Task.CompletedTask;
}

class PaymentChargedHandler : IHandleMessages<PaymentCharged>
{
    public Task Handle(PaymentCharged message, IMessageHandlerContext context) => Task.CompletedTask;
}

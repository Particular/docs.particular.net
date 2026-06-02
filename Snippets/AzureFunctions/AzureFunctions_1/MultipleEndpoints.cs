namespace AzureFunctions_1;

using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using NServiceBus;
using NServiceBus.Transport.AzureServiceBus;

#region azure-functions-multiple-endpoints
public partial class BillingFunctions
{
    [Function("BillingApi")]
    [NServiceBusFunction]
    public partial Task BillingApi(
        [ServiceBusTrigger("billing-api", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureBillingApi(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.AddHandler<ProcessPaymentHandler>();
    }

    [Function("BillingBackend")]
    [NServiceBusFunction]
    public partial Task BillingBackend(
        [ServiceBusTrigger("billing-backend", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureBillingBackend(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.AddHandler<PaymentChargedHandler>();
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

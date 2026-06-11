namespace AzureFunctions_1;

using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using NServiceBus;

#region azure-functions-multiple-endpoints
public partial class BillingFunctions
{
    [Function("Invoicing")]
    [NServiceBusFunction]
    public partial Task Invoicing(
        [ServiceBusTrigger("billing-invoicing", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureInvoicing(EndpointConfiguration endpointConfiguration)
    {
        endpointConfiguration.UseTransport(new AzureServiceBusServerlessTransport(TopicTopology.Default));
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.AddHandler<ProcessPaymentHandler>();
    }

    [Function("CardProcessing")]
    [NServiceBusFunction]
    public partial Task CardProcessing(
        [ServiceBusTrigger("billing-card-processing", AutoCompleteMessages = false)]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions,
        FunctionContext functionContext,
        CancellationToken cancellationToken = default);

    public static void ConfigureCardProcessing(EndpointConfiguration endpointConfiguration)
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

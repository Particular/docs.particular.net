using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class ProcessPaymentHandler(ILogger<ProcessPaymentHandler> logger) : IHandleMessages<ProcessPayment>
{
    public Task Handle(ProcessPayment message, IMessageHandlerContext context)
    {
        logger.LogInformation("Processing payment for OrderId {OrderId}", message.OrderId);

        var paymentCompleted = new PaymentCompleted
        {
            OrderId = message.OrderId
        };

        return context.Publish(paymentCompleted);
    }
}

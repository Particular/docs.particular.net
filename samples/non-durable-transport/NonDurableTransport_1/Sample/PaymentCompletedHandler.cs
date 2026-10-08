using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;

public class PaymentCompletedHandler(ILogger<PaymentCompletedHandler> logger) : IHandleMessages<PaymentCompleted>
{
    public Task Handle(PaymentCompleted message, IMessageHandlerContext context)
    {
        logger.LogInformation("PaymentCompletedHandler received event for OrderId {OrderId}", message.OrderId);
        return Task.CompletedTask;
    }
}

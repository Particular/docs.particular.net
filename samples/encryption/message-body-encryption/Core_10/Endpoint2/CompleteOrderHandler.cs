using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.Logging;

public class CompleteOrderHandler(ILogger<CompleteOrderHandler> logger) :
    IHandleMessages<CompleteOrder>
{

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        logger.LogInformation("Received CompleteOrder with credit card number {CreditCard}", message.CreditCard);
        return Task.CompletedTask;
    }
}
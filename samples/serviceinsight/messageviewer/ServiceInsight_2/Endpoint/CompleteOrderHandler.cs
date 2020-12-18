using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class CompleteOrderHandler :
    IHandleMessages<CompleteOrder>
{
    static ILog log = LogManager.GetLogger<CompleteOrderHandler>();

    public Task Handle(CompleteOrder message, IMessageHandlerContext context)
    {
        log.Info($"Received CompleteOrder with credit card number {message.CreditCard}");
        return Task.CompletedTask;
    }
}
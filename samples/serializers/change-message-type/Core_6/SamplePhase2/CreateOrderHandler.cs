using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class CreateOrderHandler :
    IHandleMessages<CreateOrderPhase2>
{
    static ILog log = LogManager.GetLogger<CreateOrderHandler>();

    public Task Handle(CreateOrderPhase2 message, IMessageHandlerContext context)
    {
        log.Info("Order received");
        return Task.CompletedTask;
    }
}
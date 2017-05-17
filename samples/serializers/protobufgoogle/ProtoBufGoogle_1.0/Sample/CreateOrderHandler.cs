using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;

public class CreateOrderHandler :
    IHandleMessages<CreateOrder>
{
    static ILog log = LogManager.GetLogger<CreateOrderHandler>();

    public Task Handle(CreateOrder message, IMessageHandlerContext context)
    {
        log.Info("Order received");
        return Task.CompletedTask;
    }
}
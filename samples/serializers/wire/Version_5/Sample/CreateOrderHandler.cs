using NServiceBus;
using NServiceBus.Logging;

public class CreateOrderHandler : IHandleMessages<CreateOrder>
{
    static ILog log = LogManager.GetLogger<CreateOrderHandler>();
    public void Handle(CreateOrder message)
    {
        log.Info("Order received");
    }
}

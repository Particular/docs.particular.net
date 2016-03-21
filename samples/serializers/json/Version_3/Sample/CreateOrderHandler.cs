using log4net;
using NServiceBus;

public class CreateOrderHandler : IHandleMessages<CreateOrder>
{
    static ILog log = LogManager.GetLogger(typeof(CreateOrderHandler));

    public void Handle(CreateOrder message)
    {
        log.Info("Order received");
    }
}

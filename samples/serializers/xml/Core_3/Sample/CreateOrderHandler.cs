using log4net;
using NServiceBus;
using XmlSample;

public class CreateOrderHandler : IHandleMessages<CreateOrder>
{
    static ILog log = LogManager.GetLogger(typeof(CreateOrderHandler));

    public void Handle(CreateOrder message)
    {
        log.Info("Order received");
    }
}

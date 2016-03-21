using log4net;
using NServiceBus;

public class CompleteOrderHandler : IHandleMessages<CompleteOrder>
{
    static ILog log = LogManager.GetLogger(typeof(CompleteOrderHandler));

    public void Handle(CompleteOrder message)
    {
        log.Info("Received CompleteOrder with credit card number " + message.CreditCard);
    }
}
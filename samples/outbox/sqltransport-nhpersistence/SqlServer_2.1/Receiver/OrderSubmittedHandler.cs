using System;
using NServiceBus;
using NHibernate;
using NServiceBus.Logging;

public class OrderSubmittedHandler :
    IHandleMessages<OrderSubmitted>
{
    IBus bus;
    ISession session;
    static ILog log = LogManager.GetLogger<OrderSubmittedHandler>();
    static readonly Random ChaosGenerator = new Random();

    public OrderSubmittedHandler(IBus bus, ISession session)
    {
        this.bus = bus;
        this.session = session;
    }

    public void Handle(OrderSubmitted message)
    {
        log.Info($"Order {message.OrderId} worth {message.Value} submitted");

        #region StoreUserData

        session.Save(new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        });

        #endregion

        #region Reply

        bus.Reply(new OrderAccepted
        {
            OrderId = message.OrderId,
        });

        #endregion

        if (ChaosGenerator.Next(4) == 0)
        {
            throw new Exception("Boom!");
        }

    }
}
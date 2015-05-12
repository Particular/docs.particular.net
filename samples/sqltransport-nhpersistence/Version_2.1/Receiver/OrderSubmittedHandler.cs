using System;
using NServiceBus;
using NHibernate;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    public IBus Bus { get; set; }
    public ISession Session { get; set; }

    public void Handle(OrderSubmitted message)
    {
        Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);

        #region StoreUserData

        Session.Save(new Order
        {
            OrderId = message.OrderId,
            Value = message.Value
        });

        #endregion

        #region Reply

        Bus.Reply(new OrderAccepted
        {
            OrderId = message.OrderId,
        });

        #endregion
    }
}
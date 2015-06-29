using System;
using NServiceBus;

public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
{
    public IBus Bus { get; set; }
    private readonly ReceiverDataContext ctx;

    public OrderSubmittedHandler(ReceiverDataContext ctx)
    {
        this.ctx = ctx;
    }

    public void Handle(OrderSubmitted message)
    {
        Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);

        #region StoreUserData

        ctx.Orders.Add(new Order
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

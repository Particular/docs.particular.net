using System;
using Messages;
using NServiceBus;

namespace Receiver
{
    public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
    {
        public IBus Bus { get; set; }

        public void Handle(OrderSubmitted message)
        {
            Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);
            #region StoreUserData
            using (var ctx = new ReceiverDataContext())
            {
                ctx.Orders.Add(new Order()
                {
                    OrderId = message.OrderId,
                    Value = message.Value
                });
                ctx.SaveChanges();
            }
            #endregion
            #region Reply
            Bus.Reply(new OrderAccepted()
            {
                OrderId = message.OrderId,
            });
            #endregion
        }
    }
}
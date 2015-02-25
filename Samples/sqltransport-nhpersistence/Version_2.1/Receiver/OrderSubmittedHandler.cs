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
            using (var session = Program.SessionFactory.OpenSession())
            using (var tx = session.BeginTransaction())
            {
                session.Save(new Order()
                {
                    OrderId = message.OrderId,
                    Value = message.Value
                });
                tx.Commit();
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
using System;
using Messages;
using NServiceBus;
using NServiceBus.Persistence.NHibernate;

namespace Receiver
{
    public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
    {
        public IBus Bus { get; set; }
        public NHibernateStorageContext StorageContext { get; set; }

        public void Handle(OrderSubmitted message)
        {
            Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);

            #region StoreUserData

            StorageContext.Session.Save(new Order
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
}
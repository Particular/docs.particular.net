using System;
using Messages;
using NServiceBus;

namespace Receiver
{
    using System.Data.Common;
    using NHibernate.Transaction;
    using NServiceBus.Persistence.NHibernate;

    public class OrderSubmittedHandler : IHandleMessages<OrderSubmitted>
    {
        public IBus Bus { get; set; }
        public NHibernateStorageContext StorageContext { get; set; }

        public void Handle(OrderSubmitted message)
        {
            Console.WriteLine("Order {0} worth {1} submitted", message.OrderId, message.Value);

            #region StoreUserData

            using (ReceiverDataContext ctx = new ReceiverDataContext(StorageContext.Connection))
            {
                ctx.Database.UseTransaction((DbTransaction) StorageContext.DatabaseTransaction);
                ctx.Orders.Add(new Order
                               {
                                   OrderId = message.OrderId,
                                   Value = message.Value
                               });
                ctx.SaveChanges();
            }

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
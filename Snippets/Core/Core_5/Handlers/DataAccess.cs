namespace Core5.Handlers
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Persistence;

    public class DataAccess
    {
        public class Simple
        {

            #region BusinessData-Native-NotIdempotent

            public class NonIdempotentHandler : IHandleMessages<AddOrderLine>
            {
                public IMyOrm Orm { get; set; }

                public void Handle(AddOrderLine message)
                {
                    using (var session = Orm.OpenSession())
                    {
                        var order = session.Get(message.OrderId);
                        order.AddLine(message.Product, message.Quantity);
                        session.Commit();
                    }
                }
            }

            #endregion

            #region BusinessData-Native-Idempotent

            public class IdempotentHandler : IHandleMessages<AddOrderLine>
            {
                public IMyOrm Orm { get; set; }

                public void Handle(AddOrderLine message)
                {
                    using (var session = Orm.OpenSession())
                    {
                        var order = session.Get(message.OrderId);
                        if (order.HasLine(message.LineId))
                        {
                            return;
                        }

                        order.AddLine(message.Product, message.Quantity);
                        session.Commit();
                    }
                }
            }

            #endregion
        }

        public class Managed
        {
            #region BusinessData-Native-Managed

            public class IdempotencyEnforcer : IHandleMessages<OrderMessage>
            {
                public IBus Bus { get; set; }

                public void Handle(OrderMessage message)
                {
                    var session = Bus.MyOrmSession();
                    var order = session.Get(message.OrderId);
                    if (order.HasProcessed(Bus.CurrentMessageContext.Id))
                    {
                        //Subsequent handlers are not invoked because the message has already been processed.
                        Bus.DoNotContinueDispatchingCurrentMessageToHandlers();
                    }
                    else
                    {
                        order.MarkAsProcessed(Bus.CurrentMessageContext.Id);
                    }
                }
            }

            public class NonIdempotentHandler : IHandleMessages<AddOrderLine>
            {
                public IBus Bus { get; set; }

                public void Handle(AddOrderLine message)
                {
                    var session = Bus.MyOrmSession();
                    var order = session.Get(message.OrderId);
                    order.AddLine(message.Product, message.Quantity);
                }
            }

            public void ConfigureEndpoint(BusConfiguration config)
            {
                config.LoadMessageHandlers<IdempotencyEnforcer>();
                config.UsePersistence<MyPersistence>();
            }

            #endregion
        }

        public class OrderMessage : IMessage
        {
            public object OrderId { get; set; }
        }

        public class AddOrderLine : OrderMessage
        {
            public object Product { get; set; }
            public object Quantity { get; set; }
            public object LineId { get; set; }
        }

        public interface IMyOrm
        {
            IMyOrmSession OpenSession();
        }

        public interface IMyOrmSession : IDisposable
        {
            Order Get(object orderId);
            void Commit();
        }

        public class Order
        {
            public void AddLine(object product, object quantity)
            {
                throw new NotImplementedException();
            }

            public bool HasLine(object lineId)
            {
                throw new NotImplementedException();
            }

            public bool HasProcessed(string id)
            {
                throw new NotImplementedException();
            }

            public void MarkAsProcessed(string id)
            {
                throw new NotImplementedException();
            }
        }

        public class MyPersistence : PersistenceDefinition
        {
        }
    }

    public static class MyOrmExtensions
    {
        public static DataAccess.IMyOrmSession MyOrmSession(this IBus b)
        {
            throw new NotImplementedException();
        }
    }
}
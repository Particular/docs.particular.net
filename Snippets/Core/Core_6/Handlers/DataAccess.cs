namespace Core6.Handlers
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

                public async Task Handle(AddOrderLine message, IMessageHandlerContext context)
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

                public async Task Handle(AddOrderLine message, IMessageHandlerContext context)
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
                public Task Handle(OrderMessage message, IMessageHandlerContext context)
                {
                    var session = context.SynchronizedStorageSession.MyOrmSession();
                    var order = session.Get(message.OrderId);
                    if (this.MessageHasAlreadyBeenProcessed(context.MessageId, order))
                    {
                        //Subsequent handlers are not invoked because the message has already been processed.
                        context.DoNotContinueDispatchingCurrentMessageToHandlers();
                    }
                    else
                    {
                        this.MarkAsProcessed(context.MessageId, order);
                    }
                    return Task.FromResult(0);
                }

                
            }

            public class NonIdempotentHandler : IHandleMessages<AddOrderLine>
            {
                public async Task Handle(AddOrderLine message, IMessageHandlerContext context)
                {
                    var session = context.SynchronizedStorageSession.MyOrmSession();
                    var order = session.Get(message.OrderId);
                    order.AddLine(message.Product, message.Quantity);
                }
            }

            public void ConfigureEndpoint(EndpointConfiguration config)
            {
                config.ExecuteTheseHandlersFirst(typeof(IdempotencyEnforcer));
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
        }
        public class MyPersistence : PersistenceDefinition
        {
        }
    }

    public static class MyOrmExtensions
    {
        public static DataAccess.IMyOrmSession MyOrmSession(this SynchronizedStorageSession s)
        {
            throw new NotImplementedException();
        }

        public static bool MessageHasAlreadyBeenProcessed(this DataAccess.Managed.IdempotencyEnforcer o, string messageId, DataAccess.Order order)
        {
            throw new NotImplementedException();
        }

        public static void MarkAsProcessed(this DataAccess.Managed.IdempotencyEnforcer o, string messageId, DataAccess.Order order)
        {
            throw new NotImplementedException();
        }
    }
}
namespace Core6.Handlers.DataAccess
{
    using System.Threading.Tasks;
    using NServiceBus;

    public class Managed
    {
        #region BusinessData-Native-Managed

        public class IdempotencyEnforcer :
            IHandleMessages<OrderMessage>
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
                return Task.CompletedTask;
            }
        }

        public class NonIdempotentHandler :
            IHandleMessages<AddOrderLine>
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
}
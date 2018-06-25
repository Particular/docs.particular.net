namespace Core7.Handlers.DataAccess
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
                if (MessageHasAlreadyBeenProcessed(context.MessageId, order))
                {
                    // Subsequent handlers are not invoked as the message has been processed.
                    context.DoNotContinueDispatchingCurrentMessageToHandlers();
                }
                else
                {
                    this.MarkAsProcessed(context.MessageId, order);
                }
                return Task.CompletedTask;
            }
            
            bool MessageHasAlreadyBeenProcessed(string messageId, Order order)
            {
                // Custom code to determine of this message have already been processed
                return false;
            }
        }

        public class NonIdempotentHandler :
            IHandleMessages<AddOrderLine>
        {
            public Task Handle(AddOrderLine message, IMessageHandlerContext context)
            {
                var session = context.SynchronizedStorageSession.MyOrmSession();
                var order = session.Get(message.OrderId);
                order.AddLine(message.Product, message.Quantity);
                return Task.CompletedTask;
            }
        }

        public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            endpointConfiguration.ExecuteTheseHandlersFirst(typeof(IdempotencyEnforcer));
            endpointConfiguration.UsePersistence<MyPersistence>();
        }

        #endregion
    }
}

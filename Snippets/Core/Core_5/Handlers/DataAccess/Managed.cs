namespace Core5.Handlers
{
    using NServiceBus;

    public class Managed
    {
        #region BusinessData-Native-Managed

        public class IdempotencyEnforcer :
            IHandleMessages<OrderMessage>
        {
            IBus bus;

            public IdempotencyEnforcer(IBus bus)
            {
                this.bus = bus;
            }

            public void Handle(OrderMessage message)
            {
                var session = bus.MyOrmSession();
                var order = session.Get(message.OrderId);
                if (MessageHasAlreadyBeenProcessed(bus.CurrentMessageContext.Id, order))
                {
                    // Subsequent handlers are not invoked as the message has been processed.
                    bus.DoNotContinueDispatchingCurrentMessageToHandlers();
                }
                else
                {
                    this.MarkAsProcessed(bus.CurrentMessageContext.Id, order);
                }
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
            IBus bus;

            public NonIdempotentHandler(IBus bus)
            {
                this.bus = bus;
            }

            public void Handle(AddOrderLine message)
            {
                var session = bus.MyOrmSession();
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
}

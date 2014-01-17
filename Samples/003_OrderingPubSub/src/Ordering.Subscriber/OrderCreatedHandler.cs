namespace Ordering.Subscriber
{
    using System;
    using Messages;
    using NServiceBus;

    public class OrderCreatedHandler : IHandleMessages<OrderPlaced>
    {
        public IBus Bus { get; set; }

        public void Handle(OrderPlaced message)
        {
            Console.WriteLine(@"Handling: OrderPlaceed for Order Id: {0}", message.OrderId);
        }
    }
}

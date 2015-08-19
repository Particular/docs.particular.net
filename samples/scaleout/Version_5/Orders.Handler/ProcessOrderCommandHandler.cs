namespace Orders.Handler
{
    using NServiceBus;
    using System;
    using Orders.Commands;
    using Orders.Events;

    public class ProcessOrderCommandHandler : IHandleMessages<PlaceOrder>
    {
        IBus bus;

        public ProcessOrderCommandHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(PlaceOrder placeOrder)
        {
            // Process Order...
            Console.Out.WriteLine("Processing received order....");

            bus.Publish<OrderPlaced>(m => m.OrderId = placeOrder.OrderId);
            Console.Out.WriteLine("Sent Order placed event for orderId [{0}].", placeOrder.OrderId);
        }
    }
}

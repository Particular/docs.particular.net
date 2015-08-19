namespace Orders.Handler
{
    using NServiceBus;
    using System;
    using Orders.Commands;
    using Orders.Events;

    public class ProcessOrderCommandHandler : IHandleMessages<PlaceOrder>
    {
        public IBus Bus { get; set; }
        public void Handle(PlaceOrder placeOrder)
        {
            // Process Order...
            Console.Out.WriteLine("Processing received order....");
            
            Bus.Publish<OrderPlaced>(m => m.OrderId = placeOrder.OrderId);
            Console.Out.WriteLine("Sent Order placed event for orderId [{0}].", placeOrder.OrderId);
        }
    }
}

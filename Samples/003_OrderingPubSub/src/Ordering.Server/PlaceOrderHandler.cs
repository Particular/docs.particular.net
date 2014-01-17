namespace Ordering.Server
{
    using System;
    using Messages;
    using NServiceBus;

    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        public IBus Bus { get; set; }

        public void Handle(PlaceOrder message)
        {
            Console.WriteLine(@"Order for Product:{0} placed with id: {1}", message.Product, message.Id);

            // throw new Exception("Uh oh - something went wrong....");

            Console.WriteLine(@"Publishing: OrderPlaceed for Order Id: {0}", message.Id);

            Bus.Publish<OrderPlaced>(e => {e.OrderId = message.Id;});
        }
    }
}
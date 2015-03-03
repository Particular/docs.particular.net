#region SendOrder
namespace Ordering.Client
{
    using System;
    using Messages;
    using NServiceBus;

    public class SendOrder : IWantToRunWhenBusStartsAndStops
    {
        IBus bus;

        public SendOrder(IBus bus)
        {
            this.bus = bus;
        }

        public void Start()
        {
            Console.WriteLine("Press 'Enter' to send a message.To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                Guid id = Guid.NewGuid();

                PlaceOrder placeOrder = new PlaceOrder
                                 {
                                     Product = "New shoes", 
                                     Id = id
                                 };
                bus.Send("Ordering.Server", placeOrder);

                Console.WriteLine("Send a new PlaceOrder message with id: {0}", id.ToString("N"));
            }
        }

        public void Stop()
        {
        }
    }
}
#endregion
namespace Orders.Sender
{
    using System;
    using NServiceBus;
    using Orders.Commands;
    using Orders.Messages;

    class ProcessOrderSender : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Console.WriteLine("Press 'Enter' to send a message. To exit, Ctrl + C");
            var counter = 0;
            while (Console.ReadLine() != null)
            {
                counter++;
                var placeOrder = new PlaceOrder
                                 {
                                     OrderId = "order" + counter
                                 };
                Bus.Send(placeOrder).Register(PlaceOrderReturnCodeHandler, this);
                Console.WriteLine("Sent PlacedOrder command with order id [{0}].", placeOrder.OrderId);
            }
        }

        static void PlaceOrderReturnCodeHandler(IAsyncResult asyncResult)
        {
            var result = (CompletionResult)asyncResult.AsyncState;
            Console.WriteLine("Received [{0}] Return code for Placing Order.", Enum.GetName(typeof (PlaceOrderStatus), result.ErrorCode));
        }

        public void Stop()
        {
            
        }
    }
}

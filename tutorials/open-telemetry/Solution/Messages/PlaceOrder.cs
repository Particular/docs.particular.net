using NServiceBus;

namespace Messages
{
    public class PlaceOrder : ICommand
    {
        public string CustomerId { get; set; }
        public string OrderId { get; set; }
    }
}
using NServiceBus;

namespace Orders.Commands
{
    public class PlaceOrder:IMessage
    {
        public string OrderId { get; set; }
    }
}

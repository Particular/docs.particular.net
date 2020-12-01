using NServiceBus;

namespace Messages
{
    public class ShipOrder :
        ICommand
    {
        public string OrderId { get; set; }
    }
}

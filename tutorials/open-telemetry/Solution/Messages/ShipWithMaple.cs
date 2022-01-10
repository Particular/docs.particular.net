using NServiceBus;

namespace Messages
{
    public class ShipWithMaple : ICommand
    {
        public string OrderId { get; set; }
    }
}
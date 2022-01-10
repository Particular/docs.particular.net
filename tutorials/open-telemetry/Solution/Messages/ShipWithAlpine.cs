using NServiceBus;

namespace Messages
{
    public class ShipWithAlpine : ICommand
    {
        public string OrderId { get; set; }
    }
}
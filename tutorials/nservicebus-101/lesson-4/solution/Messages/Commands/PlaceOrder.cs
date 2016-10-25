using NServiceBus;

namespace Messages.Commands
{
    public class PlaceOrder :
        ICommand
    {
        public string OrderId { get; set; }
    }
}

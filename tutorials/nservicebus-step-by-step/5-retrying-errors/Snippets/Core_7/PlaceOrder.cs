using NServiceBus;

namespace Core_6
{
    public class PlaceOrder :
        ICommand
    {
        public string OrderId { get; set; }
    }
}
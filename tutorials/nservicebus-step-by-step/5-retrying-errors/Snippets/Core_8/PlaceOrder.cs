using NServiceBus;

namespace Core_8
{
    public class PlaceOrder :
        ICommand
    {
        public string OrderId { get; set; }
    }
}
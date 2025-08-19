using NServiceBus;

namespace Core_9;

public class PlaceOrder :
    ICommand
{
    public string OrderId { get; set; }
}
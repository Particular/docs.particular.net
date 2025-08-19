using NServiceBus;

namespace Messages;

public class OrderBilled :
    IEvent
{
    public string OrderId { get; set; }
    public string CustomerId { get; set; }
    public decimal OrderValue { get; set; }
}
using NServiceBus;

class PlaceOrder : IMessage
{
    public Guid OrderId { get; set; }
    public string CustomerId { get; set; }
}

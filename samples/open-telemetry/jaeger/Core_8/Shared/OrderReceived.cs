namespace Shared;

public class OrderReceived : IEvent
{
    public Guid OrderId { get; set; }
}
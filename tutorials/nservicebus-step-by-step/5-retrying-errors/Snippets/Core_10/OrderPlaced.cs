namespace Core;

public class OrderPlaced : IEvent
{
    public string? OrderId { get; set; }
}
namespace Shared;

public class DeclineOrderMessage : IMessage
{
    public Guid OrderId { get; set; }
}
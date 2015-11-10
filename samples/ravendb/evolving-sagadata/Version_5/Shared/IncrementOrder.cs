using NServiceBus;

public class IncrementOrder : IMessage
{
    public int OrderId { get; set; }
}
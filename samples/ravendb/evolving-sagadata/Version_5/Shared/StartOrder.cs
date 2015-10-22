using NServiceBus;

public class StartOrder : IMessage
{
    public int OrderId { get; set; }
    public int ItemCount { get; set; }
}
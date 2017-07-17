using NServiceBus;

public class CompleteOrder
    : IMessage
{
    public string OrderId { get; set; }
}
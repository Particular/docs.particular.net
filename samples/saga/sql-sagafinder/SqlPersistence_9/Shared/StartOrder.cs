using NServiceBus;

public record StartOrder :
    IMessage
{
    public string OrderId { get; init; }
}
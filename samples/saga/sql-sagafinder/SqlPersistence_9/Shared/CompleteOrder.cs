using NServiceBus;

public record CompleteOrder :
    IMessage
{
    public string OrderId { get; init; }
}
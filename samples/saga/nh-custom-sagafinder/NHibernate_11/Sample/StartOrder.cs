public record StartOrder : IMessage
{
    public required string OrderId { get; init; }
}
public record CompleteOrder : IMessage
{
    public required string OrderId { get; init; }
}
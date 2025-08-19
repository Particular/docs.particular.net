using NServiceBus;

public record CancelOrder :
    ICommand
{
    public string OrderId { get; init; }
}
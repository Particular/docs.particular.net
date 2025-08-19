using NServiceBus;
public record PlaceOrder :
    ICommand
{
    public string OrderId { get; init; }
    public decimal Value { get; init; }
}
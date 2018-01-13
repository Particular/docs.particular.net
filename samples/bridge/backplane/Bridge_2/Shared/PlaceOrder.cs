using NServiceBus;

public class PlaceOrder :
    ICommand
{
    public string OrderId { get; set; }
    public decimal Value { get; set; }
}
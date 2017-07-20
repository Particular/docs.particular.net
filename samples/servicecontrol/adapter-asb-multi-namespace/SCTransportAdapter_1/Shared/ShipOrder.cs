using NServiceBus;

public class ShipOrder :
    ICommand
{
    public string OrderId { get; set; }
    public decimal Value { get; set; }
}
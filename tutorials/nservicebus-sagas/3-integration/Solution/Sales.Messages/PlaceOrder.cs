namespace Messages;

public class PlaceOrder :
    ICommand
{
    public string OrderId { get; set; }
    public string CustomerId { get; set; }
}
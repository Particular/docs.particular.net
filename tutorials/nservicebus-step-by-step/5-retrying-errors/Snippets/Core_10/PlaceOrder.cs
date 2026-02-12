namespace Core;

public class PlaceOrder : ICommand
{
    public string? OrderId { get; set; }
}
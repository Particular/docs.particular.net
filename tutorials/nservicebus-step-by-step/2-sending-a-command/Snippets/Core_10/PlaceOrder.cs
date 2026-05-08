#region PlaceOrder

namespace Messages;

public class PlaceOrder : ICommand
{
    public string? OrderId { get; set; }
}

#endregion
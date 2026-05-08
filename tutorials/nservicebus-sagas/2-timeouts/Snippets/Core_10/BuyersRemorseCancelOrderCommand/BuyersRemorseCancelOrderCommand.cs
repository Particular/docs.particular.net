#region BuyersRemorseCancelOrderCommand

public class CancelOrder : ICommand
{
    public string? OrderId { get; set; }
}
#endregion
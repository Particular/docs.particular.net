using NServiceBus;

namespace Core_9.BuyersRemorseCancelOrderCommand;

#region BuyersRemorseCancelOrderCommand
public class CancelOrder
    : ICommand
{
    public string OrderId { get; set; }
}
#endregion
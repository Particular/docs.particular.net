namespace Core_7.BuyersRemorseCancelOrderCommand
{
    using NServiceBus;

    #region BuyersRemorseCancelOrderCommand
    public class CancelOrder
        : ICommand
    {
        public string OrderId { get; set; }
    }
    #endregion
}

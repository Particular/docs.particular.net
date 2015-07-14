namespace Snippets4.WCF
{
    using NServiceBus;

    #region ExposeWCFService

    public class CancelOrderService : WcfService<CancelOrder, ErrorCodes>
    {
    }

    public class CancelOrderHandler : IHandleMessages<CancelOrder>
    {
        public void Handle(CancelOrder message)
        {
            // Write code here
        }
    }

    public enum ErrorCodes
    {
        Success,
        Fail
    }

    public class CancelOrder : ICommand
    {
        public int OrderId { get; set; }
    }

    #endregion
}

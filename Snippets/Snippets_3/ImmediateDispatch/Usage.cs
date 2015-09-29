namespace Snippets3.ImmediateDispatch
{
    using System.Transactions;
    using NServiceBus;

    class Usage
    {
        public void RequestImmediateDispatchUsingScope()
        {
            IBus bus = null;

            #region RequestImmediateDispatchUsingScope
            using (new TransactionScope(TransactionScopeOption.Suppress))
            {
                bus.Send(new MyMessage());
            }
            #endregion
        }

        class MyMessage
        {
        }
    }
}

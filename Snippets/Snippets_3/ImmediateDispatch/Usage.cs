namespace Snippets3.ImmediateDispatch
{
    using System.Transactions;
    using NServiceBus;

    class Usage
    {
        Usage(IBus bus)
        {
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
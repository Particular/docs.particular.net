namespace Core3.ImmediateDispatch
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
                var myMessage = new MyMessage();
                bus.Send(myMessage);
            }

            #endregion
        }

        class MyMessage
        {
        }
    }
}
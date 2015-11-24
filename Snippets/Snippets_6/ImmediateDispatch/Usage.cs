namespace Snippets6.ImmediateDispatch
{
    using System.Threading.Tasks;
    using System.Transactions;
    using NServiceBus;

    class Usage
    {
        public async Task RequestImmediateDispatch()
        {
            IBusContext busContext = null;

            #region RequestImmediateDispatch
            var options = new SendOptions();
            options.RequireImmediateDispatch();
            await busContext.Send(new MyMessage(), options);
            #endregion
        }

        public async Task RequestImmediateDispatchUsingScope()
        {
            IBusContext busContext = null;

            #region RequestImmediateDispatchUsingScope
            using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                await busContext.SendLocal(new MyMessage());
            }
            #endregion
        }

        class MyMessage
        {
        }
    }
}

namespace Snippets6.ImmediateDispatch
{
    using System.Threading.Tasks;
    using System.Transactions;
    using NServiceBus;

    class Usage
    {
        public async Task RequestImmediateDispatch()
        {
            IBus bus = null;

            #region RequestImmediateDispatch
            var options = new SendOptions();
            options.RequireImmediateDispatch();
            await bus.SendAsync(new MyMessage(), options);
            #endregion
        }

        public async Task RequestImmediateDispatchUsingScope()
        {
            IBus bus = null;

            #region RequestImmediateDispatchUsingScope
            using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                await bus.SendLocalAsync(new MyMessage());
            }
            #endregion
        }

        class MyMessage
        {
        }
    }
}

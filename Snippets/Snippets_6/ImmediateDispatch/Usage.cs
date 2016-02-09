namespace Snippets6.ImmediateDispatch
{
    using System.Threading.Tasks;
    using System.Transactions;
    using NServiceBus;

    class Usage
    {
        public async Task RequestImmediateDispatch()
        {
            IPipelineContext context = null;

            #region RequestImmediateDispatch
            SendOptions options = new SendOptions();
            options.RequireImmediateDispatch();
            await context.Send(new MyMessage(), options);
            #endregion
        }

        public async Task RequestImmediateDispatchUsingScope()
        {
            IPipelineContext context = null;

            #region RequestImmediateDispatchUsingScope
            using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            {
                await context.SendLocal(new MyMessage());
            }
            #endregion
        }

        class MyMessage
        {
        }
    }
}

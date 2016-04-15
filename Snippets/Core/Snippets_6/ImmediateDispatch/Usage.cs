namespace Snippets6.ImmediateDispatch
{
    using System.Threading.Tasks;
    using System.Transactions;
    using NServiceBus;

    class Usage
    {
        async Task RequestImmediateDispatch(IPipelineContext context)
        {
            #region RequestImmediateDispatch
            SendOptions options = new SendOptions();
            options.RequireImmediateDispatch();
            await context.Send(new MyMessage(), options);
            #endregion
        }

        async Task RequestImmediateDispatchUsingScope(IPipelineContext context)
        {
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

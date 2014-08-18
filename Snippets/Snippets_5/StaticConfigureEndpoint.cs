using NServiceBus;
using NServiceBus.Persistence;

public class StaticConfigureEndpoint
{
    public void Simple()
    {
        #region StaticConfigureEndpointReplacementV5

        // SendOnly
        Configure.With(b => b.SendOnly());

        // AsVolatile
        var configure = Configure.With(b =>
        {
            var transactions = b.Transactions();
            transactions.Disable();
            transactions.DoNotWrapHandlersExecutionInATransactionScope();
            transactions.DisableDistributedTransactions();
            b.DisableDurableMessages();
            b.UsePersistence<InMemory>();
        });

        // DisableDurableMessages

        Configure.With(b => b.DisableDurableMessages());

        // EnableDurableMessages
        Configure.With(b => b.EnableDurableMessages());

        #endregion
    }

}
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
            b.Transactions(transactionSettings =>
            {
                transactionSettings.Disable();
                transactionSettings.Advanced(advancedSettings =>
                {
                    advancedSettings.DoNotWrapHandlersExecutionInATransactionScope();
                    advancedSettings.DisableDistributedTransactions();
                });
            });
            b.DisableDurableMessages();
        });
        configure.UsePersistence<InMemory>();


        // DisableDurableMessages

        Configure.With(b => b.DisableDurableMessages());

        // EnableDurableMessages
        Configure.With(b => b.EnableDurableMessages());

        #endregion
    }

}
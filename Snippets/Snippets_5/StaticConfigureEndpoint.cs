using NServiceBus;
using NServiceBus.Persistence;

public class StaticConfigureEndpoint
{
    public void Simple()
    {
        #region StaticConfigureEndpointReplacementV5

        // SendOnly
        Configure.With().SendOnly();

        // AsVolatile
        var configure = Configure.With(builder =>
        {
            builder.Transactions(transactionSettings =>
            {
                transactionSettings.Disable();
                transactionSettings.Advanced(advancedSettings =>
                {
                    advancedSettings.DoNotWrapHandlersExecutionInATransactionScope();
                    advancedSettings.DisableDistributedTransactions();
                });
            });
            builder.DisableDurableMessages();
        });
        configure.UsePersistence<InMemory>();


        // DisableDurableMessages

        Configure.With(builder => builder.DisableDurableMessages());

        // EnableDurableMessages
        Configure.With(builder => builder.EnableDurableMessages());

        #endregion
    }

}
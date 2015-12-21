namespace Snippets6.UpgradeGuides._5to6
{
    using NServiceBus;
    using NServiceBus.Configuration.AdvanceExtensibility;
    using NServiceBus.ConsistencyGuarantees;

    public class Transactions
    {
        public Transactions()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6IsDtcTransactionsSuppressed
            if (busConfiguration.GetSettings().GetRequiredTransactionModeForReceives() != TransportTransactionMode.TransactionScope)
            {
                //your code here
            }
            #endregion

            #region 5to6IsTransactionsEnabled
            if (busConfiguration.GetSettings().GetRequiredTransactionModeForReceives() != TransportTransactionMode.None)
            {
                //your code here
            }
            #endregion
        }
    }
}
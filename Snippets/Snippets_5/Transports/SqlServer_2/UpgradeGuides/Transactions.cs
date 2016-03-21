namespace Snippets5.UpgradeGuides._4to5
{
    using NServiceBus;
    class Transactions
    {
        Transactions(BusConfiguration busConfiguration)
        {
            #region 2to3-enable-native-transaction

            busConfiguration
                .Transactions()
                .DisableDistributedTransactions();

            #endregion
        }
    }
}
namespace SqlServer_2.UpgradeGuides
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
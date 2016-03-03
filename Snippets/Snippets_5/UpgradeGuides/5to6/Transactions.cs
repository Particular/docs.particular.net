namespace Snippets5.UpgradeGuides._5to6
{
    using NServiceBus;

    class Transactions
    {
        public void SetDistributedTransactions()
        {
            #region 5to6-enable-native-transaction

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration
                .Transactions()
                .DisableDistributedTransactions();

            #endregion
        }
    }
}

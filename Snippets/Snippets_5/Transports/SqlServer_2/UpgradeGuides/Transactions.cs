namespace Snippets5.UpgradeGuides._4to5
{
    using NServiceBus;
    class Transactions
    {
        public void SetDistributedTransactions()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region 2to3-enable-native-transaction

            busConfiguration
                .Transactions()
                .DisableDistributedTransactions();

            #endregion
        }
    }
}
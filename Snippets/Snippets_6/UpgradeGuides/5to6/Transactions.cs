namespace Snippets5.UpgradeGuides._5to6
{
    using NServiceBus;

    class Transactions
    {
        public void SetDistributedTransactions()
        {
            #region 5to6-enable-native-transaction

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();

            endpointConfiguration
                .UseTransport<SqlServerTransport>()
                .Transactions(TransportTransactionMode.SendsAtomicWithReceive);

            #endregion
        }
    }
}

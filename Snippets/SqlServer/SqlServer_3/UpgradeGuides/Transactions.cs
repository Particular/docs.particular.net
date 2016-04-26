namespace SqlServer_3.UpgradeGuides
{
    using NServiceBus;

    class Transactions
    {
        Transactions(EndpointConfiguration endpointConfiguration)
        {
            #region 2to3-enable-native-transaction

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

            #endregion
        }
    }
}
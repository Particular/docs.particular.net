using NServiceBus;

class Transactions
{
    void NativeTransactions(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3-enable-native-transaction

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.Transactions(TransportTransactionMode.SendsAtomicWithReceive);

        #endregion
    }

    void DistributedTransactions(EndpointConfiguration endpointConfiguration)
    {
        #region 2to3-enable-ambient-transaction

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.Transactions(TransportTransactionMode.TransactionScope);

        #endregion
    }
}
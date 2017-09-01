using NServiceBus;

class Transactions
{
    Transactions(BusConfiguration busConfiguration)
    {
        #region 2to3-enable-native-transaction

        var transactions = busConfiguration.Transactions();
        transactions.DisableDistributedTransactions();

        #endregion
    }
}
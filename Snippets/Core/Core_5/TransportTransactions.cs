namespace Core5
{
    using System;
    using System.Transactions;
    using NServiceBus;

    class TransportTransactions
    {
        void Unreliable(BusConfiguration busConfiguration)
        {
            #region TransactionsDisable

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.Disable();

            #endregion
        }

        void TransportTransactionReceiveOnly(BusConfiguration busConfiguration)
        {
            #region TransportTransactionReceiveOnly

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.DisableDistributedTransactions();

            #endregion
        }

        void TransportTransactionScope(BusConfiguration busConfiguration)
        {
            #region TransportTransactionScope

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.Enable();
            transactionSettings.EnableDistributedTransactions();

            #endregion
        }

        void TransportTransactionsWithScope(BusConfiguration busConfiguration)
        {
            #region TransactionsWrapHandlersExecutionInATransactionScope

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.DisableDistributedTransactions();
            transactionSettings.WrapHandlersExecutionInATransactionScope();

            #endregion
        }


        void CustomTransactionTimeout(BusConfiguration busConfiguration)
        {
            #region CustomTransactionTimeout

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.DefaultTimeout(TimeSpan.FromSeconds(30));

            #endregion
        }

        void CustomTransactionIsolationLevel(BusConfiguration busConfiguration)
        {
            #region CustomTransactionIsolationLevel

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.IsolationLevel(IsolationLevel.RepeatableRead);

            #endregion
        }
    }
}
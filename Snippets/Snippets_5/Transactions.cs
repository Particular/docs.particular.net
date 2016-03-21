namespace Snippets5
{
    using System;
    using System.Transactions;
    using NServiceBus;

    class Transactions
    {
        void Unreliable(BusConfiguration busConfiguration)
        {
            #region TransactionsDisable
            busConfiguration.Transactions()
                .Disable();
            #endregion
        }

        void TransportTransactionReceiveOnly(BusConfiguration busConfiguration)
        {
            #region TransportTransactionReceiveOnly
            busConfiguration.Transactions()
                .DisableDistributedTransactions();
            #endregion
        }

        void TransportTransactionAtomicSendsWithReceive(BusConfiguration busConfiguration)
        {
            #region TransportTransactionAtomicSendsWithReceive
            busConfiguration.Transactions()
                .DisableDistributedTransactions();
            #endregion
        }

        void TransportTransactionScope(BusConfiguration busConfiguration)
        {
            #region TransportTransactionScope
            busConfiguration.Transactions()
                .Enable()
                .EnableDistributedTransactions();
            #endregion
        }

        void TransportTransactionsWithScope(BusConfiguration busConfiguration)
        {
            #region TransactionsWrapHandlersExecutionInATransactionScope
            busConfiguration.Transactions()
                .DisableDistributedTransactions()
                .WrapHandlersExecutionInATransactionScope();
            #endregion
        }



        void CustomTransactionTimeout(BusConfiguration busConfiguration)
        {
            #region CustomTransactionTimeout
            busConfiguration.Transactions()
                .DefaultTimeout(TimeSpan.FromSeconds(30));
            #endregion
        }

        void CustomTransactionIsolationLevel(BusConfiguration busConfiguration)
        {
            #region CustomTransactionIsolationLevel
            busConfiguration.Transactions()
                .IsolationLevel(IsolationLevel.RepeatableRead);
            #endregion
        }
    }
}
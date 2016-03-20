namespace Snippets5
{
    using System;
    using System.Transactions;
    using NServiceBus;

    public class Transactions
    {
        public void Unreliable(BusConfiguration busConfiguration)
        {
            #region TransactionsDisable
            busConfiguration.Transactions()
                .Disable();
            #endregion
        }

        public void TransportTransactionReceiveOnly(BusConfiguration busConfiguration)
        {
            #region TransportTransactionReceiveOnly
            busConfiguration.Transactions()
                .DisableDistributedTransactions();
            #endregion
        }

        public void TransportTransactionAtomicSendsWithReceive(BusConfiguration busConfiguration)
        {
            #region TransportTransactionAtomicSendsWithReceive
            busConfiguration.Transactions()
                .DisableDistributedTransactions();
            #endregion
        }

        public void TransportTransactionScope(BusConfiguration busConfiguration)
        {
            #region TransportTransactionScope
            busConfiguration.Transactions()
                .Enable()
                .EnableDistributedTransactions();
            #endregion
        }

        public void TransportTransactionsWithScope(BusConfiguration busConfiguration)
        {
            #region TransactionsWrapHandlersExecutionInATransactionScope
            busConfiguration.Transactions()
                .DisableDistributedTransactions()
                .WrapHandlersExecutionInATransactionScope();
            #endregion
        }



        public void CustomTransactionTimeout(BusConfiguration busConfiguration)
        {
            #region CustomTransactionTimeout
            busConfiguration.Transactions()
                .DefaultTimeout(TimeSpan.FromSeconds(30));
            #endregion
        }

        public void CustomTransactionIsolationLevel(BusConfiguration busConfiguration)
        {
            #region CustomTransactionIsolationLevel
            busConfiguration.Transactions()
                .IsolationLevel(IsolationLevel.RepeatableRead);
            #endregion
        }
    }
}
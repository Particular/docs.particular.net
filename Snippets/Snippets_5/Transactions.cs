namespace Snippets5
{
    using System;
    using System.Transactions;
    using NServiceBus;

    public class Transactions
    {
        public void Unreliable()
        {
            #region TransactionsDisable
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .Disable();
            #endregion
        }

        public void TransportTransactions()
        {
            #region TransactionsDisableDistributedTransactions

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .DisableDistributedTransactions();

            #endregion
        }

        public void AmbientTransactions()
        {
            #region TransactionsEnable
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .Enable()
                .EnableDistributedTransactions();
            #endregion
        }

        public void TransportTransactionsWithScope()
        {
            #region TransactionsWrapHandlersExecutionInATransactionScope
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .DisableDistributedTransactions()
                .WrapHandlersExecutionInATransactionScope();
            #endregion
        }

     

        public void CustomTransactionTimeout()
        {
            #region CustomTransactionTimeout
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions().DefaultTimeout(TimeSpan.FromSeconds(30));
            #endregion
        }

        public void CustomTransactionIsolationLevel()
        {
            #region CustomTransactionIsolationLevel
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions().IsolationLevel(IsolationLevel.RepeatableRead);
            #endregion
        }
    }
}
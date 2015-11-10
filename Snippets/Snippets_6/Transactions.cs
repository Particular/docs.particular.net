namespace Snippets6
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
            busConfiguration.Transactions().Disable();
            #endregion
        }

        public void TransportTransactions()
        {
            #region TransactionsDisableDistributedTransactions
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions().DisableDistributedTransactions();
            #endregion

        }

        public void AmbientTransactions()
        {
            #region TransactionsEnable
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions().Enable().EnableDistributedTransactions();
            #endregion

            #region TransactionsDoNotWrapHandlersExecutionInATransactionScope
            busConfiguration.Transactions().DoNotWrapHandlersExecutionInATransactionScope();
            #endregion
        }

        public void TransportTransactionsWithScope()
        {
            #region TransactionsWrapHandlersExecutionInATransactionScope
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions().DisableDistributedTransactions().WrapHandlersExecutionInATransactionScope();
            #endregion
        }

        public void Outbox()
        {

            #region TransactionsOutbox

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EnableOutbox(); //Implies .DisableDistributedTransactions().DoNotWrapHandlersExecutionInATransactionScope();

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
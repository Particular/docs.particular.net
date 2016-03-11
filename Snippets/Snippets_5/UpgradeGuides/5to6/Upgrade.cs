namespace Snippets5.UpgradeGuides._5to6
{
    using System;
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.Unicast.Transport;

    public class Upgrade
    {
        public void CriticalError()
        {
            // ReSharper disable RedundantDelegateCreation

            #region 5to6CriticalError

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.DefineCriticalErrorAction(
                new Action<string, Exception>((error, exception) =>
                {
                    // place you custom handling here 
                }));

            #endregion

            // ReSharper restore RedundantDelegateCreation
        }

        public void TransportTransactions()
        {
            #region 5to6DoNotWrapHandlersInTransaction

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .DoNotWrapHandlersExecutionInATransactionScope();

            #endregion
        }

        public void SuppressDistributedTransactions()
        {
            TransactionSettings transactionSettings = null;

            #region 5to6SuppressDistributedTransactions

            bool suppressDistributedTransactions = transactionSettings.SuppressDistributedTransactions;

            #endregion
        }

        public void IsTransactional()
        {
            TransactionSettings transactionSettings = null;

            #region 5to6IsTransactional

            bool isTransactional = transactionSettings.IsTransactional;

            #endregion
        }

        public void TransportTransactionIsolationLevelAndTimeout()
        {
            #region 5to6TransportTransactionScopeOptions
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .IsolationLevel(IsolationLevel.RepeatableRead)
                .DefaultTimeout(TimeSpan.FromSeconds(30));
            #endregion
        }

        public void WrapHandlersExecutionInATransactionScope()
        {
            #region 5to6WrapHandlersExecutionInATransactionScope
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .WrapHandlersExecutionInATransactionScope();
            #endregion
        }

        public void DelayedDelivery()
        {
            IBus bus = null;
            object message = null;

            #region 5to6delayed-delivery
            bus.Defer(TimeSpan.FromMinutes(30), message);
            // OR
            bus.Defer(new DateTime(2016, 12, 25), message);
            #endregion
        }

        public void EnableTransactions()
        {
            #region 5to6EnableTransactions
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .Enable();
            #endregion
        }

        public void DisableTransactions()
        {
            #region 5to6DisableTransactions
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .Disable();
            #endregion
        }

        public void EnableDistributedTransactions()
        {
            #region 5to6EnableDistributedTransactions
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .EnableDistributedTransactions();
            #endregion
        }

        public void DisableDistributedTransactions()
        {
            #region 5to6DisableDistributedTransactions
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.Transactions()
                .DisableDistributedTransactions();
            #endregion
        }
    }
}
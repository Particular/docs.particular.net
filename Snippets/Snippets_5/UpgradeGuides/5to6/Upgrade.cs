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

            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6CriticalError

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
            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6DoNotWrapHandlersInTransaction

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
            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6TransportTransactionScopeOptions
            busConfiguration.Transactions()
                .IsolationLevel(IsolationLevel.RepeatableRead)
                .DefaultTimeout(TimeSpan.FromSeconds(30));
            #endregion
        }

        public void WrapHandlersExecutionInATransactionScope()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6WrapHandlersExecutionInATransactionScope
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
            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6EnableTransactions
            busConfiguration.Transactions()
                .Enable();
            #endregion
        }

        public void DisableTransactions()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6DisableTransactions
            busConfiguration.Transactions()
                .Disable();
            #endregion
        }

        public void EnableDistributedTransactions()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6EnableDistributedTransactions
            busConfiguration.Transactions()
                .EnableDistributedTransactions();
            #endregion
        }

        public void DisableDistributedTransactions()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region 5to6DisableDistributedTransactions
            busConfiguration.Transactions()
                .DisableDistributedTransactions();
            #endregion
        }
    }
}
namespace Core5.UpgradeGuides._5to6
{
    using System;
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.Unicast.Transport;

    class Upgrade
    {
        void CriticalError(BusConfiguration busConfiguration)
        {
            // ReSharper disable RedundantDelegateCreation

            #region 5to6CriticalError

            busConfiguration.DefineCriticalErrorAction(
                new Action<string, Exception>((error, exception) =>
                {
                    // place you custom handling here
                }));

            #endregion

            // ReSharper restore RedundantDelegateCreation
        }

        void TransportTransactions(BusConfiguration busConfiguration)
        {
            #region 5to6DoNotWrapHandlersInTransaction

            var transactions = busConfiguration.Transactions();
            transactions.DoNotWrapHandlersExecutionInATransactionScope();

            #endregion
        }

        void SuppressDistributedTransactions(TransactionSettings transactionSettings)
        {
            #region 5to6SuppressDistributedTransactions

            bool suppressDistributedTransactions = transactionSettings.SuppressDistributedTransactions;

            #endregion
        }

        void IsTransactional(TransactionSettings transactionSettings)
        {
            #region 5to6IsTransactional

            bool isTransactional = transactionSettings.IsTransactional;

            #endregion
        }

        void TransportTransactionIsolationLevelAndTimeout(BusConfiguration busConfiguration)
        {
            #region 5to6TransportTransactionScopeOptions

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.IsolationLevel(IsolationLevel.RepeatableRead);
            transactionSettings.DefaultTimeout(TimeSpan.FromSeconds(30));
            #endregion
        }

        void WrapHandlersExecutionInATransactionScope(BusConfiguration busConfiguration)
        {
            #region 5to6WrapHandlersExecutionInATransactionScope

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.WrapHandlersExecutionInATransactionScope();
            #endregion
        }

        void DelayedDelivery(IBus bus,object message)
        {
            #region 5to6delayed-delivery
            bus.Defer(TimeSpan.FromMinutes(30), message);
            // OR
            bus.Defer(new DateTime(2016, 12, 25), message);
            #endregion
        }

        void EnableTransactions(BusConfiguration busConfiguration)
        {
            #region 5to6EnableTransactions

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.Enable();
            #endregion
        }

        void DisableTransactions(BusConfiguration busConfiguration)
        {
            #region 5to6DisableTransactions

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.Disable();
            #endregion
        }

        void EnableDistributedTransactions(BusConfiguration busConfiguration)
        {
            #region 5to6EnableDistributedTransactions

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.EnableDistributedTransactions();
            #endregion
        }

        void DisableDistributedTransactions(BusConfiguration busConfiguration)
        {
            #region 5to6DisableDistributedTransactions

            var transactionSettings = busConfiguration.Transactions();
            transactionSettings.DisableDistributedTransactions();
            #endregion
        }
    }
}
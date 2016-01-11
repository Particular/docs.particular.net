namespace Snippets5.UpgradeGuides._5to6
{
    using System;
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
    }
}
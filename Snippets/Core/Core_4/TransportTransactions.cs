namespace Core4
{
    using System;
    using System.Transactions;
    using NServiceBus;

    class TransportTransactions
    {
        void Unreliable()
        {
            #region TransactionsDisable

            var transactions = Configure.Transactions;
            transactions.Disable();

            #endregion
        }

        void TransportTransactionReceiveOnly()
        {
            #region TransportTransactionReceiveOnly

            Configure.Transactions.Advanced(
                action: advancedSettings =>
                {
                    advancedSettings.DisableDistributedTransactions();
                });

            #endregion
        }

        void TransportTransactionAtomicSendsWithReceive()
        {
            #region TransportTransactionAtomicSendsWithReceive

            var transactions = Configure.Transactions;
            transactions.Advanced(
                action: advancedSettings =>
                {
                    advancedSettings.DisableDistributedTransactions();
                });

            #endregion
        }

        void TransportTransactionScope()
        {
            #region TransportTransactionScope

            var transactions = Configure.Transactions;
            transactions.Enable();
            transactions.Advanced(
                action: advancedSettings =>
                {
                    advancedSettings.EnableDistributedTransactions();
                });

            #endregion
        }

        void TransportTransactionsWithScope()
        {
            #region TransactionsWrapHandlersExecutionInATransactionScope

            var transactions = Configure.Transactions;
            transactions.Enable();
            transactions.Advanced(
                action: advancedSettings =>
                {
                    advancedSettings.DisableDistributedTransactions();
                    advancedSettings.WrapHandlersExecutionInATransactionScope();
                });

            #endregion
        }

        void CustomTransactionTimeout()
        {
            #region CustomTransactionTimeout

            var transactions = Configure.Transactions;
            transactions.Advanced(
                action: advancedSettings =>
                {
                    advancedSettings.DefaultTimeout(TimeSpan.FromSeconds(30));
                });

            #endregion
        }

        void CustomTransactionIsolationLevel()
        {
            #region CustomTransactionIsolationLevel

            var transactions = Configure.Transactions;
            transactions.Advanced(
                action: advancedSettings =>
                {
                    advancedSettings.IsolationLevel(IsolationLevel.RepeatableRead);
                });

            #endregion
        }
    }
}
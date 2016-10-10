namespace Core3
{
    using System;
    using System.Transactions;
    using NServiceBus;

    class TransportTransactions
    {
        void Unreliable(Configure configure)
        {
            #region TransactionsDisable

            configure.DontUseTransactions();

            #endregion
        }

        void CustomTransactionTimeout(Configure configure)
        {
            #region CustomTransactionTimeout

            configure.TransactionTimeout(TimeSpan.FromSeconds(30));

            #endregion
        }

        void CustomTransactionIsolationLevel(Configure configure)
        {
            #region CustomTransactionIsolationLevel

            configure.IsolationLevel(IsolationLevel.RepeatableRead);

            #endregion
        }
    }
}
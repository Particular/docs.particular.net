namespace Core4
{
    using System;
    using System.Transactions;
    using NServiceBus;

    class Transactions
    {
        void Unreliable()
        {
            #region TransactionsDisable
            Configure.Transactions.Disable();
            #endregion
        }

        void TransportTransactionReceiveOnly()
        {
            #region TransportTransactionReceiveOnly
            Configure.Transactions.Advanced(x => x.DisableDistributedTransactions());
            #endregion
        }

        void TransportTransactionAtomicSendsWithReceive()
        {
            #region TransportTransactionAtomicSendsWithReceive
            Configure.Transactions.Advanced(x => x.DisableDistributedTransactions());
            #endregion
        }

        void TransportTransactionScope()
        {
            #region TransportTransactionScope
            Configure.Transactions.Enable().Advanced(x => x.EnableDistributedTransactions());
            #endregion
        }

        void CustomTransactionTimeout()
        {
            #region CustomTransactionTimeout
            Configure.Transactions.Advanced(x => x.DefaultTimeout(TimeSpan.FromSeconds(30)));
            #endregion
        }

        void CustomTransactionIsolationLevel()
        {
            #region CustomTransactionIsolationLevel
            Configure.Transactions.Advanced(x => x.IsolationLevel(IsolationLevel.RepeatableRead));
            #endregion
        }
    }
}
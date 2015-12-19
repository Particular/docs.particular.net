namespace Snippets4
{
    using System;
    using System.Transactions;
    using NServiceBus;

    public class Transactions
    {
        public void Unreliable()
        {
            #region TransactionsDisable
            Configure.Transactions.Disable();
            #endregion
        }

        public void TransportTransactionReceiveOnly()
        {
            #region TransportTransactionReceiveOnly
            Configure.Transactions.Advanced(x => x.DisableDistributedTransactions());
            #endregion
        }

        public void TransportTransactionAtomicSendsWithReceive()
        {
            #region TransportTransactionAtomicSendsWithReceive
            Configure.Transactions.Advanced(x => x.DisableDistributedTransactions());
            #endregion
        }

        public void AmbientTransactions()
        {
            #region TransactionsEnable
            Configure.Transactions.Enable().Advanced(x => x.EnableDistributedTransactions());
            #endregion
        }

        public void CustomTransactionTimeout()
        {
            #region CustomTransactionTimeout
            Configure.Transactions.Advanced(x => x.DefaultTimeout(TimeSpan.FromSeconds(30)));
            #endregion
        }

        public void CustomTransactionIsolationLevel()
        {
            #region CustomTransactionIsolationLevel
            Configure.Transactions.Advanced(x => x.IsolationLevel(IsolationLevel.RepeatableRead));
            #endregion
        }
    }
}
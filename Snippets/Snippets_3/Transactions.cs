namespace Snippets3
{
    using System;
    using System.Transactions;
    using NServiceBus;

    public class Transactions
    {
        public void Unreliable()
        {
            #region TransactionsDisable
            Configure configure = Configure.With();
            configure.DontUseTransactions();
            #endregion
        }

        public void CustomTransactionTimeout()
        {
            #region CustomTransactionTimeout
            Configure configure = Configure.With();
            configure.TransactionTimeout(TimeSpan.FromSeconds(30));
            #endregion
        }

        public void CustomTransactionIsolationLevel()
        {
            #region CustomTransactionIsolationLevel
            Configure configure = Configure.With();
            configure.IsolationLevel(IsolationLevel.RepeatableRead);
            #endregion
        }
    }
}
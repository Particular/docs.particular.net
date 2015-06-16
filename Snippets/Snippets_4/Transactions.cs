namespace Snippets4
{
    using NServiceBus;

    public class Transactions
    {
        public void Unreliable()
        {
            #region TransactionsDisable
            Configure.Transactions.Disable();
            #endregion
        }

        public void TransportTransactions()
        {
            #region TransactionsDisableDistributedTransactions
            Configure.Transactions.Advanced(x => x.DisableDistributedTransactions());
            #endregion

        }

        public void AmbientTransactions()
        {
            #region TransactionsEnable
            Configure.Transactions.Enable().Advanced(x => x.EnableDistributedTransactions());
            #endregion

            #region TransactionsDoNotWrapHandlersExecutionInATransactionScope
            Configure.Transactions.Advanced(x => x.DoNotWrapHandlersExecutionInATransactionScope());
            #endregion
        }
    }
}
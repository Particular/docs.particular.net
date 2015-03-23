using NServiceBus;

public class Transactions
{
    public void Unreliable()
    {
        #region TransactionsDisable
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.Transactions().Disable();
        #endregion
    }

    public void TransportTransactions()
    {
        #region TransactionsDisableDistributedTransactions
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.Transactions().DisableDistributedTransactions();
        #endregion

    }

    public void AmbientTransactions()
    {
        #region TransactionsEnable
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.Transactions().Enable().EnableDistributedTransactions();
        #endregion

        #region TransactionsDoNotWrapHandlersExecutionInATransactionScope
        busConfig.Transactions().DoNotWrapHandlersExecutionInATransactionScope();
        #endregion
    }

    public void TransportTransactionsWithScope()
    {
        #region TransactionsWrapHandlersExecutionInATransactionScope
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.Transactions().DisableDistributedTransactions().WrapHandlersExecutionInATransactionScope();
        #endregion
    }

    public void Outbox()
    {

        #region TransactionsOutbox

        BusConfiguration busConfig = new BusConfiguration();
        busConfig.EnableOutbox(); //Implies .DisableDistributedTransactions().DoNotWrapHandlersExecutionInATransactionScope();

        #endregion
    }
}
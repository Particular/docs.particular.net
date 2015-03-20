using NServiceBus;

public class Transactions
{
    public void Unreliable()
    {
        var busConfig = new BusConfiguration();

        #region TransactionsDisable
        busConfig.Transactions().Disable();
        #endregion
    }

    public void TransportTransactions()
    {
        var busConfig = new BusConfiguration();
        #region TransactionsDisableDistributedTransactions
        busConfig.Transactions().DisableDistributedTransactions();
        #endregion

    }

    public void AmbientTransactions()
    {
        var busConfig = new BusConfiguration();
        #region TransactionsEnable
        busConfig.Transactions().Enable().EnableDistributedTransactions();
        #endregion

        #region TransactionsDoNotWrapHandlersExecutionInATransactionScope
        busConfig.Transactions().DoNotWrapHandlersExecutionInATransactionScope();
        #endregion
    }

    public void TransportTransactionsWithScope()
    {
        var busConfig = new BusConfiguration();

        #region TransactionsWrapHandlersExecutionInATransactionScope
        busConfig.Transactions().DisableDistributedTransactions().WrapHandlersExecutionInATransactionScope();
        #endregion
    }

    public void Outbox()
    {
        var busConfig = new BusConfiguration();

        #region TransactionsOutbox

        busConfig.EnableOutbox(); //Implies .DisableDistributedTransactions().DoNotWrapHandlersExecutionInATransactionScope();

        #endregion
    }
}
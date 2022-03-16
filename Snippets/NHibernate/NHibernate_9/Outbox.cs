using System.Data;
using NServiceBus;

public class Outbox
{
    public void TransactionScopeMode(EndpointConfiguration endpointConfiguration)
    {
        #region OutboxTransactionIsolation

        var outboxSettings = endpointConfiguration.EnableOutbox();
        outboxSettings.UseTransactionScope();
        outboxSettings.TransactionIsolationLevel(IsolationLevel.ReadCommitted);

        #endregion
    }
}
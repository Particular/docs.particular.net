namespace Snippets6
{
    using System;
    using System.Transactions;
    using NServiceBus;

    public class MsmqTransactionScope
    {
        public void MsmqTransactionScopeIsolationLevel()
        {
            #region MsmqTransactionScopeIsolationLevel
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<MsmqTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        public void MsmqTransactionScopeTimeout()
        {
            #region MsmqTransactionScopeTimeout
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UseTransport<MsmqTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
    }
}
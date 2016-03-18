namespace Snippets6
{
    using System;
    using System.Transactions;
    using NServiceBus;

    public class MsmqTransactionScope
    {
        public void MsmqTransactionScopeIsolationLevel(EndpointConfiguration endpointConfiguration)
        {
            #region MsmqTransactionScopeIsolationLevel
            endpointConfiguration.UseTransport<MsmqTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        public void MsmqTransactionScopeTimeout(EndpointConfiguration endpointConfiguration)
        {
            #region MsmqTransactionScopeTimeout
            endpointConfiguration.UseTransport<MsmqTransport>()
                .Transactions(TransportTransactionMode.TransactionScope)
                .TransactionScopeOptions(timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
    }
}
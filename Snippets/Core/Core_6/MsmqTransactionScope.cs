namespace Core6
{
    using System;
    using System.Transactions;
    using NServiceBus;

    class MsmqTransactionScope
    {
        void MsmqTransactionScopeIsolationLevel(EndpointConfiguration endpointConfiguration)
        {
            #region MsmqTransactionScopeIsolationLevel

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.Transactions(TransportTransactionMode.TransactionScope);
            transport.TransactionScopeOptions(
                isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        void MsmqTransactionScopeTimeout(EndpointConfiguration endpointConfiguration)
        {
            #region MsmqTransactionScopeTimeout

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.Transactions(TransportTransactionMode.TransactionScope);
            transport.TransactionScopeOptions(
                timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
    }
}
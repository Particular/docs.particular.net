namespace Snippets6
{
    using System;
    using System.Transactions;
    using NServiceBus;

    public class TransactionScopeUnitOfWork
    {
        public void UnitOfWorkWrapHandlersInATransactionScope(EndpointConfiguration endpointConfiguration)
        {
            #region UnitOfWorkWrapHandlersInATransactionScope
            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope();
            #endregion
        }

        public void UnitOfWorkCustomTransactionIsolationLevel(EndpointConfiguration endpointConfiguration)
        {
            #region UnitOfWorkCustomTransactionIsolationLevel
            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope(isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        public void UnitOfWorkCustomTransactionTimeout(EndpointConfiguration endpointConfiguration)
        {
            #region UnitOfWorkCustomTransactionTimeout
            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope(timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
    }
}
namespace Snippets6
{
    using System;
    using System.Transactions;
    using NServiceBus;

    public class TransactionScopeUnitOfWork
    {
        public void UnitOfWorkWrapHandlersInATransactionScope()
        {
            #region UnitOfWorkWrapHandlersInATransactionScope
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope();
            #endregion
        }

        public void UnitOfWorkCustomTransactionIsolationLevel()
        {
            #region UnitOfWorkCustomTransactionIsolationLevel
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope(isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        public void UnitOfWorkCustomTransactionTimeout()
        {
            #region UnitOfWorkCustomTransactionTimeout
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope(timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
    }
}
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
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UnitOfWork()
                .WrapHandlersInATransactionScope();
            #endregion
        }

        public void UnitOfWorkCustomTransactionIsolationLevel()
        {
            #region UnitOfWorkCustomTransactionIsolationLevel
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UnitOfWork()
                .WrapHandlersInATransactionScope(isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        public void UnitOfWorkCustomTransactionTimeout()
        {
            #region UnitOfWorkCustomTransactionTimeout
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.UnitOfWork()
                .WrapHandlersInATransactionScope(timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
    }
}
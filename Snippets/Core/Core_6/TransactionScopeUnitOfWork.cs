namespace Core6
{
    using System;
    using System.Transactions;
    using NServiceBus;

    class TransactionScopeUnitOfWork
    {
        void UnitOfWorkWrapHandlersInATransactionScope(EndpointConfiguration endpointConfiguration)
        {
            #region UnitOfWorkWrapHandlersInATransactionScope
            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope();
            #endregion
        }

        void UnitOfWorkCustomTransactionIsolationLevel(EndpointConfiguration endpointConfiguration)
        {
            #region UnitOfWorkCustomTransactionIsolationLevel
            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope(isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        void UnitOfWorkCustomTransactionTimeout(EndpointConfiguration endpointConfiguration)
        {
            #region UnitOfWorkCustomTransactionTimeout
            endpointConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope(timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
    }
}
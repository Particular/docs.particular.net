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
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope();
            #endregion
        }

        public void UnitOfWorkCustomTransactionIsolationLevel()
        {
            #region UnitOfWorkCustomTransactionIsolationLevel
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope(isolationLevel: IsolationLevel.RepeatableRead);
            #endregion
        }

        public void UnitOfWorkCustomTransactionTimeout()
        {
            #region UnitOfWorkCustomTransactionTimeout
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UnitOfWork()
                .WrapHandlersInATransactionScope(timeout: TimeSpan.FromSeconds(30));
            #endregion
        }
    }
}
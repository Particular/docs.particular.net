namespace Core8
{
    using System;
    using System.Transactions;
    using NServiceBus;

    class TransactionScopeUnitOfWork
    {
        void UnitOfWorkWrapHandlersInATransactionScope(EndpointConfiguration endpointConfiguration)
        {
            #region UnitOfWorkWrapHandlersInATransactionScope

            var unitOfWork = endpointConfiguration.UnitOfWork();
            unitOfWork.WrapHandlersInATransactionScope();

            #endregion
        }

        void UnitOfWorkCustomTransactionIsolationLevel(EndpointConfiguration endpointConfiguration)
        {
            #region UnitOfWorkCustomTransactionIsolationLevel

            var unitOfWork = endpointConfiguration.UnitOfWork();
            unitOfWork.WrapHandlersInATransactionScope(
                isolationLevel: IsolationLevel.RepeatableRead);

            #endregion
        }

        void UnitOfWorkCustomTransactionTimeout(EndpointConfiguration endpointConfiguration)
        {
            #region UnitOfWorkCustomTransactionTimeout

            var unitOfWork = endpointConfiguration.UnitOfWork();
            unitOfWork.WrapHandlersInATransactionScope(
                timeout: TimeSpan.FromSeconds(30));

            #endregion
        }
    }
}
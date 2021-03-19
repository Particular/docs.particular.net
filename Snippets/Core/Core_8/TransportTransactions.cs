namespace Core8
{
    using NServiceBus;

    class TransportTransactions
    {
        void Unreliable(EndpointConfiguration endpointConfiguration)
        {
            #region TransactionsDisable

            var transport = endpointConfiguration.UseTransport(new TransportDefinition
            {
                TransportTransactionMode = TransportTransactionMode.None
            });

            #endregion
        }

        void TransportTransactionReceiveOnly(EndpointConfiguration endpointConfiguration)
        {
            #region TransportTransactionReceiveOnly

            var transport = endpointConfiguration.UseTransport(new TransportDefinition
            {
                TransportTransactionMode = TransportTransactionMode.ReceiveOnly
            });

            #endregion
        }

        void TransportTransactionAtomicSendsWithReceive(EndpointConfiguration endpointConfiguration)
        {
            #region TransportTransactionAtomicSendsWithReceive

            var transport = endpointConfiguration.UseTransport(new TransportDefinition
            {
                TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
            });

            #endregion
        }

        void TransportTransactionScope(EndpointConfiguration endpointConfiguration)
        {
            #region TransportTransactionScope

            var transport = endpointConfiguration.UseTransport(new TransportDefinition
            {
                TransportTransactionMode = TransportTransactionMode.TransactionScope
            });

            #endregion
        }

        void TransportTransactionsWithScope(EndpointConfiguration endpointConfiguration)
        {
            #region TransactionsWrapHandlersExecutionInATransactionScope

            var unitOfWorkSettings = endpointConfiguration.UnitOfWork();
            unitOfWorkSettings.WrapHandlersInATransactionScope();

            #endregion
        }
    }
}
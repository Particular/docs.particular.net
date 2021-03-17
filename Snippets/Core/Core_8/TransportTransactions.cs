namespace Core8
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Transactions;
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Transport;

    class TransportTransactions
    {
        void Unreliable(EndpointConfiguration endpointConfiguration)
        {
            #region TransactionsDisable

            var transport = endpointConfiguration.UseTransport(new MyTransport()
            {
                TransportTransactionMode = TransportTransactionMode.None
            });

            #endregion
        }

        void TransportTransactionReceiveOnly(EndpointConfiguration endpointConfiguration)
        {
            #region TransportTransactionReceiveOnly

            var transport = endpointConfiguration.UseTransport(new MyTransport
            {
                TransportTransactionMode = TransportTransactionMode.ReceiveOnly
            });

            #endregion
        }

        void TransportTransactionAtomicSendsWithReceive(EndpointConfiguration endpointConfiguration)
        {
            #region TransportTransactionAtomicSendsWithReceive

            var transport = endpointConfiguration.UseTransport(new MyTransport
            {
                TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
            });

            #endregion
        }

        void TransportTransactionScope(EndpointConfiguration endpointConfiguration)
        {
            #region TransportTransactionScope

            var transport = endpointConfiguration.UseTransport(new MyTransport
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

    public class MyTransport :
        TransportDefinition, IMessageDrivenSubscriptionTransport
    {
        public MyTransport()
            : base(TransportTransactionMode.None, true, true, true)
        {
        }

        public override Task<TransportInfrastructure> Initialize(HostSettings hostSettings, ReceiveSettings[] receivers, string[] sendingAddresses, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public override string ToTransportAddress(QueueAddress address)
        {
            throw new NotImplementedException();
        }

        public override IReadOnlyCollection<TransportTransactionMode> GetSupportedTransactionModes()
        {
            throw new NotImplementedException();
        }
    }
}
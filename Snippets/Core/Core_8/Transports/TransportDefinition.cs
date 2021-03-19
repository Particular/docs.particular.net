namespace Core8
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Transport;

    class TransportDefinition : NServiceBus.Transport.TransportDefinition, IMessageDrivenSubscriptionTransport
    {
        public TransportDefinition() : base(TransportTransactionMode.None, true, true, true)
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
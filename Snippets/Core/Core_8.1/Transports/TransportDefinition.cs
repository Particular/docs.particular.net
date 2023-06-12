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

        [Obsolete("Inject the ITransportAddressResolver type to access the address translation mechanism at runtime. See the NServiceBus version 8 upgrade guide for further details. Will be treated as an error from version 9.0.0. Will be removed in version 10.0.0.", false)]
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
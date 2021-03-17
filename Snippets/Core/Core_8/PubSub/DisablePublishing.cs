namespace Core8.PublishSubscribe
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Transport;

    class DisablePublishing
    {
        void DisablePublishingConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region DisablePublishing
            var transportConfiguration = endpointConfiguration.UseTransport(new TransportDefinition());
            transportConfiguration.DisablePublishing();
            #endregion
        }
    }

    class TransportDefinition : NServiceBus.Transport.TransportDefinition, IMessageDrivenSubscriptionTransport
    {
        public TransportDefinition()
            :base (TransportTransactionMode.None, true, true, true)
        {
        }

        public override Task<TransportInfrastructure> Initialize(HostSettings hostSettings, ReceiveSettings[] receivers, string[] sendingAddresses, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new System.NotImplementedException();
        }

        public override string ToTransportAddress(QueueAddress address)
        {
            throw new System.NotImplementedException();
        }

        public override IReadOnlyCollection<TransportTransactionMode> GetSupportedTransactionModes()
        {
            throw new System.NotImplementedException();
        }
    }
}
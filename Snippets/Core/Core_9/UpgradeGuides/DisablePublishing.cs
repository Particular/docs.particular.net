namespace Core9.PublishSubscribe
{
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Transport;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    class DisablePublishingUpgradeGuide
    {
        void DisablePublishingConfiguration(EndpointConfiguration endpointConfiguration)
        {
            #region DisablePublishing-UpgradeGuide
            var transportConfiguration = endpointConfiguration.UseTransport(new MyTransport());
            transportConfiguration.DisablePublishing();
            #endregion
        }
    }

    internal class MyTransport : TransportDefinition, IMessageDrivenSubscriptionTransport
    {
        public MyTransport() : base(TransportTransactionMode.None, false, false, false)
        {
        }

        public override IReadOnlyCollection<TransportTransactionMode> GetSupportedTransactionModes()
        {
            throw new System.NotImplementedException();
        }

        public override Task<TransportInfrastructure> Initialize(HostSettings hostSettings, ReceiveSettings[] receivers, string[] sendingAddresses, CancellationToken cancellationToken = default)
        {
            throw new System.NotImplementedException();
        }
    }
}
namespace Core8.EndpointName
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Transport;

    class Usage
    {
        void EndpointNameCode()
        {
            #region EndpointNameCode

            var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

            #endregion
        }
        void InputQueueName(EndpointConfiguration endpointConfiguration)
        {
            #region InputQueueName

            endpointConfiguration.OverrideLocalAddress("MyEndpoint.Messages");

            #endregion
        }

        class MyTransport :
            TransportDefinition
        {
            public MyTransport(TransportTransactionMode defaultTransactionMode, bool supportsDelayedDelivery, bool supportsPublishSubscribe, bool supportsTTBR) : base(defaultTransactionMode, supportsDelayedDelivery, supportsPublishSubscribe, supportsTTBR)
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

        class MyMessage
        {
        }
    }
}

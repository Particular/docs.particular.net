namespace Core8
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Transport;

    class BestPracticesConfiguration
    {
        void DisableFeature(EndpointConfiguration endpointConfiguration)
        {
            #region DisableBestPracticeEnforcementPerEndpoint

            var routing = endpointConfiguration.UseTransport(new TransportName());
            routing.DoNotEnforceBestPractices();

            #endregion
        }

        async Task DisablePerMessage(IPipelineContext context)
        {
            #region DisableBestPracticeEnforcementPerMessage

            var options = new SendOptions();

            options.DoNotEnforceBestPractices();

            await context.Send(new MyEvent(), options)
                .ConfigureAwait(false);

            #endregion
        }

        class MyEvent
        {
        }

        class TransportName :
            TransportDefinition
        {
            public TransportName()
                : base(TransportTransactionMode.None, true, true, true)
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
}
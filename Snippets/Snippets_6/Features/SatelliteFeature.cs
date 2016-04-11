namespace Snippets6.Features
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;
    using NServiceBus.Transports;

    #region SimpleSatelliteFeature
    public class MySatelliteFeature : Feature
    {
        public MySatelliteFeature()
        {
            EnableByDefault();
        }
        protected override void Setup(FeatureConfigurationContext context)
        {
            PipelineSettings satelliteMessagePipeline = context.AddSatellitePipeline("CustomSatellite", TransportTransactionMode.TransactionScope, PushRuntimeSettings.Default, "targetQueue");
            satelliteMessagePipeline.Register("Satellite Identifier", new MySatelliteBehavior(), "Description of what the satellite does");
        }
    }
    #endregion

    #region SatelliteBehavior
    class MySatelliteBehavior : PipelineTerminator<ISatelliteProcessingContext>
    {
        protected override Task Terminate(ISatelliteProcessingContext context)
        {
            // Implement what this satellite needs to do once it receives a message
            IncomingMessage message = context.Message;
            return Task.FromResult(true);
        }
    }
    #endregion
}

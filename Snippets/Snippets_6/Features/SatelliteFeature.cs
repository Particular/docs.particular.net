namespace Snippets6.Features
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Pipeline;
    using NServiceBus.Transports;

    public class MySatelliteFeature : Feature
    {

        public MySatelliteFeature()
        {
            EnableByDefault();
        }

        #region SatelliteFeatureSetup
        protected override void Setup(FeatureConfigurationContext context)
        {
            PipelineSettings messageProcessorPipeline = context.AddSatellitePipeline("CustomSatellite", TransportTransactionMode.TransactionScope, PushRuntimeSettings.Default, "targetQueue");
            messageProcessorPipeline.Register("CustomSatellite", new MySatelliteBehavior(), "Description of what the satellite does");
        }
        #endregion
    }

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

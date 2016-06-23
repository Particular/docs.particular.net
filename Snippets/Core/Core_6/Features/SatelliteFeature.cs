namespace Core6.Features
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.ObjectBuilder;
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
            context.AddSatelliteReceiver(
                name: "MyCustomSatellite",
                transportAddress: "targetQueue",
                requiredTransportTransactionMode: TransportTransactionMode.TransactionScope,
                runtimeSettings: PushRuntimeSettings.Default,
                onMessage: OnMessage);
        }

        Task OnMessage(IBuilder builder, PushContext messageContext)
        {
            // Implement what this satellite needs to do once it receives a message
            var messageId = messageContext.MessageId;
            return Task.FromResult(true);
        }
    }
    #endregion

    #region SatelliteBehavior
    class MySatelliteBehavior : PipelineTerminator<ISatelliteProcessingContext>
    {
        protected override Task Terminate(ISatelliteProcessingContext context)
        {
            // Implement what this satellite needs to do once it receives a message
            var incomingMessage = context.Message;
            return Task.FromResult(true);
        }
    }
    #endregion
}

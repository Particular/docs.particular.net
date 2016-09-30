namespace Core6.Features
{
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.ObjectBuilder;
    using NServiceBus.Transport;

    #region SimpleSatelliteFeature
    public class MySatelliteFeature :
        Feature
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
                recoverabilityPolicy: (config, errorContext) =>
                {
                    return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
                },
                onMessage: OnMessage);
        }

        Task OnMessage(IBuilder builder, MessageContext context)
        {
            // Implement what this satellite needs to do once it receives a message
            var messageId = context.MessageId;
            return Task.CompletedTask;
        }
    }
    #endregion

}

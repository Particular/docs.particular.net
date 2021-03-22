namespace Core8.Features
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
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
                runtimeSettings: PushRuntimeSettings.Default,
                recoverabilityPolicy: (config, errorContext) =>
                {
                    return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
                },
                onMessage: OnMessage);
        }

        Task OnMessage(IServiceProvider serviceProvider, MessageContext context, CancellationToken cancellationToken)
        {
            // Implement what this satellite needs to do once it receives a message
            var messageId = context.NativeMessageId;
            return Task.CompletedTask;
        }
    }
    #endregion

}

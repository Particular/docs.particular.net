namespace Core6.Features
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.ObjectBuilder;
    using NServiceBus.Pipeline;
    using NServiceBus.Transport;

    public class MyAdvancedSatelliteFeature :
        Feature
    {
        public MyAdvancedSatelliteFeature()
        {
            EnableByDefault();
        }

        #region AdvancedSatelliteFeatureSetup

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.AddSatelliteReceiver(
                name: "CustomSatellite",
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
            // To raise a critical error
            var exception = new Exception("CriticalError occurred");

            builder.Build<CriticalError>()
                .Raise("Something bad happened - trigger critical error", exception);

            return Task.FromResult(true);
        }

        #endregion
    }

    #region AdvancedSatelliteBehavior
    class MyAdvancedSatelliteBehavior :
        PipelineTerminator<ISatelliteProcessingContext>
    {
        CriticalError criticalError;

        public MyAdvancedSatelliteBehavior(CriticalError criticalError)
        {
            this.criticalError = criticalError;
        }

        protected override Task Terminate(ISatelliteProcessingContext context)
        {
            // To raise a critical error
            var exception = new Exception("CriticalError occurred");
            criticalError.Raise("Something bad happened - trigger critical error", exception);
            return Task.FromResult(true);
        }
    }
    #endregion
}

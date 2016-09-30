namespace Core6.Features
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.ObjectBuilder;
    using NServiceBus.Transport;

    public class SatelliteRecoverability
    {

        public void MoveToError(FeatureConfigurationContext context)
        {
            #region SatelliteRecoverability-ErrorQueue

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

            #endregion
        }

        public void DelayedRetry(FeatureConfigurationContext context)
        {
            #region SatelliteRecoverability-DelayedRetry

            context.AddSatelliteReceiver(
                name: "CustomSatellite",
                transportAddress: "targetQueue",
                requiredTransportTransactionMode: TransportTransactionMode.TransactionScope,
                runtimeSettings: PushRuntimeSettings.Default,
                recoverabilityPolicy: (config, errorContext) =>
                {
                    return RecoverabilityAction.DelayedRetry(
                        timeSpan: TimeSpan.FromMinutes(2));
                },
                onMessage: OnMessage);

            #endregion
        }

        public void ImmediateRetry(FeatureConfigurationContext context)
        {
            #region SatelliteRecoverability-ImmediateRetry

            context.AddSatelliteReceiver(
                name: "CustomSatellite",
                transportAddress: "targetQueue",
                requiredTransportTransactionMode: TransportTransactionMode.TransactionScope,
                runtimeSettings: PushRuntimeSettings.Default,
                recoverabilityPolicy: (config, errorContext) =>
                {
                    return RecoverabilityAction.ImmediateRetry();
                },
                onMessage: OnMessage);

            #endregion
        }

        Task OnMessage(IBuilder builder, MessageContext context)
        {
            // To raise a critical error
            var exception = new Exception("CriticalError occurred");

            var criticalError = builder.Build<CriticalError>();
            criticalError.Raise("Something bad happened - trigger critical error", exception);

            return Task.CompletedTask;
        }

    }

}
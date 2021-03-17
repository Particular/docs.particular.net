namespace Core8.Features
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Transport;

    public class SatelliteRecoverability
    {

        public void MoveToError(FeatureConfigurationContext context)
        {
            #region SatelliteRecoverability-ErrorQueue

            context.AddSatelliteReceiver(
                name: "CustomSatellite",
                transportAddress: "targetQueue",
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
                runtimeSettings: PushRuntimeSettings.Default,
                recoverabilityPolicy: (config, errorContext) =>
                {
                    return RecoverabilityAction.ImmediateRetry();
                },
                onMessage: OnMessage);

            #endregion
        }

        Task OnMessage(IServiceProvider serviceProvider, MessageContext context, CancellationToken cancellationToken)
        {
            // To raise a critical error
            var exception = new Exception("CriticalError occurred");

            var criticalError = serviceProvider.GetRequiredService<CriticalError>();
            criticalError.Raise("Something bad happened - trigger critical error", exception, cancellationToken);

            return Task.CompletedTask;
        }

    }

}
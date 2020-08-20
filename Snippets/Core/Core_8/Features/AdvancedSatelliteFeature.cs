﻿namespace Core8.Features
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using NServiceBus;
    using NServiceBus.Features;
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
                runtimeSettings: PushRuntimeSettings.Default,
                recoverabilityPolicy: (config, errorContext) =>
                {
                    return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
                },
                onMessage: OnMessage);
        }

        Task OnMessage(IServiceProvider serviceProvider, MessageContext context)
        {
            // To raise a critical error
            var exception = new Exception("CriticalError occurred");

            var criticalError = serviceProvider.GetService<CriticalError>();
            criticalError.Raise("Something bad happened - trigger critical error", exception);

            return Task.CompletedTask;
        }

        #endregion
    }

}
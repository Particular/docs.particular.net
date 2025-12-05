namespace Core.Features;

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

public class MyAdvancedSatelliteFeature :
    Feature
{
    #region AdvancedSatelliteFeatureSetup

    protected override void Setup(FeatureConfigurationContext context)
    {
        context.AddSatelliteReceiver(
            name: "CustomSatellite",
            transportAddress: new QueueAddress("targetQueue"),
            runtimeSettings: PushRuntimeSettings.Default,
            recoverabilityPolicy: (config, _) => RecoverabilityAction.MoveToError(config.Failed.ErrorQueue),
            onMessage: OnMessage);
    }

    Task OnMessage(IServiceProvider serviceProvider, MessageContext context, CancellationToken cancellationToken)
    {
        // To raise a critical error
        var exception = new Exception("CriticalError occurred");

        var criticalError = serviceProvider.GetRequiredService<CriticalError>();
        criticalError.Raise("Something bad happened - trigger critical error", exception, cancellationToken);

        return Task.CompletedTask;
    }

    #endregion
}
namespace Core.Features;

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
    protected override void Setup(FeatureConfigurationContext context)
    {
        context.AddSatelliteReceiver(
            name: "MyCustomSatellite",
            transportAddress: new QueueAddress("targetQueue"),
            runtimeSettings: PushRuntimeSettings.Default,
            recoverabilityPolicy: (config, _) => RecoverabilityAction.MoveToError(config.Failed.ErrorQueue),
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
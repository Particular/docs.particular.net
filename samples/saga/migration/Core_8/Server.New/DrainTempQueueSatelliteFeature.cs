#define POST_MIGRATION

#if POST_MIGRATION

using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Logging;
using NServiceBus.Routing;
using NServiceBus.Transport;
using System;
using System.Threading;
using System.Threading.Tasks;

#region DrainTempQueueSatellite
public class DrainTempQueueSatelliteFeature :
    Feature
{
    static ILog log = LogManager.GetLogger<DrainTempQueueSatelliteFeature>();

    public DrainTempQueueSatelliteFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;

        context.AddSatelliteReceiver(
            name: "DrainTempQueueSatellite",
            transportAddress: new QueueAddress("Samples.SagaMigration.Server.New"),
            runtimeSettings: new PushRuntimeSettings(maxConcurrency: 1),
            recoverabilityPolicy: (config, errorContext) =>
            {
                return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
            },
            onMessage: OnMessage);
    }

    Task OnMessage(IServiceProvider serviceProvider, MessageContext context, CancellationToken cancellationToken)
    {
        var receiveAddresses = serviceProvider.GetRequiredService<ReceiveAddresses>();
        var dispatcher = serviceProvider.GetRequiredService<IMessageDispatcher>();
        var headers = context.Headers;

        var message = new OutgoingMessage(context.NativeMessageId, headers, context.Body);
        var operation = new TransportOperation(message, new UnicastAddressTag(receiveAddresses.MainReceiveAddress));

        log.Info($"Moving message from {context.ReceiveAddress} to {receiveAddresses.MainReceiveAddress}");

        var operations = new TransportOperations(operation);
        return dispatcher.Dispatch(operations, context.TransportTransaction, cancellationToken);
    }
}
#endregion

#endif

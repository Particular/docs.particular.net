//#define POST_MIGRATION


#if POST_MIGRATION

using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Features;
using NServiceBus.Routing;
using NServiceBus.Transport;

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
        #region DrainTempQueueSatellite

        var settings = context.Settings;
        var qualifiedAddress = settings.LogicalAddress()
            .CreateQualifiedAddress("New");
        var tempQueue = settings.GetTransportAddress(qualifiedAddress);
        var mainQueue = settings.LocalAddress();

        context.AddSatelliteReceiver(
            name: "DrainTempQueueSatellite",
            transportAddress: tempQueue,
            runtimeSettings: new PushRuntimeSettings(maxConcurrency: 1),
            recoverabilityPolicy: (config, errorContext) =>
            {
                return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
            },
            onMessage: (builder, messageContext) =>
            {
                var dispatcher = builder.Build<IDispatchMessages>();
                var headers = messageContext.Headers;

                var message = new OutgoingMessage(messageContext.MessageId, headers, messageContext.Body);
                var operation = new TransportOperation(message, new UnicastAddressTag(mainQueue));

                log.Info($"Moving message from {tempQueue} to {mainQueue}");

                var operations = new TransportOperations(operation);
                return dispatcher.Dispatch(operations, messageContext.TransportTransaction, messageContext.Context);
            });

        #endregion
    }
}

#endif
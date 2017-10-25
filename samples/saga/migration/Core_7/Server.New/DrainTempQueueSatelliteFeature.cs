//#define POST_MIGRATION

#if POST_MIGRATION

using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Features;
using NServiceBus.ObjectBuilder;
using NServiceBus.Routing;
using NServiceBus.Transport;

#region DrainTempQueueSatellite
public class DrainTempQueueSatelliteFeature :
    Feature
{
    static ILog log = LogManager.GetLogger<DrainTempQueueSatelliteFeature>();
    string tempQueue;
    string mainQueue;

    public DrainTempQueueSatelliteFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;
        var qualifiedAddress = settings.LogicalAddress()
            .CreateQualifiedAddress("New");
        tempQueue = settings.GetTransportAddress(qualifiedAddress);
        mainQueue = settings.LocalAddress();

        context.AddSatelliteReceiver(
            name: "DrainTempQueueSatellite",
            transportAddress: tempQueue,
            runtimeSettings: new PushRuntimeSettings(maxConcurrency: 1),
            recoverabilityPolicy: (config, errorContext) =>
            {
                return RecoverabilityAction.MoveToError(config.Failed.ErrorQueue);
            },
            onMessage: OnMessage);
    }

    Task OnMessage(IBuilder builder, MessageContext context)
    {
        var dispatcher = builder.Build<IDispatchMessages>();
        var headers = context.Headers;

        var message = new OutgoingMessage(context.MessageId, headers, context.Body);
        var operation = new TransportOperation(message, new UnicastAddressTag(mainQueue));

        log.Info($"Moving message from {tempQueue} to {mainQueue}");

        var operations = new TransportOperations(operation);
        return dispatcher.Dispatch(operations, context.TransportTransaction, context.Extensions);
    }
}
#endregion

#endif

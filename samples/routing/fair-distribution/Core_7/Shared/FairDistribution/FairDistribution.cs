using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

public class FairDistribution :
    Feature
{
    public FairDistribution()
    {
        Defaults(s => s.Set(new FlowManager()));
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var sessionId = Guid.NewGuid().ToString();
        var flowManager = context.Settings.Get<FlowManager>();
        var controlAddress = context.Settings.InstanceSpecificQueue() ?? context.Settings.LocalAddress();

        var transportInfrastructure = context.Settings.Get<TransportInfrastructure>();

        var pipeline = context.Pipeline;
        pipeline.Register(
            stepId: "FlowControl.MessageMarker",
            behavior: new MessageMarker(flowManager),
            description: "Marks outgoing messages");
        pipeline.Register(
            stepId: "FlowControl.AcknowledgementProcessor",
            behavior: new AcknowledgementProcessor(flowManager, sessionId),
            description: "Processes incoming acknowledgements.");
        pipeline.Register(
            stepId: "FlowControl.MarkerProcessor",
            behavior: new MarkerProcessor(context.Settings.EndpointName(), context.Settings.LocalAddress(), 1),
            description: "Processes markers and sends ACKs");
        pipeline.Register(
            stepId: "FlowControl.SendMessageMarker",
            behavior: new SendMessageMarker(controlAddress, sessionId),
            description: "Marks point-to-point messages");
        pipeline.Register(
            stepId: "FlowControl.PublishMessageMarker",
            behavior: new PublishMessageMarker(controlAddress, sessionId),
            description: "Marks published messages");
    }
}
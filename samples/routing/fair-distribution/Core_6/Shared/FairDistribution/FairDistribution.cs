using System;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Routing;
using NServiceBus.Transports;

public class FairDistribution : Feature
{
    public FairDistribution()
    {
        Defaults(s => s.Set<FlowManager>(new FlowManager()));
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var sessionId = Guid.NewGuid().ToString();
        var flowManager = context.Settings.Get<FlowManager>();
        var controlAddress = context.Settings.InstanceSpecificQueue() ?? context.Settings.LocalAddress();

        var instance = context.Settings.Get<TransportInfrastructure>().BindToLocalEndpoint(
            new EndpointInstance(context.Settings.EndpointName(), context.Settings.GetOrDefault<string>("EndpointInstanceDiscriminator")));

        var pipeline = context.Pipeline;
        pipeline.Register("FlowControl.MessageMarker", new MessageMarker(flowManager), "Marks outgoing messages");
        pipeline.Register("FlowControl.AcknowledgementProcessor", new AcknowledgementProcessor(flowManager, sessionId), "Processes incoming acknowledgements.");
        pipeline.Register("FlowControl.MarkerProcessor", new MarkerProcessor(instance, 1), "Processes markers and sends ACKs");
        pipeline.Register("FlowControl.SendMessageMarker", new SendMessageMarker(controlAddress, sessionId), "Marks point-to-point messages");
        pipeline.Register("FlowControl.PublishMessageMarker", new PublishMessageMarker(controlAddress, sessionId), "Marks published messages");
    }
}
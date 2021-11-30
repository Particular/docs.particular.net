using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Features;
using System;

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

        var pipeline = context.Pipeline;
        pipeline.Register(
            stepId: "FlowControl.MessageMarker",
            behavior: new MessageMarker(flowManager),
            description: "Marks outgoing messages");
        pipeline.Register(
            stepId: "FlowControl.AcknowledgementProcessor",
            behavior: new AcknowledgementProcessor(flowManager, sessionId),
            description: "Processes incoming acknowledgements.");
        pipeline.Register(s => new MarkerProcessor(
            context.Settings.EndpointName(),
            s.GetRequiredService<ReceiveAddresses>().MainReceiveAddress, 1), "Processes markers and sends ACKs");

        pipeline.Register(sp => new SendMessageMarker(GetControlAddress(sp), sessionId), "Marks point-to-point messages");
        pipeline.Register(sp => new PublishMessageMarker(GetControlAddress(sp), sessionId), "Marks published messages");
    }

    string GetControlAddress(IServiceProvider serviceProvider)
    {
        var receiveAddresses = serviceProvider.GetRequiredService<ReceiveAddresses>();

        return receiveAddresses.InstanceReceiveAddress ?? receiveAddresses.MainReceiveAddress;
    }
}
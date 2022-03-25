using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Transport;

class HardcodeReplyToAddressToLogicalAddressFeature : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        context.Pipeline.Register(s =>
        {
            var instanceSpecificQueueAddress = s.GetRequiredService<ITransportAddressResolver>()
                                                .ToTransportAddress(context.InstanceSpecificQueueAddress());
            return new HardcodeReplyToAddressToLogicalAddress(instanceSpecificQueueAddress);
        },  "Hardcodes the ReplyToAddress to the instance specific address of this endpoint.");
    }
}
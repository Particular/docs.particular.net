using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Pipeline;

#region behavior-configuration

public class MultiSerializerFeature : Feature
{
    internal MultiSerializerFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var pipeline = context.Pipeline;
        pipeline.Replace(WellKnownStep.DeserializeMessages, typeof(DeserializeBehavior));
        pipeline.Replace(WellKnownStep.SerializeMessage, typeof(SerializeBehavior));
        var container = context.Container;
        container.ConfigureComponent<SerializationMapper>(DependencyLifecycle.SingleInstance);
    }
}

#endregion
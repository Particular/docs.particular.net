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
        context.Pipeline.Replace(WellKnownStep.DeserializeMessages, typeof(DeserializeBehavior));
        context.Pipeline.Replace(WellKnownStep.SerializeMessage, typeof(SerializeBehavior));

        context.Container.ConfigureComponent<SerializationMapper>(DependencyLifecycle.SingleInstance);
    }
}

#endregion
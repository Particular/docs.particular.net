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
        PipelineSettings pipeline = context.Pipeline;
        //TODO: the magic strings here are temporary until connector replacement is enable in the core
        pipeline.Replace("NServiceBus.DeserializeLogicalMessagesConnector", typeof(DeserializeBehavior));
        pipeline.Replace("NServiceBus.SerializeMessagesBehavior", typeof(SerializeBehavior));

        context.Container.ConfigureComponent<SerializationMapper>(DependencyLifecycle.SingleInstance);
    }
}

#endregion


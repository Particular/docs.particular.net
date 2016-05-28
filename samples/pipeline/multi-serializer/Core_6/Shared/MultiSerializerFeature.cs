using NServiceBus;
using NServiceBus.Features;

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
        pipeline.Replace("NServiceBus.DeserializeLogicalMessagesConnector", typeof(DeserializeConnector));
        pipeline.Replace("NServiceBus.SerializeMessageConnector", typeof(SerializeConnector));
        context.Container.ConfigureComponent<SerializationMapper>(DependencyLifecycle.SingleInstance);
    }
}

#endregion


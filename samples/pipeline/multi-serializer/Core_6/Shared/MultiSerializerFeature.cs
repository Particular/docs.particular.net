using NServiceBus;
using NServiceBus.Features;

#region behavior-configuration

public class MultiSerializerFeature :
    Feature
{
    internal MultiSerializerFeature()
    {
        EnableByDefault();
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var pipeline = context.Pipeline;
        pipeline.Replace("DeserializeLogicalMessagesConnector", typeof(DeserializeConnector));
        pipeline.Replace("SerializeMessageConnector", typeof(SerializeConnector));
        var container = context.Container;
        container.ConfigureComponent<SerializationMapper>(DependencyLifecycle.SingleInstance);
    }
}

#endregion


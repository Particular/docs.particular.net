using System;
using System.Reflection;
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
        //TODO: the reflection here is temporary until connector replacement is enable in the core
        Assembly nsbAssembly = typeof(Feature).Assembly;
        Type deserializeConnector = nsbAssembly.GetType("NServiceBus.DeserializeLogicalMessagesConnector", true);
        context.Pipeline.Replace(deserializeConnector.Name, typeof(DeserializeBehavior));
        Type serializeConnector = nsbAssembly.GetType("NServiceBus.SerializeMessagesBehavior", true);
        context.Pipeline.Replace(serializeConnector.Name, typeof(SerializeBehavior));

        context.Container.ConfigureComponent<SerializationMapper>(DependencyLifecycle.SingleInstance);
    }
}

#endregion


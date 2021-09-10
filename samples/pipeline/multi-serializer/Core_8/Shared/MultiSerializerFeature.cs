using Microsoft.Extensions.DependencyInjection;
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
        context.Services.AddSingleton<SerializationMapper>();
    }
}

#endregion


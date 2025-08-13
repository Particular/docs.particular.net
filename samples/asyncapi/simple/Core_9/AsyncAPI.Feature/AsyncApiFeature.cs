using Microsoft.Extensions.DependencyInjection;
using Neuroglia.AsyncApi.Generation;
using NServiceBus.Features;
using NServiceBus.Unicast.Messages;

sealed class AsyncApiFeature : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var conventions = context.Settings.Get<Conventions>();

        var messageMetadataRegistry = context.Settings.Get<MessageMetadataRegistry>();

        List<Type> events = [];

        //get a list of all events
        #region RegisterEvents
        foreach (var messageMetadata in messageMetadataRegistry.GetAllMessages())
        {
            if (conventions.IsEventType(messageMetadata.MessageType))
            {
                events.Add(messageMetadata.MessageType);
            }
        }

        context.Services.AddSingleton(new TypeCache
        {
            EndpointName = context.Settings.EndpointName(),
            Events = events
        });
        #endregion

        #region RegisterCustomDocumentGenerator
        context.Services.AddTransient<IAsyncApiDocumentGenerator>(
            provider => new ApiDocumentGenerator(provider));
        #endregion
    }
}
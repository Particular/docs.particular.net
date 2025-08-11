using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Neuroglia.AsyncApi.Generation;
using NServiceBus.Features;
using NServiceBus.Unicast.Messages;

namespace AsyncAPI.Feature;

public sealed class AsyncApiFeature : NServiceBus.Features.Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var conventions = context.Settings.Get<Conventions>();

        var messageMetadataRegistry = context.Settings.Get<MessageMetadataRegistry>();

        var proxyGenerator = new TypeProxyGenerator();

        Dictionary<Type, Type> publishedEventCache = new();
        Dictionary<string, (Type SubscribedType, Type ActualType)> subscribedEventCache = new();

        foreach (var messageMetadata in messageMetadataRegistry.GetAllMessages())
        {
            //NOTE only events decorated with the "PublishedEvent" or "SubscribedEvent" are being stored so that they can be "translated" at publish and subscribe time from their concrete types
            if (conventions.IsEventType(messageMetadata.MessageType))
            {
                var publishedEvent = messageMetadata.MessageType.GetCustomAttribute<PublishedEvent>();
                if (publishedEvent != null)
                {
                    publishedEventCache.Add(messageMetadata.MessageType,
                        proxyGenerator.CreateTypeFrom($"{publishedEvent.EventName}V{publishedEvent.Version}"));
                }

                var subscribedEvent = messageMetadata.MessageType.GetCustomAttribute<SubscribedEvent>();
                if (subscribedEvent != null)
                {
                    var subscribedType =
                        proxyGenerator.CreateTypeFrom($"{subscribedEvent.EventName}V{subscribedEvent.Version}");
                    subscribedEventCache.Add(subscribedType.FullName!,
                        (SubscribedType: subscribedType, ActualType: messageMetadata.MessageType));
                }
            }
        }

        if (context.Settings.GetOrDefault<bool>("Installers.Enable"))
        {
            context.RegisterStartupTask(static provider => new ManualSubscribe(provider.GetRequiredService<TypeCache>()
                .SubscribedEventCache.Values.Select(x => x.SubscribedType).ToArray()));
        }
       
        context.Pipeline.Register(
            static provider =>
                new ReplaceOutgoingEnclosedMessageTypeHeaderBehavior(provider.GetRequiredService<TypeCache>().PublishedEventCache),
            "Replaces the outgoing enclosed message type header with the published event type fullname");
        context.Pipeline.Register(
            static provider => new ReplaceMulticastRoutingBehavior(provider.GetRequiredService<TypeCache>().PublishedEventCache),
            "Replaces the multicast routing strategies that match the actual published event type with the published event type name");        

        if (!context.Settings.GetOrDefault<bool>("Endpoint.SendOnly"))
        {
            context.Pipeline.Register(
                static provider =>
                    new ReplaceIncomingEnclosedMessageTypeHeaderBehavior(provider.GetRequiredService<TypeCache>()
                        .SubscribedEventCache), "Replaces the incoming published event type name with the actual local event type name");
        }
        
        // with v8 registration will follow the regular MS DI stuff
        context.Services.AddSingleton(new TypeCache
            { EndpointName = context.Settings.EndpointName(), PublishedEventCache = publishedEventCache, SubscribedEventCache = subscribedEventCache });

        context.Services.AddTransient<IAsyncApiDocumentGenerator>(provider => new ApiDocumentGenerator(provider));
    }

    class ManualSubscribe : FeatureStartupTask
    {
        private Type[] subscribedEvents;

        public ManualSubscribe(Type[] subscribedEvents)
        {
            this.subscribedEvents = subscribedEvents;
        }

        protected override Task OnStart(IMessageSession session, CancellationToken cancellationToken = default)
        {
            return Task.WhenAll(subscribedEvents.Select(subscribedEvent => session.Subscribe(subscribedEvent)));
        }

        protected override Task OnStop(IMessageSession session, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
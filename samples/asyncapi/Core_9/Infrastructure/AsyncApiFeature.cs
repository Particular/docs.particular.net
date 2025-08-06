using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Unicast;
using NServiceBus.Unicast.Messages;

namespace Infrastructure;

public sealed class AsyncApiFeature : Feature
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
                    subscribedEventCache.Add(subscribedType.FullName,
                        (SubscribedType: subscribedType, ActualType: messageMetadata.MessageType));
                }
            }
            else if (conventions.IsCommandType(messageMetadata.MessageType))
            {
                // TODO
            }
        }

        //if (context.Settings.GetOrDefault<bool>("Installers.Enable"))
        //{
        //    context.RegisterStartupTask(static provider => new ManualSubscribe(provider.GetRequiredService<TypeCache>()
        //        .SubscribedEventCache.Values.Select(x => x.SubscribedType).ToArray()));
        //}

        //context.Pipeline.Register(
        //    static provider =>
        //        new ReplaceOutgoingEnclosedMessageTypeHeaderBehavior(provider.GetRequiredService<TypeCache>().PublishedEventCache),
        //    "Replaces the outgoing enclosed message type header with the published event type fullname");
        //context.Pipeline.Register(
        //    static provider => new ReplaceMulticastRoutingBehavior(provider.GetRequiredService<TypeCache>().PublishedEventCache),
        //    "Replaces the multicast routing strategies that match the actual published event type with the published event type name");

        //if (!context.Settings.GetOrDefault<bool>("Endpoint.SendOnly"))
        //{
        //    context.Pipeline.Register(
        //        static provider =>
        //            new ReplaceIncomingEnclosedMessageTypeHeaderBehavior(provider.GetRequiredService<TypeCache>()
        //                .SubscribedEventCache), "Replaces the incoming published event type name with the actual local event type name");
        //}

        // with v8 registration will follow the regular MS DI stuff
        context.Services.AddSingleton(new TypeCache
            { PublishedEventCache = publishedEventCache, SubscribedEventCache = subscribedEventCache });
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
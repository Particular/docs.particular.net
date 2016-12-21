using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Routing;
using NServiceBus.Routing.MessageDrivenSubscriptions;
using NServiceBus.Unicast;

class AutomaticRoutingFeature :
    Feature
{
    public AutomaticRoutingFeature()
    {
        Defaults(s => s.SetDefault("NServiceBus.AutomaticRouting.PublishedTypes", new Type[0]));
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var settings = context.Settings;
        var conventions = settings.Get<Conventions>();
        var uniqueKey = settings.InstanceSpecificQueue() ?? settings.LocalAddress();
        var unicastRoutingTable = settings.Get<UnicastRoutingTable>();
        var endpointInstances = settings.Get<EndpointInstances>();
        var publishers = settings.Get<Publishers>();
        var connectionString = settings.Get<string>("NServiceBus.AutomaticRouting.ConnectionString");
        var messageTypesPublished = settings.Get<Type[]>("NServiceBus.AutomaticRouting.PublishedTypes");

        #region Feature

        // Create the infrastructure
        var dataAccess = new SqlDataAccess(uniqueKey, connectionString);
        var communicator = new RoutingInfoCommunicator(dataAccess);
        context.RegisterStartupTask(communicator);

        // Register the routing info publisher
        context.RegisterStartupTask(builder =>
        {
            var handlerRegistry = builder.Build<MessageHandlerRegistry>();
            var messageTypesHandled = GetHandledCommands(handlerRegistry, conventions);
            return new RoutingInfoPublisher(
                dataBackplane: communicator,
                hanledMessageTypes: messageTypesHandled,
                publishedMessageTypes: messageTypesPublished,
                settings: settings,
                heartbeatPeriod: TimeSpan.FromSeconds(5));
        });

        // Register the routing info subscriber
        context.RegisterStartupTask(builder =>
        {
            var handlerRegistry = builder.Build<MessageHandlerRegistry>();
            var messageTypesHandled = GetHandledEvents(handlerRegistry, conventions);
            var subscriber = new RoutingInfoSubscriber(
                routingTable: unicastRoutingTable,
                endpointInstances: endpointInstances,
                messageTypesHandledByThisEndpoint: messageTypesHandled,
                publishers: publishers,
                sweepPeriod: TimeSpan.FromSeconds(5),
                heartbeatTimeout: TimeSpan.FromSeconds(20));
            communicator.Changed = subscriber.OnChanged;
            communicator.Removed = subscriber.OnRemoved;
            return subscriber;
        });

        context.Pipeline.Register(
            stepId: "VerifyAdvertisedBehavior",
            behavior: new VerifyAdvertisedBehavior(messageTypesPublished),
            description: "Verifies if all published types has been advertised.");

        #endregion
    }

    static List<Type> GetHandledCommands(MessageHandlerRegistry handlerRegistry, Conventions conventions)
    {
        // get all potential messages
        return handlerRegistry.GetMessageTypes()
            // never auto-route system messages and events
            .Where(t => !conventions.IsInSystemConventionList(t) && !conventions.IsEventType(t))
            .ToList();
    }

    static List<Type> GetHandledEvents(MessageHandlerRegistry handlerRegistry, Conventions conventions)
    {
        // get all potential messages
        return handlerRegistry.GetMessageTypes()
            // never auto-route system messages and events
            .Where(t => !conventions.IsInSystemConventionList(t) && conventions.IsEventType(t))
            .ToList();
    }
}
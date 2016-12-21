using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Unicast;

class AutomaticRoutingFeature :
    Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var routing = context.Settings.Get<IRoutingComponent>();

        var settings = context.Settings;
        var conventions = settings.Get<Conventions>();
        var uniqueKey = settings.InstanceSpecificQueue() ?? settings.LocalAddress();
        var connectionString = settings.Get<string>("NServiceBus.AutomaticRouting.ConnectionString");

        #region Feature

        // Create the infrastructure
        var dataAccess = new SqlDataAccess(uniqueKey, connectionString);
        var communicator = new RoutingInfoCommunicator(dataAccess);
        context.RegisterStartupTask(communicator);

        // Register the routing info publisher
        context.RegisterStartupTask(builder =>
        {
            var handlerRegistry = builder.Build<MessageHandlerRegistry>();
            return new RoutingInfoPublisher(
                dataBackplane: communicator,
                hanledCommandTypes: GetHandledCommands(handlerRegistry, conventions),
                handledEventTypes: GetHandledEvents(handlerRegistry, conventions),
                settings: settings,
                heartbeatPeriod: TimeSpan.FromSeconds(5));
        });

        // Register the routing info subscriber
        context.RegisterStartupTask(builder =>
        {
            var handlerRegistry = builder.Build<MessageHandlerRegistry>();
            var messageTypesHandled = GetHandledCommands(handlerRegistry, conventions);
            var subscriber = new RoutingInfoSubscriber(
                routingTable: routing.Sending,
                subscriberTable: routing.Publishing,
                sweepPeriod: TimeSpan.FromSeconds(5),
                heartbeatTimeout: TimeSpan.FromSeconds(20));
            communicator.Changed = subscriber.OnChanged;
            communicator.Removed = subscriber.OnRemoved;
            return subscriber;
        });
        
        #endregion
    }

    static List<Type> GetHandledCommands(MessageHandlerRegistry handlerRegistry, Conventions conventions)
    {
        // get all potential messages
        return handlerRegistry.GetMessageTypes()
            // never auto-route system messages
            .Where(t => !conventions.IsInSystemConventionList(t) && conventions.IsCommandType(t))
            .ToList();
    }

    static List<Type> GetHandledEvents(MessageHandlerRegistry handlerRegistry, Conventions conventions)
    {
        // get all potential messages
        return handlerRegistry.GetMessageTypes()
            // never auto-route system messages
            .Where(t => !conventions.IsInSystemConventionList(t) && conventions.IsEventType(t))
            .ToList();
    }
}
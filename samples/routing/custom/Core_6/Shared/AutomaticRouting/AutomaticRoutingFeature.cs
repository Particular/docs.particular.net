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

        //Create the infrastructure
        var dataAccess = new SqlDataAccess(uniqueKey, connectionString);
        var communicator = new RoutingInfoCommunicator(dataAccess);
        context.RegisterStartupTask(communicator);

        //Register the routing info publisher
        context.RegisterStartupTask(b =>
        {
            var messageTypesHandled = GetHandledMessages(b.Build<MessageHandlerRegistry>(), conventions);
            return new RoutingInfoPublisher(communicator, messageTypesHandled, messageTypesPublished, settings,
                TimeSpan.FromSeconds(5));
        });

        //Register the routing info subscriber
        context.RegisterStartupTask(b =>
        {
            var messageTypesHandled = GetHandledMessages(b.Build<MessageHandlerRegistry>(), conventions);
            var subscriber = new RoutingInfoSubscriber(unicastRoutingTable, endpointInstances, messageTypesHandled,
                publishers, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20));
            communicator.Changed = subscriber.OnChanged;
            communicator.Removed = subscriber.OnRemoved;
            return subscriber;
        });

        context.Pipeline.Register("VerifyAdvertisedBehavior", new VerifyAdvertisedBehavior(messageTypesPublished),
            "Verifies if all published types has been advertised.");

        #endregion
    }

    static List<Type> GetHandledMessages(MessageHandlerRegistry handlerRegistry, Conventions conventions)
    {
        return handlerRegistry.GetMessageTypes() //get all potential messages
            .Where(t => !conventions.IsInSystemConventionList(t)) //never auto-route system messages
            .ToList();
    }
}

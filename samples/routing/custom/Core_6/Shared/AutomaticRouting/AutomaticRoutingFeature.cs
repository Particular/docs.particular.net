using System;
using System.Collections.Generic;
using System.Linq;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Routing;
using NServiceBus.Routing.MessageDrivenSubscriptions;
using NServiceBus.Unicast;

class AutomaticRoutingFeature : Feature
{
    public AutomaticRoutingFeature()
    {
        Defaults(s => s.SetDefault("NServiceBus.AutomaticRouting.PublishedTypes", new Type[0]));
    }

    protected override void Setup(FeatureConfigurationContext context)
    {
        var conventions = context.Settings.Get<Conventions>();
        var uniqueKey = context.Settings.InstanceSpecificQueue() ?? context.Settings.LocalAddress();
        var unicastRoutingTable = context.Settings.Get<UnicastRoutingTable>();
        var endpointInstances = context.Settings.Get<EndpointInstances>();
        var publishers = context.Settings.Get<Publishers>();
        var connectionString = context.Settings.Get<string>("NServiceBus.AutomaticRouting.ConnectionString");
        var messageTypesPublished = context.Settings.Get<Type[]>("NServiceBus.AutomaticRouting.PublishedTypes");

#region Feature
        
        //Create the infrastructure
        var dataAccess = new SqlDataAccess(uniqueKey, connectionString);
        var communicator = new RoutingInfoCommunicator(dataAccess);
        context.RegisterStartupTask(communicator);

        //Register the routing info publisher
        context.RegisterStartupTask(b =>
        {
            var messageTypesHandled = GetHandledMessages(b.Build<MessageHandlerRegistry>(), conventions);
            return new RoutingInfoPublisher(communicator, messageTypesHandled, messageTypesPublished, context.Settings, 
                TimeSpan.FromSeconds(5));
        });

        //Register the routing info subscriber
        context.RegisterStartupTask(b =>
        {
            var messageTypesHandled = GetHandledMessages(b.Build<MessageHandlerRegistry>(), conventions);
            var subscriber = new RoutingInfoSubscriber(unicastRoutingTable, endpointInstances, messageTypesHandled, 
                publishers, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20));
            communicator.Changed += e => subscriber.OnChanged(e);
            communicator.Removed += e => subscriber.OnRemoved(e);
            return subscriber;
        });

        context.Pipeline.Register("VerifyAdvertisedBehavior", new VerifyAdvertisedBehavior(messageTypesPublished),
            "Verifies if all published types has been advertised.");

        #endregion
    }

    static List<Type> GetHandledMessages(MessageHandlerRegistry handlerRegistry, Conventions conventions)
    {
        var messageTypesHandled = handlerRegistry.GetMessageTypes()//get all potential messages
            .Where(t => !conventions.IsInSystemConventionList(t)) //never auto-route system messages
            .ToList();

        return messageTypesHandled;
    }
}

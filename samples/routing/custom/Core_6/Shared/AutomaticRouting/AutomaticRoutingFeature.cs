using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Routing;
using NServiceBus.Transports;
using NServiceBus.Unicast;
using NServiceBus.Unicast.Messages;

class AutomaticRoutingFeature : Feature
{
    protected override void Setup(FeatureConfigurationContext context)
    {
        var conventions = context.Settings.Get<Conventions>();
        var localAddress = context.Settings.LocalAddress();
        var unicastRoutingTable = context.Settings.Get<UnicastRoutingTable>();
        var endpointInstances = context.Settings.Get<EndpointInstances>();
        var transportAddresses = context.Settings.Get<TransportAddresses>();
        var connectionString = context.Settings.Get<string>("NServiceBus.AutomaticRouting.ConnectionString");
        var distributionPolicy = context.Settings.Get<DistributionPolicy>();

        //Processes the routing information from other endpoints
        var subscriber = new RoutingInfoSubscriber(context.Settings, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(20));

        var dataAccess = new SqlDataAccess(localAddress, connectionString);
        var communicator = new RoutingInfoCommunicator(dataAccess, subscriber.OnChanged, subscriber.OnRemoved);

        //Register the routing info publisher
        context.RegisterStartupTask(b =>
        {
            var messageTypesHandled = GetHandledMessages(b.Build<MessageHandlerRegistry>(), conventions);
            return new RoutingInfoPublisher(communicator, messageTypesHandled, context.Settings, TimeSpan.FromSeconds(5));
        });

        //Tell NSB to start the subscriber when the endpoint starts.
        context.RegisterStartupTask(subscriber);

        //Tell NSB to start the communicator when the endpoint starts.
        context.RegisterStartupTask(communicator);

        //Replace the message-driven pub/sub with no-op implementations because pub/sub is handled automatically.
        context.Pipeline.Replace("MessageDrivenSubscribeTerminator", b => new NoOpSubscribeTerminator());
        context.Pipeline.Replace("MessageDrivenUnsubscribeTerminator", b => new NoOpUnsubscribeTerminator());

        //Replace the publish router connector with one that uses the automatic routing
        context.Pipeline.Replace("UnicastPublishRouterConnector", b =>
        {
            var publishRouter = new AutomaticPublishRouter(unicastRoutingTable, b.Build<MessageMetadataRegistry>(), 
                endpointInstances, transportAddresses);
            return new UnicastPublishRouterConnector(publishRouter, distributionPolicy);
        });
    }

    static List<Type> GetHandledMessages(MessageHandlerRegistry handlerRegistry, Conventions conventions)
    {
        var messageTypesHandled = handlerRegistry.GetMessageTypes()//get all potential messages
            .Where(t => !conventions.IsInSystemConventionList(t)) //never auto-route system messages
            .ToList();

        return messageTypesHandled;
    }
}

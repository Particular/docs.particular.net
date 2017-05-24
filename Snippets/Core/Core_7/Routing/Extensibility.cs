namespace Core7.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Configuration.AdvanceExtensibility;
    using NServiceBus.Features;
    using NServiceBus.Routing;
    using NServiceBus.Routing.MessageDrivenSubscriptions;

    public class Extensibility
    {
        void RouteTableViaConfig(EndpointConfiguration endpointConfiguration)
        {
            #region RoutingExtensibility-RouteTableConfig

            var routingTable = endpointConfiguration.GetSettings().Get<UnicastRoutingTable>();
            routingTable.AddOrReplaceRoutes("MySource",
                    new List<RouteTableEntry>
                    {
                        new RouteTableEntry(typeof(MyCommand),
                            UnicastRoute.CreateFromEndpointName("MyEndpoint"))
                    });

            #endregion
        }

        class RefreshingRouteTable :
            Feature
        {
            #region RoutingExtensibility-StartupTaskRegistration
            protected override void Setup(FeatureConfigurationContext context)
            {
                var routingTable = context.Settings.Get<UnicastRoutingTable>();
                var refresherTask = new Refresher(routingTable);
                context.RegisterStartupTask(refresherTask);
            }
            #endregion

            class Refresher :
                FeatureStartupTask
            {
                UnicastRoutingTable routeTable;
                Timer timer;

                #region RoutingExtensibility-StartupTask

                public Refresher(UnicastRoutingTable routeTable)
                {
                    this.routeTable = routeTable;
                }

                protected override Task OnStart(IMessageSession session)
                {
                    timer = new Timer(_ =>
                    {
                        routeTable.AddOrReplaceRoutes("MySource", LoadRoutes());
                    }, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
                    return Task.CompletedTask;
                }

                #endregion

                IList<RouteTableEntry> LoadRoutes() => null;

                protected override Task OnStop(IMessageSession session) => Task.CompletedTask;
            }

            class RobustRefresher :
                FeatureStartupTask
            {
                UnicastRoutingTable routeTable;
                CriticalError criticalError;
                Timer timer;

                #region RoutingExtensibility-TriggerEndpointShutdown

                public RobustRefresher(UnicastRoutingTable routeTable, CriticalError criticalError)
                {
                    this.routeTable = routeTable;
                    this.criticalError = criticalError;
                }

                protected override Task OnStart(IMessageSession session)
                {
                    timer = new Timer(_ =>
                    {
                        try
                        {
                            routeTable.AddOrReplaceRoutes("MySource", LoadRoutes());
                        }
                        catch (Exception ex)
                        {
                            criticalError.Raise("Ambiguous route detected", ex);
                        }
                    }, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
                    return Task.CompletedTask;
                }

                #endregion

                IList<RouteTableEntry> LoadRoutes() => null;

                protected override Task OnStop(IMessageSession session) => Task.CompletedTask;
            }
        }

        class PublishersTable :
            Feature
        {
            #region RoutingExtensibility-Publishers
            protected override void Setup(FeatureConfigurationContext context)
            {
                var publishers = context.Settings.Get<Publishers>();
                var publisherAddress = PublisherAddress.CreateFromEndpointName("PublisherEndpoint");
                publishers.AddOrReplacePublishers("MySource",
                    new List<PublisherTableEntry>
                    {
                        new PublisherTableEntry(typeof(MyEvent), publisherAddress)
                    });
            }
            #endregion
        }

        class Instances :
            Feature
        {
            #region RoutingExtensibility-Instances
            protected override void Setup(FeatureConfigurationContext context)
            {
                var endpointInstances = context.Settings.Get<EndpointInstances>();
                endpointInstances.AddOrReplaceInstances("MySource",
                    new List<EndpointInstance>
                    {
                        new EndpointInstance("MyEndpoint").AtMachine("VM-1"),
                        new EndpointInstance("MyEndpoint").AtMachine("VM-2")
                    });
            }
            #endregion
        }

        void CustomDistributionStrategy(EndpointConfiguration endpointConfiguration)
        {
            #region RoutingExtensibility-Distribution

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            routing.SetMessageDistributionStrategy(new RandomStrategy("Sales", DistributionStrategyScope.Send));

            #endregion
        }

        #region RoutingExtensibility-DistributionStrategy

        class RandomStrategy :
            DistributionStrategy
        {
            static Random random = new Random();

            public RandomStrategy(string endpoint, DistributionStrategyScope scope) : base(endpoint, scope)
            {
            }

            // Method will not be called since SelectDestination doesn't call base.SelectDestination
            public override string SelectReceiver(string[] receiverAddresses)
            {
                throw new NotImplementedException();
            }

            public override string SelectDestination(DistributionContext context)
            {
                // access to headers, payload...
                return context.ReceiverAddresses[random.Next(context.ReceiverAddresses.Length)];
            }
        }

        #endregion

        class MyCommand :
            ICommand
        {
        }

        class MyEvent :
            IEvent
        {
        }
    }

}

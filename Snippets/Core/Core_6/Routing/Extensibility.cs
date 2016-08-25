namespace Core6.Routing
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
        void RouteTableViaConfig(EndpointConfiguration config)
        {
            #region RoutingExtensibility-RouteTableConfig

            config.GetSettings().Get<UnicastRoutingTable>().AddOrReplaceRoutes("MySource",
                    new List<RouteTableEntry>
                    {
                        new RouteTableEntry(typeof(MyCommand),
                            UnicastRoute.CreateFromEndpointName("MyEndpoint"))
                    });

            #endregion

        }

        class RefreshingRouteTable : Feature
        {
            #region RoutingExtensibility-StartupTaskRegistration
            protected override void Setup(FeatureConfigurationContext context)
            {
                var routeTable = context.Settings.Get<UnicastRoutingTable>();
                var refresherTask = new Refresher(routeTable);
                context.RegisterStartupTask(refresherTask);
            }
            #endregion

            class Refresher : FeatureStartupTask
            {
                readonly UnicastRoutingTable routeTable;
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
                    return Task.FromResult(0);
                }

                #endregion

                IList<RouteTableEntry> LoadRoutes()
                {
                    return null;
                }

                protected override Task OnStop(IMessageSession session)
                {
                    return Task.FromResult(0);
                }
            }

            class RobustRefresher : FeatureStartupTask
            {
                readonly UnicastRoutingTable routeTable;
                readonly CriticalError criticalError;
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
                    return Task.FromResult(0);
                }

                #endregion

                IList<RouteTableEntry> LoadRoutes()
                {
                    return null;
                }

                protected override Task OnStop(IMessageSession session)
                {
                    return Task.FromResult(0);
                }
            }
        }

        class PublishersTable : Feature
        {
            #region RoutingExtensibility-Publishers
            protected override void Setup(FeatureConfigurationContext context)
            {
                context.Settings.Get<Publishers>().AddOrReplacePublishers("MySource",
                    new List<PublisherTableEntry>
                    {
                        new PublisherTableEntry(typeof(MyEvent),
                            PublisherAddress.CreateFromEndpointName("PublisherEndpoint"))
                    });
            }
            #endregion
        }

        class Instances : Feature
        {
            #region RoutingExtensibility-Instances
            protected override void Setup(FeatureConfigurationContext context)
            {
                context.Settings.Get<EndpointInstances>().AddOrReplaceInstances("MySource",
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

        class RandomStrategy : DistributionStrategy
        {
            static readonly Random r = new Random();

            public RandomStrategy(string endpoint, DistributionStrategyScope scope) : base(endpoint, scope)
            {
            }

            public override string SelectReceiver(string[] receiverAddresses)
            {
                return receiverAddresses[r.Next(receiverAddresses.Length)];
            }
        }

        #endregion

        class MyCommand : ICommand
        {
        }

        class MyEvent : IEvent
        {

        }
    }
}
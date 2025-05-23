﻿namespace Core6.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Configuration.AdvancedExtensibility;
    using NServiceBus.Features;
    using NServiceBus.Routing;
    using NServiceBus.Routing.MessageDrivenSubscriptions;

    public class Extensibility
    {
        public void RouteTableViaConfig(EndpointConfiguration endpointConfiguration)
        {
            #region RoutingExtensibility-RouteTableConfig

            var settings = endpointConfiguration.GetSettings();
            var routingTable = settings.GetOrCreate<UnicastRoutingTable>();
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
                UnicastRoutingTable routingTable;
                Timer timer;

                #region RoutingExtensibility-StartupTask

                public Refresher(UnicastRoutingTable routingTable)
                {
                    this.routingTable = routingTable;
                }

                protected override Task OnStart(IMessageSession session)
                {
                    timer = new Timer(
                        callback: _ =>
                        {
                            routingTable.AddOrReplaceRoutes("MySource", LoadRoutes());
                        },
                        state: null,
                        dueTime: TimeSpan.FromSeconds(30),
                        period: TimeSpan.FromSeconds(30));
                    return Task.CompletedTask;
                }

                #endregion

                IList<RouteTableEntry> LoadRoutes() => null;

                protected override Task OnStop(IMessageSession session) => Task.CompletedTask;
            }

            class RobustRefresher :
                FeatureStartupTask
            {
                UnicastRoutingTable routingTable;
                CriticalError criticalError;
                Timer timer;

                #region RoutingExtensibility-TriggerEndpointShutdown

                public RobustRefresher(UnicastRoutingTable routingTable, CriticalError criticalError)
                {
                    this.routingTable = routingTable;
                    this.criticalError = criticalError;
                }

                protected override Task OnStart(IMessageSession session)
                {
                    timer = new Timer(
                        callback: _ =>
                        {
                            try
                            {
                                routingTable.AddOrReplaceRoutes("MySource", LoadRoutes());
                            }
                            catch (Exception exception)
                            {
                                criticalError.Raise("Ambiguous route detected", exception);
                            }
                        },
                        state: null,
                        dueTime: TimeSpan.FromSeconds(30),
                        period: TimeSpan.FromSeconds(30));
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
                publishers.AddOrReplacePublishers(
                    sourceKey: "MySource",
                    entries: new List<PublisherTableEntry>
                    {
                        new PublisherTableEntry(typeof(MyEvent), publisherAddress)
                    });
            }

            #endregion
        }

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
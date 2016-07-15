// ReSharper disable UnusedParameter.Local

namespace Core6.Routing
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transport;

    class RoutingAPIs
    {
        void StaticRoutesEndpoint(TransportExtensions transportExtensions)
        {
            #region Routing-StaticRoutes-Endpoint

            var routing = transportExtensions.Routing();
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");

            #endregion
        }

        void StaticRoutesEndpointMsmq(TransportExtensions transportExtensions)
        {
            #region Routing-StaticRoutes-Endpoint-Msmq

            var routing = transportExtensions.Routing();
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");

            #endregion
        }

        void StaticRoutesEndpointBroker(TransportExtensions transportExtensions)
        {
            #region Routing-StaticRoutes-Endpoint-Broker

            var routing = transportExtensions.Routing();
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");

            #endregion
        }

        void StaticRoutesAddress(TransportExtensions transportExtensions)
        {
            #region Routing-StaticRoutes-Address

            var routing = transportExtensions.Routing();
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");

            #endregion
        }

        #region Routing-DynamicRoutes

        class DynamicRouting : Feature
        {
            protected override void Setup(FeatureConfigurationContext context)
            {
                var routingTable = context.RoutingTable();
                routingTable.AddDynamic((types, contextBag) => new[]
                {
                    // Use endpoint name
                    UnicastRoute.CreateFromEndpointName("Sales"),
                    // Use endpoint instance name
                    UnicastRoute.CreateFromEndpointInstance(new EndpointInstance("Sales", "1")),
                    // Use transport address (e.g. MSMQ)
                    UnicastRoute.CreateFromEndpointName("Sales-2@MachineA")
                });
            }
        }

        #endregion

        #region Routing-CustomRoutingStore

        class CustomRoutingStore : Feature
        {
            protected override void Setup(FeatureConfigurationContext context)
            {
                var routingTable = context.RoutingTable();
                routingTable.AddDynamic((t, c) =>
                LoadFromCache(t) ?? LoadFromDatabaseAndPutToCache(t));
            }
        }

        #endregion

        static IEnumerable<IUnicastRoute> LoadFromDatabaseAndPutToCache(Type[] type)
        {
            throw new NotImplementedException();
        }

        static IEnumerable<IUnicastRoute> LoadFromCache(Type[] type)
        {
            throw new NotImplementedException();
        }

        #region Routing-StaticEndpointMapping
        class StaticEndpointMapping : Feature
        {
            protected override void Setup(FeatureConfigurationContext context)
            {
                var sales = "Sales";

                var endpointInstances = context.EndpointInstances();
                endpointInstances.Add(
                    new EndpointInstance(sales, "1"),
                    new EndpointInstance(sales, "2"));
            }
        }
        #endregion

        #region Routing-DynamicEndpointMapping
        class DynamicEndpointMapping : Feature
        {
            protected override void Setup(FeatureConfigurationContext context)
            {
                var endpointInstances = context.EndpointInstances();
                endpointInstances.AddDynamic(async e =>
                {
                    if (e.ToString().StartsWith("Sales"))
                    {
                        return new[]
                        {
                        new EndpointInstance(e, "1").SetProperty("SomeProp", "SomeValue"),
                        new EndpointInstance(e, "2").AtMachine("B")
                    };
                    }
                    return null;
                });
            }
        }
        #endregion

        void CustomDistributionStrategy(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-CustomDistributionStrategy

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            routing.SetMessageDistributionStrategy("Sales", new CustomStrategy());
            #endregion
        }

        void SpecialCaseTransportAddress(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-SpecialCaseTransportAddress

            var endpointInstance = new EndpointInstance("Sales", "1");
            var transport = endpointConfiguration.UseTransport<MyTransport>();
            transport.AddAddressTranslationException(endpointInstance, "Sales-One@MachineA");

            #endregion
        }

        // ReSharper disable once ConvertClosureToMethodGroup
        void TransportAddressRules(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-TransportAddressRule

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            transport.AddAddressTranslationRule(i => CustomTranslationRule(i));

            #endregion
        }

        string CustomTranslationRule(LogicalAddress endpointInstanceName)
        {
            throw new NotImplementedException();
        }

        void FileBasedRouting(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-FileBased-Config

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            var routingTable = routing.InstanceMappingFile();
            routingTable.FilePath(@"C:\Routes.xml");
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");
            routing.RouteToEndpoint(typeof(SendOrder), "Shipping");

            #endregion
        }

        public void FileBasedRoutingRefreshInterval(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-FileBased-RefreshInterval


            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            var routingTable = routing.InstanceMappingFile();
            var fileRoutingTable = routingTable.FilePath(@"C:\Routes.xml");
            fileRoutingTable.RefreshInterval(TimeSpan.FromSeconds(45));

            #endregion
        }

        public void InstanceMappingFilePath(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-FileBased-FilePath

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();
            var routingTable = routing.InstanceMappingFile();
            routingTable.FilePath(@"C:\Routes.xml");

            #endregion
        }

        interface IUseCustomDistributionStrategy
        {
        }

        class CustomStrategy :
            DistributionStrategy
        {
            public override IEnumerable<UnicastRoutingTarget> SelectDestination(IList<UnicastRoutingTarget> allInstances)
            {
                throw new NotImplementedException();
            }
        }

        class AcceptOrder
        {
        }

        class SendOrder
        {
        }

        class OrderAccepted
        {
        }

        class MyTransport :
            TransportDefinition
        {
            public override TransportInfrastructure Initialize(SettingsHolder settings, string connectionString)
            {
                throw new NotImplementedException();
            }

            public override string ExampleConnectionStringForErrorMessage { get; }
        }
    }
}
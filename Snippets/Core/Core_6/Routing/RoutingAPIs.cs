// ReSharper disable UnusedParameter.Local

namespace Core6.Routing
{
    using System;
    using System.Collections.Generic;
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transport;

    class RoutingAPIs
    {
        void StaticRoutesEndpoint(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-StaticRoutes-Endpoint

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");

            #endregion
        }

        void StaticRoutesEndpointMsmq(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-StaticRoutes-Endpoint-Msmq

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");

            #endregion
        }

        void StaticRoutesEndpointBroker(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-StaticRoutes-Endpoint-Broker

            var transport = endpointConfiguration.UseTransport<MyTransport>();
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");

            #endregion
        }

        void StaticRoutesAddress(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-StaticRoutes-Address

            var routing = endpointConfiguration.Routing();
            routing.Mapping.Logical
                .RouteToAddress(typeof(AcceptOrder), "Sales@SomeMachine");

            #endregion
        }

        void DynamicRoutes(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-DynamicRoutes

            var routing = endpointConfiguration.Routing();
            routing.Mapping.Logical
                .AddDynamic((types, contextBag) => new[]
                {
                    // Use endpoint name
                    UnicastRoute.CreateFromEndpointName("Sales"),
                    // Use endpoint instance name
                    UnicastRoute.CreateFromEndpointInstance(new EndpointInstance("Sales", "1")),
                    // Use transport address (e.g. MSMQ)
                    UnicastRoute.CreateFromEndpointName("Sales-2@MachineA")
                });

            #endregion
        }

        void CustomRoutingStore(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-CustomRoutingStore

            var routing = endpointConfiguration.Routing();
            routing.Mapping.Logical.AddDynamic((t, c) =>
                LoadFromCache(t) ?? LoadFromDatabaseAndPutToCache(t));

            #endregion
        }
        void StaticEndpointMapping(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-StaticEndpointMapping

            var sales = "Sales";
            var routing = endpointConfiguration.Routing();
            routing.Mapping.Physical
                .Add(
                    new EndpointInstance(sales, "1"),
                    new EndpointInstance(sales, "2"));

            #endregion
        }

        void DynamicEndpointMapping(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-DynamicEndpointMapping

            var routing = endpointConfiguration.Routing();
            routing.Mapping.Physical.AddDynamic(async e =>
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

            #endregion
        }

        void CustomDistributionStrategy(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-CustomDistributionStrategy

            var routing = endpointConfiguration.Routing();
            routing.Mapping.SetMessageDistributionStrategy("Sales", new CustomStrategy());
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

            var routing = endpointConfiguration.Routing();
            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");
            routing.RouteToEndpoint(typeof(SendOrder), "Shipping");

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            transport.DistributeMessagesUsingFileBasedEndpointInstanceMapping(@"C:\Routes.xml");

            #endregion
        }

        public void FileBasedRoutingAdvanced(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-FileBased-ConfigAdvanced

            var routing = endpointConfiguration.Routing();
            routing.Mapping
                .DistributeMessagesUsingFileBasedEndpointInstanceMapping(@"C:\Routes.xml");

            #endregion
        }
        public void FileBasedRoutingMaxLoadAttempts(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-FileBased-MaxLoadAttempts

            var routing = endpointConfiguration.Routing();
            var fileRoutingTable = routing.Mapping
                .DistributeMessagesUsingFileBasedEndpointInstanceMapping(@"C:\Routes.xml");
            fileRoutingTable.MaxLoadAttempts(15);

            #endregion
        }
        public void FileBasedRoutingRefreshInterval(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-FileBased-RefreshInterval

            var routing = endpointConfiguration.Routing();
            var fileRoutingTable = routing.Mapping
                .DistributeMessagesUsingFileBasedEndpointInstanceMapping(@"C:\Routes.xml");
            fileRoutingTable.RefreshInterval(TimeSpan.FromSeconds(45));

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

        IEnumerable<IUnicastRoute> LoadFromDatabaseAndPutToCache(List<Type> type)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IUnicastRoute> LoadFromCache(List<Type> type)
        {
            throw new NotImplementedException();
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

// ReSharper disable UnusedParameter.Local
namespace Snippets6.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Routing;
    using NServiceBus.Settings;
    using NServiceBus.Transports;

    public class RoutingAPIs
    {
        public void StaticRoutes()
        {
            var busConfiguration = new BusConfiguration();
            #region Routing-StaticRoutes-Endpoint
            busConfiguration.Routing().UnicastRoutingTable.RouteToEndpoint(typeof(AcceptOrder), "Sales");
            #endregion

            #region Routing-StaticRoutes-Address
            busConfiguration.Routing().UnicastRoutingTable.RouteToAddress(typeof(AcceptOrder), "Sales@SomeMachine");
            #endregion
        }

        public void DynamicRoutes()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Routing-DynamicRoutes
            busConfiguration.Routing().UnicastRoutingTable
                .AddDynamic((types, contextBag) => new[]
                {
                    //Use endpoint name
                    new UnicastRoute("Sales"),
                    //Use endpoint instance name
                    new UnicastRoute(new EndpointInstance("Sales", "1")),
                    //Use transport address (e.g. MSMQ)
                    new UnicastRoute("Sales-2@MachineA")
                });
            #endregion
        }

        public void CustomRoutingStore()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region Routing-CustomRoutingStore

            busConfiguration.Routing().UnicastRoutingTable.AddDynamic((t, c) =>
                LoadFromCache(t) ?? LoadFromDatabaseAndPutToCache(t));

            #endregion
        }

        public void StaticEndpointMapping()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Routing-StaticEndpointMapping
            EndpointName sales = new EndpointName("Sales");
            busConfiguration.Routing().EndpointInstances
                .AddStatic(sales, new EndpointInstance(sales, "1", null), new EndpointInstance(sales, "2", null));
            #endregion
        }

        public void DynamicEndpointMapping()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region Routing-DynamicEndpointMapping

            busConfiguration.Routing().EndpointInstances.AddDynamic(e =>
            {
                if (e.ToString().StartsWith("Sales"))
                {
                    EndpointInstance[] instances =
                    {
                        new EndpointInstance(e, "1").SetProperty("SomeProp", "SomeValue"),
                        new EndpointInstance(e, "2").AtMachine("B")
                    };
                    return Task.FromResult<IEnumerable<EndpointInstance>>(instances);
                }
                return null;
            });

            #endregion
        }

        public void SpecialCaseTransportAddress()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Routing-SpecialCaseTransportAddress
            busConfiguration
                .UseTransport<MyTransport>()
                .AddAddressTranslationException(new EndpointInstance("Sales", "1"), "Sales-One@MachineA");
            #endregion
        }

        // ReSharper disable once ConvertClosureToMethodGroup
        public void TransportAddressRules()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Routing-TransportAddressRule
            busConfiguration
                .UseTransport<MyTransport>()
                .AddAddressTranslationRule(i => CustomTranslationRule(i));
            #endregion
        }

        string CustomTranslationRule(LogicalAddress endpointInstanceName)
        {
            throw new NotImplementedException();
        }

        public void FileBasedRouting()
        {
            var busConfiguration = new BusConfiguration();
            #region Routing-FileBased-Config
            busConfiguration.Routing().UnicastRoutingTable.RouteToEndpoint(typeof(AcceptOrder), "Sales");
            busConfiguration.Routing().UnicastRoutingTable.RouteToEndpoint(typeof(SendOrder), "Shipping");
            busConfiguration.Routing().UseFileBasedEndpointInstanceMapping(@"C:\Routes.xml");
            #endregion
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

        class MyTransport : TransportDefinition
        {
            protected override TransportReceivingConfigurationResult ConfigureForReceiving(TransportReceivingConfigurationContext context)
            {
                throw new NotImplementedException();
            }

            protected override TransportSendingConfigurationResult ConfigureForSending(TransportSendingConfigurationContext context)
            {
                throw new NotImplementedException();
            }

            public override IEnumerable<Type> GetSupportedDeliveryConstraints()
            {
                throw new NotImplementedException();
            }

            public override TransportTransactionMode GetSupportedTransactionMode()
            {
                throw new NotImplementedException();
            }

            public override IManageSubscriptions GetSubscriptionManager()
            {
                throw new NotImplementedException();
            }

            public override EndpointInstance BindToLocalEndpoint(EndpointInstance instance, ReadOnlySettings settings)
            {
                throw new NotImplementedException();
            }

            public override string ToTransportAddress(LogicalAddress logicalAddress)
            {
                throw new NotImplementedException();
            }

            public override OutboundRoutingPolicy GetOutboundRoutingPolicy(ReadOnlySettings settings)
            {
                throw new NotImplementedException();
            }

            public override string ExampleConnectionStringForErrorMessage { get; }
        }
    }
}
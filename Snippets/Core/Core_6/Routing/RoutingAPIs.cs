// ReSharper disable UnusedParameter.Local

namespace Core6.Routing
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Features;
    using NServiceBus.Routing;

    class RoutingAPIs
    {
        void StaticRoutesEndpoint(TransportExtensions transportExtensions)
        {
            #region Routing-StaticRoutes-Endpoint

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
                routingTable.SetFallbackRoute((type, contextBag) =>
                        Task.FromResult<IUnicastRoute>(UnicastRoute.CreateFromEndpointName("Sales")));
            }
        }

        #endregion

        #region Routing-CustomRoutingStore

        class CustomRoutingStore : Feature
        {
            protected override void Setup(FeatureConfigurationContext context)
            {
                var routingTable = context.RoutingTable();
                routingTable.SetFallbackRoute((type, c) =>
                {
                    var cacheResult = LoadFromCache(type);
                    if (cacheResult != null)
                    {
                        return Task.FromResult(cacheResult);
                    }

                    return LoadFromDatabaseAndPutToCache(type);
                });
            }
        }

        #endregion

        static Task<IUnicastRoute> LoadFromDatabaseAndPutToCache(Type type)
        {
            throw new NotImplementedException();
        }

        static IUnicastRoute LoadFromCache(Type type)
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

        void MapMessagesToLogicalEndpoints(EndpointConfiguration endpointConfiguration)
        {
            #region Routing-MapMessagesToLogicalEndpoints

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();
            var routing = transport.Routing();

            routing.RouteToEndpoint(typeof(AcceptOrder), "Sales");
            routing.RouteToEndpoint(typeof(SendOrder), "Shipping");

            #endregion
        }

        class CustomStrategy :
            DistributionStrategy
        {
            public override UnicastRoutingTarget SelectDestination(UnicastRoutingTarget[] allInstances)
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
    }
}
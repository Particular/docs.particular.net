// ReSharper disable UnusedParameter.Local
namespace Snippets6.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Routing;

    public class RoutingAPIs
    {

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
                .UseTransport<MsmqTransport>()
                .AddAddressTranslationException(new EndpointInstance("Sales", "1"), "Sales-One@MachineA");
            #endregion
        }

        // ReSharper disable once ConvertClosureToMethodGroup
        public void TransportAddressRules()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region Routing-TransportAddressRule
            busConfiguration
                .UseTransport<MsmqTransport>()
                .AddAddressTranslationRule(i => CustomTranslationRule(i));
            #endregion
        }

        string CustomTranslationRule(LogicalAddress endpointInstanceName)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IUnicastRoute> LoadFromDatabaseAndPutToCache(List<Type> type)
        {
            throw new NotImplementedException();
        }

        IEnumerable<IUnicastRoute> LoadFromCache(List<Type> type)
        {
            throw new NotImplementedException();
        }

        class OrderAccepted
        {
        }
    }
}
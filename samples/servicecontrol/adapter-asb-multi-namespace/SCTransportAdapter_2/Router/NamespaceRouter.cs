namespace Router
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using NServiceBus;
    using NServiceBus.Router;
    using ServiceControl.TransportAdapter;

    class NamespaceRouter
    {
        readonly string name;
        readonly NamespaceDescription[] endpointNamespaces;
        readonly string serviceControlMainQueue;
        readonly string serviceControlConnectionString;
        List<ITransportAdapter> adapters = new List<ITransportAdapter>();
        IRouter router;

        public NamespaceRouter(string name, NamespaceDescription[] endpointNamespaces, string serviceControlMainQueue, string serviceControlConnectionString)
        {
            this.name = name;
            this.endpointNamespaces = endpointNamespaces;
            this.serviceControlMainQueue = serviceControlMainQueue;
            this.serviceControlConnectionString = serviceControlConnectionString;
        }

        public async Task Start()
        {
            var routerConfig = new RouterConfiguration(name);

            foreach (var namespaceDescription in endpointNamespaces)
            {
                #region RouterInterface

                routerConfig.AddInterface<AzureServiceBusTransport>(namespaceDescription.Name, t =>
                {
                    t.ConnectionString(namespaceDescription.ConnectionString);
                    t.Transactions(TransportTransactionMode.ReceiveOnly);
                });

                #endregion

                var transportAdapterConfig = new TransportAdapterConfig<AzureServiceBusTransport, AzureServiceBusTransport>($"{namespaceDescription.Name}.Adapter");


                #region AdapterInterface
                transportAdapterConfig.CustomizeEndpointTransport(
                    t =>
                    {
                        t.ConnectionString(namespaceDescription.ConnectionString);
                        t.Transactions(TransportTransactionMode.ReceiveOnly);
                    });
                
                transportAdapterConfig.CustomizeServiceControlTransport(
                    t =>
                    {
                        t.ConnectionString(serviceControlConnectionString);
                        t.Transactions(TransportTransactionMode.ReceiveOnly);
                    });

                transportAdapterConfig.ServiceControlSideControlQueue = serviceControlMainQueue;

                #endregion

                var adapter = TransportAdapter.Create(transportAdapterConfig);
                adapters.Add(adapter);
            }


            var staticRouting = routerConfig.UseStaticRoutingProtocol();
            foreach (var namespaceDescription in endpointNamespaces)
            {
                #region ConventionBasedRouting

                staticRouting.AddRoute(
                    destinationFilter: (iface, destination) =>
                    {
                        return destination.Endpoint.EndsWith(namespaceDescription.Name);
                    },
                    destinationFilterDescription: $"To {namespaceDescription.Name}",
                    gateway: null,
                    iface: namespaceDescription.Name);

                #endregion

            }

            routerConfig.AutoCreateQueues();

            router = Router.Create(routerConfig);
            await router.Start();

            foreach (var adapter in adapters)
            {
                await adapter.Start();
            }
        }

        public async Task Stop()
        {
            foreach (var adapter in adapters)
            {
                await adapter.Stop();
            }

            await router.Stop();
        }
    }
}
namespace Snippets6
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using NServiceBus;

    public class Hosting
    {
        public async Task SendOnly()
        {
            #region Hosting-SendOnly

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.SendOnly();
            IEndpointInstance endpointInstance = await Endpoint.Start(busConfiguration);

            #endregion
        }

        public async Task Startup()
        {
            #region Hosting-Startup
            BusConfiguration busConfiguration = new BusConfiguration();
            //Apply configuration
            IInitializableEndpoint initializableEndpoint = Endpoint.Create(busConfiguration);
            IStartableEndpoint startableEndpoint = await initializableEndpoint.Initialize();
            IEndpointInstance endpointInstance = await startableEndpoint.Start();

            //Shortcut
            IEndpointInstance endpointInstance2 = await Endpoint.Start(busConfiguration);
            #endregion
        }

        public async Task Shutdown()
        {
            IEndpointInstance endpointInstance = null;

            #region Hosting-Shutdown
            await endpointInstance.Stop();
            #endregion
        }

        #region Hosting-Static
        public static class EndpointInstance
        {
            public static IEndpointInstance Endpoint { get; private set; }
            public static void SetInstance(IEndpointInstance endpoint)
            {
                if (Endpoint != null)
                {
                    throw new Exception("Endpoint already set.");
                }
                Endpoint = endpoint;
            }
        }
        #endregion

        public async Task InjectEndpoint()
        {
            #region Hosting-Inject
            ContainerBuilder containerBuilder = new ContainerBuilder();

            BusConfiguration busConfiguration = new BusConfiguration();
            IEndpointInstance endpointInstance = await Endpoint.Start(busConfiguration);
            containerBuilder.Register(_ => endpointInstance.CreateBusContext()).InstancePerDependency();

            #endregion

        }

    }
}
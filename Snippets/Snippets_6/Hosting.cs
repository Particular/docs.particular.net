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

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.SendOnly();
            IEndpointInstance endpointInstance = await Endpoint.Start(configuration);

            #endregion
        }

        public async Task Startup()
        {
            #region Hosting-Startup
            EndpointConfiguration configuration = new EndpointConfiguration();
            //Apply configuration
            IInitializableEndpoint initializableEndpoint = Endpoint.Prepare(configuration);
            IStartableEndpoint startableEndpoint = await initializableEndpoint.Initialize();
            IEndpointInstance endpointInstance = await startableEndpoint.Start();

            //Shortcut
            IEndpointInstance endpointInstance2 = await Endpoint.Start(configuration);
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

            EndpointConfiguration configuration = new EndpointConfiguration();
            IEndpointInstance endpointInstance = await Endpoint.Start(configuration);
            containerBuilder.Register(_ => endpointInstance).InstancePerDependency();

            #endregion

        }

    }
}
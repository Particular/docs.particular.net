namespace Core6
{
    using System;
    using System.Threading.Tasks;
    using Autofac;
    using NServiceBus;

    class Hosting
    {
        async Task SendOnly()
        {
            #region Hosting-SendOnly

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");
            endpointConfiguration.SendOnly();
            IEndpointInstance endpointInstance = await Endpoint.Start(endpointConfiguration);

            #endregion
        }

        async Task Startup()
        {
            #region Hosting-Startup
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");
            //Apply configuration
            IStartableEndpoint startableEndpoint = await Endpoint.Create(endpointConfiguration);
            IEndpointInstance endpointInstance = await startableEndpoint.Start();

            //Shortcut
            IEndpointInstance endpointInstance2 = await Endpoint.Start(endpointConfiguration);
            #endregion
        }

        async Task Shutdown(IEndpointInstance endpointInstance)
        {
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

        async Task InjectEndpoint()
        {
            #region Hosting-Inject
            ContainerBuilder containerBuilder = new ContainerBuilder();

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");
            IEndpointInstance endpointInstance = await Endpoint.Start(endpointConfiguration);
            containerBuilder.Register(_ => endpointInstance).InstancePerDependency();

            #endregion

        }

    }
}
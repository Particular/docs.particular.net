namespace Core8
{
    using System;
    using System.Threading.Tasks;
    using NServiceBus;

    class Hosting
    {
        async Task SendOnly()
        {
            #region Hosting-SendOnly

            var endpointConfiguration = new EndpointConfiguration("EndpointName");
            endpointConfiguration.SendOnly();
            // Apply other necessary endpoint configuration, e.g. transport
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            #endregion
        }

        async Task Startup()
        {
            #region Hosting-Startup
            var endpointConfiguration = new EndpointConfiguration("EndpointName");
            // Apply other necessary endpoint configuration, e.g. transport
            var startableEndpoint = await Endpoint.Create(endpointConfiguration)
                .ConfigureAwait(false);
            var endpointInstance = await startableEndpoint.Start()
                .ConfigureAwait(false);

            // Shortcut
            var endpointInstance2 = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);
            #endregion
        }

        async Task Shutdown(IEndpointInstance endpointInstance)
        {
            #region Hosting-Shutdown
            await endpointInstance.Stop().ConfigureAwait(false);
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
    }
}

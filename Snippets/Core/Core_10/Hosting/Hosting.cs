namespace Core.Hosting;

using System;
using System.Threading.Tasks;
using NServiceBus;

class Hosting
{
    void SendOnly()
    {
        #region Hosting-SendOnly

        var endpointConfiguration = new EndpointConfiguration("EndpointName");
        endpointConfiguration.SendOnly();

        #endregion
    }

#pragma warning disable CS0618 // Type or member is obsolete
    async Task Startup()
    {
        #region Hosting-Startup
        var endpointConfiguration = new EndpointConfiguration("EndpointName");
        // Apply other necessary endpoint configuration, e.g. transport
        var startableEndpoint = await Endpoint.Create(endpointConfiguration);
        var endpointInstance = await startableEndpoint.Start();

        // Shortcut
        var endpointInstance2 = await Endpoint.Start(endpointConfiguration);
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
#pragma warning restore CS0618 // Type or member is obsolete
}
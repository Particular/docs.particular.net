namespace Snippets6.Lifecycle
{
    using System.Threading.Tasks;
    using NServiceBus;

    class RunWhenEndpointStartsAndStops
    {
        static async Task StartAndStopEndpoint()
        {
            #region lifecycle-EndpointStartAndStop

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("SelfHosted");

            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

            // perform startup logic

            await endpoint.Stop();

            // perform shutdown logic

            #endregion
        }
    }
}

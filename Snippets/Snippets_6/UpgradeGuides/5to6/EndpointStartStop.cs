namespace Snippets6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class EndpointStartStop
    {
        async Task StartEndpoint()
        {
            #region 5to6-endpoint-start-stop
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");

            // Custom code before start
            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
            // Custom code after start

            // Block the process

            // Custom code before stop
            await endpoint.Stop();
            // Custom code after stop
            #endregion
        }
    }
}

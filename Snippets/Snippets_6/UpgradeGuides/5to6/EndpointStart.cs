namespace Snippets6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class EndpointStart
    {
        async Task StartEndpoint()
        {
            #region 5to6-endpoint-start
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("EndpointName");
            //configure the endpoint

            //Start the endpoint
            IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

            //Shut down the endpoint
            await endpoint.Stop();
            #endregion
        }

    }
}

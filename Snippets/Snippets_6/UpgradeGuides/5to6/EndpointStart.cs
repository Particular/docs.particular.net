namespace Snippets6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class EndpointStart
    {
        public async Task StartEndpoint()
        {
            #region 5to6-endpoint-start
            EndpointConfiguration configuration = new EndpointConfiguration();
            //configure the endpoint

            //Start the endpoint
            IEndpointInstance endpoint = await Endpoint.Start(configuration);

            //Shut down the endpoint
            await endpoint.Stop();
            #endregion
        }

    }
}

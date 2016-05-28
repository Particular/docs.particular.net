namespace Core6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class EndpointStart
    {
        async Task StartEndpoint()
        {
            #region 5to6-endpoint-start
            var endpointConfiguration = new EndpointConfiguration("EndpointName");
            //configure the endpoint

            //Start the endpoint
            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            //Shut down the endpoint
            await endpointInstance.Stop()
                .ConfigureAwait(false);
            #endregion
        }
    }
}

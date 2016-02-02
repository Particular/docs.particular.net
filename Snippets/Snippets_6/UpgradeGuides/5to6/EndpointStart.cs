﻿namespace Snippets6.UpgradeGuides._5to6
{
    using System.Threading.Tasks;
    using NServiceBus;

    class EndpointStart
    {
        public async Task StartEndpoint()
        {
            #region 5to6-endpoint-start
            BusConfiguration busConfiguration = new BusConfiguration();
            //configure the bus

            //Start the endpoint
            IEndpointInstance endpoint = await Endpoint.Start(busConfiguration);

            //Shut down the endpoint
            await endpoint.Stop();
            #endregion
        }

    }
}

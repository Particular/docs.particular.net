﻿namespace Snippets6.UpgradeGuides._5to6
{
    using NServiceBus;

    class EndpointStart
    {
        public void StartEndpoint()
        {
            #region 5to6-endpoint-start
            BusConfiguration busConfiguration = new BusConfiguration();
            //configure the bus

            //Start the endpoint
            IStartableBus startable = Bus.Create(busConfiguration);
            IBus endpoint = startable.Start();

            //Shut down the endpoint
            endpoint.Dispose();
            #endregion
        }

    }
}

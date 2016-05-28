namespace Core5.UpgradeGuides._5to6
{
    using NServiceBus;

    class EndpointStart
    {
        void StartEndpoint()
        {
            #region 5to6-endpoint-start
            var busConfiguration = new BusConfiguration();
            //configure the bus

            //Start the endpoint
            var startableBus = Bus.Create(busConfiguration);
            var endpoint = startableBus.Start();

            //Shut down the endpoint
            endpoint.Dispose();
            #endregion
        }

    }
}

namespace Snippets5.UpgradeGuides._5to6
{
    using NServiceBus;

    class EndpointStartStop
    {
        void StartEndpoint()
        {
            #region 5to6-endpoint-start-stop
            BusConfiguration busConfiguration = new BusConfiguration();

            // Custom code before start
            IStartableBus startable = Bus.Create(busConfiguration);
            IBus endpoint = startable.Start();
            // Custom code after start

            // Block the process

            // Custom code before stop
            endpoint.Dispose();
            // Custom code after stop
            #endregion
        }

    }
}

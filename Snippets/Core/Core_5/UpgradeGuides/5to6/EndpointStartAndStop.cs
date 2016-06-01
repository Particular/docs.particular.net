namespace Core5.UpgradeGuides._5to6
{
    using NServiceBus;

    class EndpointStartStop
    {
        void StartEndpoint()
        {
            #region 5to6-endpoint-start-stop
            var busConfiguration = new BusConfiguration();

            // Custom code before start
            var startableBus = Bus.Create(busConfiguration);
            using (var bus = startableBus.Start())
            {
                // Custom code after start

                // Block the process

                // Custom code before stop
            }
            // Custom code after stop
            #endregion
        }

    }
}

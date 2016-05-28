namespace Core5.UpgradeGuides._5to6
{
    using NServiceBus;

    class EndpointNameRequired
    {
        EndpointNameRequired()
        {
            #region 5to6-endpointNameRequired
            var busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("MyEndpointName");
            #endregion
        }

    }

}

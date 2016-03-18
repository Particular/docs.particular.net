namespace Snippets6.UpgradeGuides._5to6
{
    using NServiceBus;
    
    class EndpointNameRequired
    {
        public EndpointNameRequired()
        {
            #region 5to6-endpointNameRequired
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("MyEndpointName");
            #endregion
        }

    }

}

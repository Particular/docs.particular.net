namespace Snippets6.UpgradeGuides._5to6
{
    using NServiceBus;
    
    class EndpointNameRequired
    {
        public void StartEndpoint()
        {
            #region 5to6-endpointNameRequired
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration("MyEndpointName");
            #endregion
        }

    }

}

namespace Snippets6.EndpointName
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region EndpointNameCode

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.EndpointName("MyEndpoint");
        
            #endregion
        }

    }
}

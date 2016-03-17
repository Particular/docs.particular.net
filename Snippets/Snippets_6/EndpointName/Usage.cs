namespace Snippets6.EndpointName
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            #region EndpointNameCode

            endpointConfiguration.EndpointName("MyEndpoint");
        
            #endregion
        }

    }
}

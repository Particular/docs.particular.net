namespace Snippets6.EndpointName
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region EndpointNameCode

            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.EndpointName("MyEndpoint");
        
            #endregion
        }

    }
}

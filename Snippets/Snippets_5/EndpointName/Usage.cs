namespace Snippets5.EndpointName
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region EndpointNameCode

            busConfiguration.EndpointName("MyEndpoint");
        
            #endregion
        }

    }
}

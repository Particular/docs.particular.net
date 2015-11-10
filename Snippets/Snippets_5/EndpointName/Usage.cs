namespace Snippets5.EndpointName
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region EndpointNameCode

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.EndpointName("MyEndpoint");
        
            #endregion
        }

    }
}

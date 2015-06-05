namespace Snippets5.EndpointName
{
    using NServiceBus;

    public class Usage
    {
        public Usage()
        {
            #region EndpointNameFluent

            BusConfiguration busConfiguration = new BusConfiguration();

            busConfiguration.EndpointName("MyEndpoint");
        
            #endregion
        }

    }
}

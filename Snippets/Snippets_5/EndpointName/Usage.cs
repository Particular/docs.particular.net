namespace Snippets5.EndpointName
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region EndpointNameCode

            busConfiguration.EndpointName("MyEndpoint");
        
            #endregion
        }

    }
}

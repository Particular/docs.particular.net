namespace Core5.EndpointName
{
    using NServiceBus;

    class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region EndpointNameCode

            busConfiguration.EndpointName("MyEndpoint");

            #endregion

            #region InputQueueName

            busConfiguration.OverrideLocalAddress("MyEndpoint.Messages");

            #endregion
        }
    }
}

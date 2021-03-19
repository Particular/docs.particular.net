namespace Core8.EndpointName
{
    using NServiceBus;

    class Usage
    {
        void EndpointNameCode()
        {
            #region EndpointNameCode

            var endpointConfiguration = new EndpointConfiguration("MyEndpoint");

            #endregion
        }
        void InputQueueName(EndpointConfiguration endpointConfiguration)
        {
            #region InputQueueName

            endpointConfiguration.OverrideLocalAddress("MyEndpoint.Messages");

            #endregion
        }

        class MyMessage
        {
        }
    }
}

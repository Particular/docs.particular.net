namespace Core8.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        public StaticHeaders(EndpointConfiguration endpointConfiguration)
        {
            #region header-static-endpoint
            endpointConfiguration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");
            #endregion
        }
    }
}
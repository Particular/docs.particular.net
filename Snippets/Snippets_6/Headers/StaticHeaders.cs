namespace Snippets6.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        public StaticHeaders()
        {
            #region header-static-endpoint
            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");
            #endregion
        }
    }
}
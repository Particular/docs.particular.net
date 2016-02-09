namespace Snippets6.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        public StaticHeaders()
        {
            #region header-static-endpoint
            EndpointConfiguration configuration = new EndpointConfiguration();
            configuration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");
            #endregion
        }
    }
}
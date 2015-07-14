namespace Snippets6.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        public StaticHeaders()
        {

            #region header-static-endpoint
            BusConfiguration configuration = new BusConfiguration();
            configuration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");
            #endregion
        }
    }
}
namespace Snippets6.Headers
{
    using NServiceBus;

    class StaticHeaders
    {
        public StaticHeaders()
        {

            #region header-static-endpoint
            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.AddHeaderToAllOutgoingMessages("MyGlobalHeader", "some static value");
            #endregion
        }
    }
}
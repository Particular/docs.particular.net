using NServiceBus;

namespace SqlTransport_6._3
{
    class DefaultCatalog
    {
        void OverwriteDefaultCatalog(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-default-catalog

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.DefaultCatalog("mycatalog");

            #endregion
        }
    }
}

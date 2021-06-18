using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

class MultiSchema
{
    void OverrideDefaultCatalog(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-override-default-catalog

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.DefaultCatalog("mycatalog");

        #endregion
    }

}
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class LegacyMultiInstance
{
#pragma warning disable 0618

    void ConfigureMultiCatalog(EndpointConfiguration endpointConfiguration)
    {
        #region 31to4-multi-catalog

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString("Data Source=SQL; Database=ThisEndpoint; Integrated Security=True");
        transport.UseCatalogForEndpoint("RemoteEndpoint", "RemoteEndpoint");

        #endregion
    }

#pragma warning restore 0618
}
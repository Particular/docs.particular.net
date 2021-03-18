using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

class MultiCatalog
{
    async Task CatalogForQueue(EndpointConfiguration endpointConfiguration, IMessageSession messageSession)
    {
        #region sqlserver-multicatalog-config-for-queue

        var transport = new SqlServerTransport("connectionString");

        transport.SchemaAndCatalog.UseCatalogForQueue(queueName: "myqueue", catalog: "MyCatalog");
        transport.SchemaAndCatalog.UseCatalogForQueue(queueName: "myerror", catalog: "ServiceControl");

        #endregion

        #region sqlserver-multicatalog-config-for-queue-send

        await messageSession.Send("myqueue", new MyMessage());

        #endregion

        #region sqlserver-multicatalog-config-for-queue-error

        endpointConfiguration.SendFailedMessagesTo("myerror");

        #endregion
    }

    void CatalogForEndpoint(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-multicatalog-config-for-endpoint

        var routing = endpointConfiguration.UseTransport(new SqlServerTransport("connectionString"));

        routing.RouteToEndpoint(typeof(MyMessage), "MyEndpoint");
        routing.UseCatalogForEndpoint(endpointName: "MyEndpoint", catalog: "MyCatalog");

        #endregion
    }

    class MyMessage
    {
    }
}
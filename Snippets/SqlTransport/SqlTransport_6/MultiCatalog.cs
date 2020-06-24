using System.Threading.Tasks;
using NServiceBus;

class MultiCatalog
{
    async Task CatalogForQueue(EndpointConfiguration endpointConfiguration, IMessageSession messageSession)
    {
        #region sqlserver-multicatalog-config-for-queue

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

        transport.UseCatalogForQueue(queueName: "myqueue", catalog: "MyCatalog");
        transport.UseCatalogForQueue(queueName: "myerror", catalog: "ServiceControl");

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

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(MyMessage), "MyEndpoint");

        transport.UseCatalogForEndpoint(endpointName: "MyEndpoint", catalog: "MyCatalog");

        #endregion
    }

    class MyMessage
    {
    }
}
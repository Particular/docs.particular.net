using NServiceBus;
using NServiceBus.Transport.SQLServer;

class MultiCatalog
{
    void CatalogForQueue(EndpointConfiguration endpointConfiguration, IMessageSession messageSession)
    {
        #region sqlserver-multicatalog-config-for-queue

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

        transport.UseCatalogForQueue(queueName: "myqueue", catalog: "MyCatalog");
        transport.UseCatalogForQueue(queueName: "error", catalog: "ServiceControl");

        messageSession.Send("myqueue", new MyMessage());

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
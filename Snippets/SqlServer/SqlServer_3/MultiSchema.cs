using NServiceBus;
using NServiceBus.Transport.SQLServer;

class MultiSchema
{
    void NonStandardSchema(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-non-standard-schema

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.DefaultSchema("myschema");

        #endregion
    }

    void OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration, IMessageSession messageSession)
    {
        #region sqlserver-multischema-config-for-queue

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

        transport.UseSchemaForQueue(queueName: "myqueue", schema: "my_schema");
        transport.UseSchemaForQueue(queueName: "error", schema: "error");

        messageSession.Send("myqueue", new MyMessage());

        #endregion
    }

    void ConfigureCustomSchemaForEndpoint(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-multischema-config-for-endpoint

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(MyMessage), "MyEndpoint");

        transport.UseSchemaForEndpoint(endpointName: "MyEndpoint", schema: "my_schema");

        #endregion
    }

    void ConfigureCustomSchemaForEndpointAndQueue(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-multischema-config-for-endpoint-and-queue

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSchemaForEndpoint(endpointName: "sales", schema: "sender");
        transport.UseSchemaForQueue(queueName: "error", schema: "control");

        #endregion
    }

    class MyMessage
    {
    }
}
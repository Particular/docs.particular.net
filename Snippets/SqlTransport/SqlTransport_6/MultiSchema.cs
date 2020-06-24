using System.Threading.Tasks;
using NServiceBus;

class MultiSchema
{
    void NonStandardSchema(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-non-standard-schema

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.DefaultSchema("myschema");

        #endregion
    }

    async Task OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration, IMessageSession messageSession)
    {
        #region sqlserver-multischema-config-for-queue

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();

        transport.UseSchemaForQueue(queueName: "myqueue", schema: "my_schema");
        transport.UseSchemaForQueue(queueName: "myerror", schema: "sc");

        #endregion

        #region sqlserver-multischema-config-for-queue-send

        await messageSession.Send("myqueue", new MyMessage());

        #endregion

        #region sqlserver-multischema-config-for-queue-error

        endpointConfiguration.SendFailedMessagesTo("myerror");

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

    class MyMessage
    {
    }
}
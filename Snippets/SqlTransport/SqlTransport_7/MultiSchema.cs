using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Transport.SqlServer;

class MultiSchema
{
    void NonStandardSchema(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-non-standard-schema

        var transport = new SqlServerTransport("connectionString")
        {
            DefaultSchema = "myschema"
        };

        #endregion
    }

    async Task OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration, IMessageSession messageSession)
    {
        #region sqlserver-multischema-config-for-queue

        var transport = new SqlServerTransport("connectionString");

        transport.SchemaAndCatalog.UseSchemaForQueue(queueName: "myqueue", schema: "my_schema");
        transport.SchemaAndCatalog.UseSchemaForQueue(queueName: "myerror", schema: "sc");

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

        var routing = endpointConfiguration.UseTransport(new SqlServerTransport("connectionString"));

        routing.RouteToEndpoint(typeof(MyMessage), "MyEndpoint");
        routing.UseSchemaForEndpoint(endpointName: "MyEndpoint", schema: "my_schema");

        #endregion
    }

    class MyMessage
    {
    }
}
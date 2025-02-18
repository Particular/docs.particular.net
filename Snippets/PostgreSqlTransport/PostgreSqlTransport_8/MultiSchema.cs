using System.Threading.Tasks;
using NServiceBus;

class MultiSchema
{
    void NonStandardSchema(EndpointConfiguration endpointConfiguration)
    {
        #region postgresql-non-standard-schema

        var transport = new PostgreSqlTransport("connectionString")
        {
            DefaultSchema = "myschema"
        };

        #endregion
    }

    async Task OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration, IMessageSession messageSession)
    {
        #region postgresql-multischema-config-for-queue

        var transport = new PostgreSqlTransport("connectionString");

        transport.Schema.UseSchemaForQueue(queueName: "myqueue", schema: "my_schema");
        transport.Schema.UseSchemaForQueue(queueName: "myerror", schema: "sc");

        #endregion

        #region postgresql-multischema-config-for-queue-send

        await messageSession.Send("myqueue", new MyMessage());

        #endregion

        #region postgresql-multischema-config-for-queue-error

        endpointConfiguration.SendFailedMessagesTo("myerror");

        #endregion

        #region postgresql-multischema-config-for-queue-heartbeats

        endpointConfiguration.SendHeartbeatTo("\"Particular.ServiceControl\"");
        
        #endregion        
    }

    void ConfigureCustomSchemaForEndpoint(EndpointConfiguration endpointConfiguration)
    {
        #region postgresql-multischema-config-for-endpoint

        var routing = endpointConfiguration.UseTransport(new PostgreSqlTransport("connectionString"));

        routing.RouteToEndpoint(typeof(MyMessage), "MyEndpoint");
        routing.UseSchemaForEndpoint(endpointName: "MyEndpoint", schema: "my_schema");

        #endregion
    }

    class MyMessage
    {
    }
}
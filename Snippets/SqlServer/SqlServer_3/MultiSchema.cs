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

    void OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-multischema-config-for-queue

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSchemaForQueue(queueName: "sales", schema: "salesSchema");
        transport.UseSchemaForQueue(queueName: "billing", schema: "[billingSchema]");
        transport.UseSchemaForQueue(queueName: "error", schema: "error");

        #endregion
    }

    void ConfigureCustomSchemaForEndpoint(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-multischema-config-for-endpoint

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSchemaForEndpoint(endpointName: "sales", schema: "sender");
        transport.UseSchemaForEndpoint(endpointName: "billing", schema: "receiver");

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
}
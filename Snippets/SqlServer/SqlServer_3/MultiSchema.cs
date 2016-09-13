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
        transport.UseSchemaForQueue("sales", "salesSchema");
        transport.UseSchemaForQueue("billing", "[billingSchema]");
        transport.UseSchemaForQueue("error", "error");

        #endregion
    }

    void ConfigureCustomSchemaForEndpoint(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-multischema-config-for-endpoint

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSchemaForEndpoint("sales", "sender");
        transport.UseSchemaForEndpoint("billing", "receiver");

        #endregion
    }

    void ConfigureCustomSchemaForEndpointAndQueue(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-multischema-config-for-endpoint-and-queue

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSchemaForEndpoint("sales", "sender");
        transport.UseSchemaForQueue("error", "control");

        #endregion
    }
}
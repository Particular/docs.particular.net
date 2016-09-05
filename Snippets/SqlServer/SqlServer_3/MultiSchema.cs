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
        #region sqlserver-multischema-config-pull

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSchemaForEndpoint("Sales", "salesSchema");
        transport.UseSchemaForEndpoint("Billing", "[billingSchema]");
        transport.UseSchemaForQueue("error", "error");
        
        #endregion
    }
}
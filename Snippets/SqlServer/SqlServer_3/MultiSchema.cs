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
        transport.UseSpecificSchema(queueName =>
            {
                if (queueName == "sales")
                {
                    return "salesSchema";
                }
                if (queueName == "billing")
                {
                    return "[billingSchema]";
                }
                if (queueName == "error")
                {
                    return "error";
                }
                return null;
            });

        #endregion
    }
}
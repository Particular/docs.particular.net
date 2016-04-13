namespace SqlServer_3
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class MultiSchema
    {
        void NonStandardSchema(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-non-standard-schema 3

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .DefaultSchema("myschema");

            #endregion
        }

        void OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-multischema-config-pull

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificSchema(queueName =>
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
}
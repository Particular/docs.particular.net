using NServiceBus;
using NServiceBus.Transports.SQLServer;

class MultiSchema
{
    void NonStandardSchema(BusConfiguration busConfiguration)
    {
        #region sqlserver-non-standard-schema 2.1

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.DefaultSchema("myschema");

        #endregion
    }

    void NonStandardSchemaInConnectionString(BusConfiguration busConfiguration)
    {
        #region sqlserver-non-standard-schema-connString

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString("Data Source=INSTANCE_NAME;Initial Catalog=db; Queue Schema=myschema");

        #endregion
    }

    void ConfigureCustomSchemaForEndpoint(BusConfiguration busConfiguration)
    {
        #region sqlserver-multischema-config-push 2.1

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSpecificConnectionInformation(
            EndpointConnectionInfo.For("sales")
                .UseSchema("sender"),
            EndpointConnectionInfo.For("billing")
                .UseSchema("receiver")
            );

        #endregion
    }

    void OtherEndpointConnectionParamsPull(BusConfiguration busConfiguration)
    {
        #region sqlserver-multischema-config-pull 2.1

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSpecificConnectionInformation(queueName =>
        {
            if (queueName == "sales")
            {
                return ConnectionInfo.Create()
                    .UseSchema("salesSchema");
            }
            if (queueName == "billing")
            {
                return ConnectionInfo.Create()
                    .UseSchema("billingSchema");
            }
            if (queueName == "error")
            {
                return ConnectionInfo.Create()
                    .UseSchema("error");
            }
            return null;
        });

        #endregion
    }
}
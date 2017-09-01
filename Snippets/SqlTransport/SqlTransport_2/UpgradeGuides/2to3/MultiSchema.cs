using NServiceBus;

namespace SqlTransport_2.UpgradeGuides._2to3
{
    using NServiceBus.Transports.SQLServer;

    class MultiSchema
    {
        void NonStandardSchema(BusConfiguration busConfiguration)
        {
            #region 2to3-sqlserver-non-standard-schema

            var transport = busConfiguration.UseTransport<SqlServerTransport>();
            transport.DefaultSchema("myschema");

            #endregion
        }

        void ConfigureCustomSchemaForEndpointAndQueue(BusConfiguration busConfiguration)
        {
            #region 2to3-sqlserver-multischema-config-for-endpoint-and-queue

            var transport = busConfiguration.UseTransport<SqlServerTransport>();
            transport.UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("sales")
                    .UseSchema("sender"),
                EndpointConnectionInfo.For("error")
                    .UseSchema("control")
            );

            #endregion
        }
    }
}
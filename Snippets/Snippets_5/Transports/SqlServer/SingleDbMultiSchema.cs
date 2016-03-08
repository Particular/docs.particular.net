namespace Snippets5.Transports.SqlServer
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    public class SingleDbMultiSchema
    {
        void CurrentEndpointSchema()
        {
            #region sqlserver-singledb-multischema 2.1

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .DefaultSchema("myschema");

            #endregion
        }

        void OtherEndpointConnectionParamsPush()
        {
            #region sqlserver-singledb-multidb-push [2.1,2.0]

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>().UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("AnotherEndpoint")
                    .UseSchema("receiver1"),
                EndpointConnectionInfo.For("YetAnotherEndpoint")
                    .UseSchema("receiver2")
                );

            #endregion
        }

        void OtherEndpointConnectionParamsPull()
        {
            #region sqlserver-singledb-multidb-pull 2.1

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificConnectionInformation(x => x == "AnotherEndpoint"
                    ? ConnectionInfo.Create()
                        .UseSchema("nsb")
                    : null);

            #endregion
        }
    }
}

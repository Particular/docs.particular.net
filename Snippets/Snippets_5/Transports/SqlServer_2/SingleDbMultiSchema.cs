namespace Snippets5.Transports.SqlServer
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    public class SingleDbMultiSchema
    {
        void CurrentEndpointSchema()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region sqlserver-singledb-multischema 2.1

            busConfiguration.UseTransport<SqlServerTransport>()
                .DefaultSchema("myschema");

            #endregion
        }

        void OtherEndpointConnectionParamsPush()
        {
            BusConfiguration busConfiguration = new BusConfiguration();

            #region sqlserver-singledb-multidb-push [2.1,3.0)

            busConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificConnectionInformation(
                    EndpointConnectionInfo.For("AnotherEndpoint")
                        .UseSchema("receiver1"),
                    EndpointConnectionInfo.For("YetAnotherEndpoint")
                        .UseSchema("receiver2")
                );

            #endregion
        }

        void OtherEndpointConnectionParamsPull()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region sqlserver-singledb-multidb-pull 2.1

            busConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificConnectionInformation(x => x == "AnotherEndpoint"
                    ? ConnectionInfo.Create()
                        .UseSchema("nsb")
                    : null);

            #endregion
        }
    }
}

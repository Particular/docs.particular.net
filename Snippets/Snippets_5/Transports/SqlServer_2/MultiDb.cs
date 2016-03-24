namespace Snippets5.Transports.SqlServer
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class MultiDb
    {

        void OtherEndpointConnectionParamsPush(BusConfiguration busConfiguration)
        {
            #region sqlserver-multidb-other-endpoint-connection-push [2.1,3.0]

            busConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificConnectionInformation(
                    EndpointConnectionInfo.For("RemoteEndpoint")
                        .UseSchema("receiver1")
                        .UseConnectionString("SomeConnectionString"),
                    EndpointConnectionInfo.For("AnotherEndpoint")
                        .UseSchema("receiver2")
                        .UseConnectionString("SomeConnectionString")
                );

            #endregion
        }

        void OtherEndpointConnectionParamsPull(BusConfiguration busConfiguration)
        {
            #region sqlserver-multidb-other-endpoint-connection-pull 2.1

            busConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificConnectionInformation(x => x == "RemoteEndpoint"
                    ? ConnectionInfo.Create()
                        .UseConnectionString("Data Source=...")
                        .UseSchema("nsb")
                    : null);

            #endregion
        }

    }
}
using NServiceBus;
using NServiceBus.Transports.SQLServer;

public class MultiDb
{
    void CurrentEndpointSchema()
    {
        BusConfiguration busConfig = new BusConfiguration();

        #region sqlserver-multidb-current-endpoint-schema 2.1

        busConfig.UseTransport<SqlServerTransport>()
            .DefaultSchema("myschema");

        #endregion
    }

    void CurrentEndpointConnectionString()
    {
        BusConfiguration busConfig = new BusConfiguration();

        #region sqlserver-multidb-current-endpoint-connection-string 2

        busConfig.UseTransport<SqlServerTransport>()
            .ConnectionString(@"Data Source=INSTANCE_NAME;Initial Catalog=some_database;Integrated Security=True");

        #endregion
    }

    void OtherEndpointConnectionParamsPush()
    {
        BusConfiguration busConfig = new BusConfiguration();

        #region sqlserver-multidb-other-endpoint-connection-push 2.1

        busConfig.UseTransport<SqlServerTransport>().UseSpecificConnectionInformation(
            EndpointConnectionInfo.For("RemoteEndpoint")
                .UseSchema("receiver1")
                .UseConnectionString("SomeConnectionString"),
            EndpointConnectionInfo.For("AnotherEndpoint")
                .UseSchema("receiver2")
                .UseConnectionString("SomeConnectionString")
            );

        #endregion
    }

    void OtherEndpointConnectionParamsPull()
    {
        BusConfiguration busConfig = new BusConfiguration();

        #region sqlserver-multidb-other-endpoint-connection-pull 2.1

        busConfig.UseTransport<SqlServerTransport>()
            .UseSpecificConnectionInformation(x => x == "RemoteEndpoint"
                ? ConnectionInfo.Create()
                    .UseConnectionString(@"Data Source=...")
                    .UseSchema("nsb")
                : null);

        #endregion
    }
}
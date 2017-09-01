    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

class SqlServer
{
    void MultiInstance(BusConfiguration busConfiguration)
    {
        #region 2to3-sqlserver-multiinstance-upgrade

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        // Option 1
        transport
            .UseSpecificConnectionInformation(
                EndpointConnectionInfo.For("RemoteEndpoint")
                    .UseConnectionString("SomeConnectionString"),
                EndpointConnectionInfo.For("AnotherEndpoint")
                    .UseConnectionString("SomeOtherConnectionString"));

        // Option 2
        transport
            .UseSpecificConnectionInformation(
                connectionInformationProvider: x =>
                {
                    if (x == "RemoteEndpoint")
                    {
                        var connection = ConnectionInfo.Create();
                        return connection.UseConnectionString("SomeConnectionString");
                    }
                    if (x == "AnotherEndpoint")
                    {
                        var connection = ConnectionInfo.Create();
                        return connection.UseConnectionString("SomeOtherConnectionString");
                    }
                    return null;
                });

        #endregion
    }
}
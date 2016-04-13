namespace Snippets5.UpgradeGuides._5to6
{
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class SqlServer
    {
        void MultiInstance(BusConfiguration busConfiguration)
        {
            #region sqlserver-multiinstance-upgrade [2.1,3.0)

            busConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificConnectionInformation(
                    EndpointConnectionInfo.For("RemoteEndpoint")
                        .UseConnectionString("SomeConnectionString"),
                    EndpointConnectionInfo.For("AnotherEndpoint")
                        .UseConnectionString("SomeOtherConnectionString"));

            busConfiguration.UseTransport<SqlServerTransport>()
                .UseSpecificConnectionInformation(x =>
                {
                    if (x == "RemoteEndpoint")
                        return ConnectionInfo.Create()
                            .UseConnectionString("SomeConnectionString");
                    if (x == "AnotherEndpoint")
                        return ConnectionInfo.Create()
                            .UseConnectionString("SomeOtherConnectionString");
                    return null;
                });

            #endregion
        }
    }
}

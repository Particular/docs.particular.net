using System;
using NServiceBus;
using NServiceBus.Transports.SQLServer;

class MultiDb
{

    void OtherEndpointConnectionParamsPush(BusConfiguration busConfiguration)
    {
        #region sqlserver-multidb-other-endpoint-connection-push 2.1

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSpecificConnectionInformation(
            EndpointConnectionInfo.For("RemoteEndpoint")
                .UseSchema("schema1")
                .UseConnectionString("SomeConnectionString"),
            EndpointConnectionInfo.For("AnotherEndpoint")
                .UseSchema("schema2")
                .UseConnectionString("SomeOtherConnectionString")
            );

        #endregion
    }

    void OtherEndpointConnectionParamsPull(BusConfiguration busConfiguration)
    {
        #region sqlserver-multidb-other-endpoint-connection-pull 2.1

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.UseSpecificConnectionInformation(x =>
        {
            if (x == "RemoteEndpoint")
            {
                return ConnectionInfo.Create()
                    .UseConnectionString("SomeConnectionString")
                    .UseSchema("schema1");
            }
            if (x == "AnotherEndpoint")
            {
                return ConnectionInfo.Create()
                    .UseConnectionString("SomeOtherConnectionString")
                    .UseSchema("schema2");
            }
            throw new Exception($"Connection string not found for transport address {x}");
        });

        #endregion
    }
}
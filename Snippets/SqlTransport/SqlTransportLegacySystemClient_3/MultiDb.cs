using System;
using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class MultiDb
{
#pragma warning disable 0618

    void OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration)
    {
        #region sqlserver-multidb-other-endpoint-connection

        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.EnableLegacyMultiInstanceMode(async address =>
        {
            var connectionString = address.Equals("RemoteEndpoint") ? "SomeConnectionString" : "SomeOtherConnectionString";
            var connection = new SqlConnection(connectionString);
            await connection.OpenAsync()
                .ConfigureAwait(false);
            return connection;
        });

        #endregion
    }

#pragma warning restore 0618
}
using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Transport.SQLServer;

class MultiDb
{

    void OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration)
    {
#pragma warning disable 0618

        #region sqlserver-multidb-other-endpoint-connection-pull

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

#pragma warning restore 0618
    }
}

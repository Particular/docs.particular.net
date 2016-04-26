namespace SqlServer_3
{
    using System.Data.SqlClient;
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class MultiDb
    {

        void OtherEndpointConnectionParamsPull(EndpointConfiguration endpointConfiguration)
        {
#pragma warning disable 0618

            #region sqlserver-multidb-other-endpoint-connection-pull

            var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
            transport.EnableLagacyMultiInstanceMode(async address =>
            {
                string connectionString = address.Equals("RemoteEndpoint") ? "SomeConnectionString" : "SomeOtherConnectionString";
                SqlConnection connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return connection;
            });

            #endregion

#pragma warning restore 0618
        }
    }
}
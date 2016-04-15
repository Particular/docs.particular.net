namespace SqlServer_3.UpgradeGuides
{
    using System.Data.SqlClient;
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class SqlServer
    {
        void MultiInstance(EndpointConfiguration endpointConfiguration)
        {
#pragma warning disable 0618
            #region sqlserver-multiinstance-upgrade

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .EnableLagacyMultiInstanceMode(async address =>
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

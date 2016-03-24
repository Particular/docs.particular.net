namespace Snippets5.Transports.SqlServer
{
    using System.Data.SqlClient;
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    class NamedConnectionString
    {
        private string connectionString;

        void ConnectionString(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-config-connectionstring

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString("Data Source=INSTANCE_NAME;Initial Catalog=some_database;Integrated Security=True");

            #endregion
        }
		
        void ConnectionName(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-named-connection-string

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionStringName("MyConnectionString");

            #endregion
        }

        void ConnectionFactory(EndpointConfiguration endpointConfiguration)
        {
            #region sqlserver-custom-connection-factory 3

            endpointConfiguration.UseTransport<SqlServerTransport>()
                .UseCustomSqlConnectionFactory(async () =>
                {
                    connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

                    SqlConnection connection = new SqlConnection(connectionString);
                    await connection.OpenAsync();

                    // perform custom operations

                    return connection;
                });

            #endregion
        }
    }
}
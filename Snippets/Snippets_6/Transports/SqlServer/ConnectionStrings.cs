namespace Snippets5.Transports.SqlServer
{
    using System.Data.SqlClient;
    using NServiceBus;
    using NServiceBus.Transports.SQLServer;

    public class NamedConnectionString
    {
		void ConnectionString()
        {
            #region sqlserver-config-connectionstring 3

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString("Data Source=INSTANCE_NAME;Initial Catalog=some_database;Integrated Security=True");

            #endregion
        }
		
        void ConnectionName()
        {
            #region sqlserver-named-connection-string 3

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionStringName("MyConnectionString");

            #endregion
        }

        void ConnectionFactory()
        {
            #region sqlserver-custom-connection-factory 3

            EndpointConfiguration endpointConfiguration = new EndpointConfiguration();
            endpointConfiguration.UseTransport<SqlServerTransport>()
                .UseCustomSqlConnectionFactory(async () =>
                {
                    SqlConnection connection = new SqlConnection(@"Server=localhost\sqlexpress;Database=nservicebus;Trusted_Connection=True;");

                    await connection.OpenAsync();

                    return connection;
                });

            #endregion
        }
    }
}
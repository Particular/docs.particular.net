namespace Snippets5.Transports.SqlServer
{
    using System.Data.SqlClient;
    using NServiceBus;

    public class NamedConnectionString
    {
        void ConnectionString()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region sqlserver-config-connectionstring 2

            busConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString("Data Source=INSTANCE_NAME;Initial Catalog=some_database;Integrated Security=True");

            #endregion
        }

        void ConnectionName()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region sqlserver-named-connection-string 2

            busConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionStringName("MyConnectionString");

            #endregion
        }

        void ConnectionFactory()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region sqlserver-custom-connection-factory 2

            busConfiguration.UseTransport<SqlServerTransport>()
                .UseCustomSqlConnectionFactory(
                    connectionString => new SqlConnection(@"Server=localhost\sqlexpress;Database=nservicebus;Trusted_Connection=True;"));

            #endregion
        }
    }
}
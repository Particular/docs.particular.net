namespace Snippets5.Transports.SqlServer
{
    using NServiceBus;

    public class NamedConnectionString
    {
		void ConnectionString()
        {
            #region sqlserver-config-connectionstring 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionString("Data Source=INSTANCE_NAME;Initial Catalog=some_database;Integrated Security=True");

            #endregion
        }
        
		void ConnectionName()
        {
            #region sqlserver-named-connection-string 2

            BusConfiguration busConfiguration = new BusConfiguration();
            busConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionStringName("MyConnectionString");

            #endregion
        }
    }
}
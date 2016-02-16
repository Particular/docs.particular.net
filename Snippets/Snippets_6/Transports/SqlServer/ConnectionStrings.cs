namespace Snippets5.Transports.SqlServer
{
    using NServiceBus;

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
    }
}
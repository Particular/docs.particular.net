using NServiceBus;

class NamedConnectionString
{
    void ConnectionString(BusConfiguration busConfiguration)
    {
        #region sqlserver-config-connectionstring

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString("Data Source=INSTANCE_NAME;Initial Catalog=some_database");

        #endregion
    }

    void ConnectionName(BusConfiguration busConfiguration)
    {
        #region sqlserver-named-connection-string

        var transport = busConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionStringName("MyConnectionString");

        #endregion
    }
}
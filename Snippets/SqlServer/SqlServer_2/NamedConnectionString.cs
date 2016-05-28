    using System.Data.SqlClient;
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

        void ConnectionFactory(BusConfiguration busConfiguration)
        {
            #region sqlserver-custom-connection-factory

            var transport = busConfiguration.UseTransport<SqlServerTransport>();
            transport.UseCustomSqlConnectionFactory(
                connectionString =>
                {
                    var newConnection = new SqlConnection(connectionString);
                    newConnection.Open();
                    // custom operations
                    return newConnection;
                });

            #endregion
        }
    }
using System;
using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Timeouts
{
    void Connection(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceTimeoutConnection

        var connection = @"Data Source=.\SqlExpress;Initial Catalog=timeouts;Integrated Security=True";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        //TODO: uncomment after updating dependency to 4.1
        //var timeouts = persistence.TimeoutSettings();
        //timeouts.ConnectionBuilder(
        //    connectionBuilder: () =>
        //    {
        //        return new SqlConnection(connection);
        //    });

        #endregion
    }
}

using System;
using System.Data.SqlClient;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Subscriptions
{
    void Connection(EndpointConfiguration endpointConfiguration)
    {
        #region SqlPersistenceSubscriptionConnection

        var connection = @"Data Source=.\SqlExpress;Initial Catalog=subscriptions;Integrated Security=True";
        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        var subscriptions = persistence.SubscriptionSettings();
        //TODO: uncomment after updating dependency to 4.1
        //subscriptions.ConnectionBuilder(
        //    connectionBuilder: () =>
        //    {
        //        return new SqlConnection(connection);
        //    });

        #endregion
    }
}

namespace MongoTekmaven_9
{
    using NServiceBus;
    using NServiceBus.Persistence.MongoDB;

    public class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region MongoUsage

            endpointConfiguration.UsePersistence<MongoDbPersistence>();

            #endregion
        }
        void ConnectionStringName(EndpointConfiguration endpointConfiguration)
        {
            #region MongoConnectionStringName

            var persistence = endpointConfiguration.UsePersistence<MongoDbPersistence>();
            persistence.SetConnectionStringName("SharedConnectionString");

            #endregion
        }
        void ConnectionString(EndpointConfiguration endpointConfiguration)
        {
            #region MongoConnectionString

            var persistence = endpointConfiguration.UsePersistence<MongoDbPersistence>();
            persistence.SetConnectionString("mongodb://localhost/databaseName");

            #endregion
        }
    }
}

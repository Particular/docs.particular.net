namespace MongoTekmaven_8
{
    using NServiceBus;
    using NServiceBus.Persistence.MongoDB;

    public class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region MongoUsage

            busConfiguration.UsePersistence<MongoDbPersistence>();

            #endregion
        }
        void ConnectionStringName(BusConfiguration busConfiguration)
        {
            #region MongoConnectionStringName

            var persistence = busConfiguration.UsePersistence<MongoDbPersistence>();
            persistence.SetConnectionStringName("SharedConnectionString");

            #endregion
        }
        void ConnectionString(BusConfiguration busConfiguration)
        {
            #region MongoConnectionString

            var persistence = busConfiguration.UsePersistence<MongoDbPersistence>();
            persistence.SetConnectionString("mongodb://localhost/databaseName");

            #endregion
        }
    }
}

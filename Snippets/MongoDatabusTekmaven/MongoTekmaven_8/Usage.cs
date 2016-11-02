namespace MongoTekmaven_8
{
    using NServiceBus;
    using NServiceBus.Persistence.MongoDB;
    using NServiceBus.Persistence.MongoDB.DataBus;

    public class Usage
    {
        Usage(BusConfiguration busConfiguration)
        {
            #region MongoDataBusUsage

            var persistence = busConfiguration.UsePersistence<MongoDbPersistence>();
            persistence.SetConnectionString("mongodb://localhost/databaseName");
            busConfiguration.UseDataBus<MongoDbDataBus>();

            #endregion
        }
    }
}

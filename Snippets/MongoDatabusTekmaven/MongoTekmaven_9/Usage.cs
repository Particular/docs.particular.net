namespace MongoTekmaven_9
{
    using NServiceBus;
    using NServiceBus.Persistence.MongoDB;
    using NServiceBus.Persistence.MongoDB.DataBus;

    public class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDataBusUsage

            var persistence = endpointConfiguration.UsePersistence<MongoDbPersistence>();
            persistence.SetConnectionString("mongodb://localhost/databaseName");
            endpointConfiguration.UseDataBus<MongoDbDataBus>();

            #endregion
        }
    }
}

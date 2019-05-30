namespace MongoDB_2
{
    using MongoDB.Driver;
    using NServiceBus;

    public class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBUsage

            endpointConfiguration.UsePersistence<MongoDBPersistence>();

            #endregion
        }
        void MongoDBClient(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBClient

            var persistence = endpointConfiguration.UsePersistence<MongoDBPersistence>();
            persistence.Client(new MongoClient("SharedMongoUrl"));

            #endregion
        }
        void DatabaseName(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBSetDatabaseName

            var persistence = endpointConfiguration.UsePersistence<MongoDBPersistence>();
            persistence.DatabaseName("DatabaseName");

            #endregion
        }
        void DisableTransactions(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBDisableTransactions

            var persistence = endpointConfiguration.UsePersistence<MongoDBPersistence>();
            persistence.UseTransactions(false);

            #endregion
        }
        void SBMakoCompatibility(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBSBMakoCompatibility

            var persistence = endpointConfiguration.UsePersistence<MongoDBPersistence>();
            var compatibility = persistence.CommunityPersistenceCompatibility();
            compatibility.CollectionNamingScheme(type => type.Name);
            compatibility.VersionFieldName("DocumentVersion");

            #endregion
        }
        void TekmavenCompatibility(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBTekmavenCompatibility

            var persistence = endpointConfiguration.UsePersistence<MongoDBPersistence>();
            var compatibility = persistence.CommunityPersistenceCompatibility();
            compatibility.VersionFieldName("Version");

            #endregion
        }
    }
}

namespace MongoDB_2
{
    using System;
    using MongoDB.Driver;
    using NServiceBus;

    public class Usage
    {
        Usage(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBUsage

            endpointConfiguration.UsePersistence<MongoPersistence>();

            #endregion
        }

        void MongoClient(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBClient

            var persistence = endpointConfiguration.UsePersistence<MongoPersistence>();
            persistence.MongoClient(new MongoClient("SharedMongoUrl"));

            #endregion
        }

        void DatabaseName(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBDatabaseName

            var persistence = endpointConfiguration.UsePersistence<MongoPersistence>();
            persistence.DatabaseName("DatabaseName");

            #endregion
        }

        void UseTransactions(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBDisableTransactions

            var persistence = endpointConfiguration.UsePersistence<MongoPersistence>();
            persistence.UseTransactions(false);

            #endregion
        }

        void TimeToKeepOutboxDeduplicationData(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBOutboxCleanup

            var persistence = endpointConfiguration.UsePersistence<MongoPersistence>();
            persistence.TimeToKeepOutboxDeduplicationData(TimeSpan.FromDays(30));

            #endregion
        }

        void SBMakoCompatibility(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBSBMakoCompatibility

            var persistence = endpointConfiguration.UsePersistence<MongoPersistence>();
            var compatibility = persistence.CommunityPersistenceCompatibility();
            compatibility.CollectionNamingConvention(type => type.Name);
            compatibility.VersionElementName("DocumentVersion");

            #endregion
        }

        void TekmavenCompatibility(EndpointConfiguration endpointConfiguration)
        {
            #region MongoDBTekmavenCompatibility

            var persistence = endpointConfiguration.UsePersistence<MongoPersistence>();
            var compatibility = persistence.CommunityPersistenceCompatibility();
            compatibility.VersionElementName("Version");

            #endregion
        }
    }
}

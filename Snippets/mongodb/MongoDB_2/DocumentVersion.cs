using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB_2
{
    class DocumentVersion
    {
        async Task UpdateWithVersion(IMongoCollection<BsonDocument> collection, string versionFieldName, UpdateDefinitionBuilder<BsonDocument> updateBuilder, int currentVersion, Guid documentId)
        {
            #region MongoDBUpdateWithVersion

            UpdateDefinition<BsonDocument> updateDefinition = updateBuilder.Inc(versionFieldName, 1);

            //Define other update operations on the document

            var modifiedDocument = await collection.FindOneAndUpdateAsync<BsonDocument>(
                filter: document => document["_id"] == documentId && document["_version"] == currentVersion,
                update: updateDefinition,
                options: new FindOneAndUpdateOptions<BsonDocument, BsonDocument> { IsUpsert = false, ReturnDocument = ReturnDocument.After });

            if (modifiedDocument == null)
            {
                //The document was not updated because the version was already incremented.
            }

            #endregion
        }
    }
}

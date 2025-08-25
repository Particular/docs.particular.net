using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

class DocumentVersion
{
    async Task UpdateWithVersion(IMongoCollection<BsonDocument> collection, string versionFieldName, UpdateDefinitionBuilder<BsonDocument> updateBuilder, int currentVersion, Guid documentId)
    {
        #region MongoDBUpdateWithVersion

        UpdateDefinition<BsonDocument> updateDefinition = updateBuilder.Inc(versionFieldName, 1);
        FilterDefinition<BsonDocument> filterDefinition = Builders<BsonDocument>.Filter.Eq("_id", documentId)
            & Builders<BsonDocument>.Filter.Eq(versionFieldName, currentVersion);

        //Define other update operations on the document

        var modifiedDocument = await collection.FindOneAndUpdateAsync(
            filter: filterDefinition,
            update: updateDefinition,
            options: new FindOneAndUpdateOptions<BsonDocument, BsonDocument> { IsUpsert = false, ReturnDocument = ReturnDocument.After });

        if (modifiedDocument == null)
        {
            //The document was not updated because the version was already incremented.
        }

        #endregion
    }
}

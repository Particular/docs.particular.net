MongoDB provides no out-of-the-box concurrency controls. A common pattern for supporting concurrency is using a document version number (`int`) that is used as a filter for update statements:

snippet: MongoDBUpdateWithVersion

By updating the document with a filter specifying the expected current value of the document, no update will be made if another process has incremented the version before the current process is able to. This is the pattern this persister is using for updating Sagas.

This pattern requires an element storing the current version in the `BsonDocument` in the collection. This persister achieves this by adding a version element to the serialized saga data `BsonDocument` when the saga is initially created. When the saga data is later fetched the version element's current value is retrieved from the `BsonDocument` before deserializing to the users saga data type. This current value is stored during the lifetime of the saga message processing and used to create the update filter.

By default the element is named `_version`. For migration scenarios the persistence must be configured with the element name used by the community persistence being migrated from.
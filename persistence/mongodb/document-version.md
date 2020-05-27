---
title: How document versioning works
summary: How the NServiceBus.Storage.MongoDB packages implements concurrency control
component: mongodb
versions: '[2,)'
related:
 - persistence/mongodb
reviewed: 2019-10-24
---

MongoDB provides no out-of-the-box concurrency controls. A common pattern for supporting concurrency is using a document version number (`int`) that is used as a filter for update statements:

snippet: MongoDBUpdateWithVersion

By updating the document with a filter specifying the expected current version value of the document, no update will be made if another process has incremented the version before the current process is able to. This package relies on this pattern to ensure only one process/thread can update the saga at a time.

This pattern requires an element in the `BsonDocument` to store the current version value. Instead of requiring the user provide this as a property of their saga data type, this package uses the MongoDB client's BSON serializer to add a version element to the serialized saga data as it is initially created and stored in the collection. When the saga data serialized `BsonDocument` is later fetched, the version element's current value is retrieved before deserializing to the saga data type. The current value is then retained for the lifetime of saga message processing and is used to create the update filter.

By default, the `BsonDocument` element is named `_version`.
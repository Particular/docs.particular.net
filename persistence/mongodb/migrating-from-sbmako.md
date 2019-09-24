---
title: Migrating from NServiceBus.MongoDB
component: mongodb
versions: '[1,)'
tags:
 - Persistence
related:
 - persistence/mongodb/migrating-from-tekmaven
reviewed: 2019-05-29
---

This package was designed to be fully compatible with the community [`NServiceBus.MongoDB`](https://github.com/sbmako/NServiceBus.MongoDB)) package with some minor configuration.

include: migration-warning

## Configuration

Use the following compatibility API to configure the package to work with existing saga data:

snippet: MongoDBSBMakoCompatibility

The `VersionElementName` value must match the element name used for the `DocumentVersion` property from the community persister.

include: must-apply-conventions-for-version

In addition, the collection naming convention for sagas must be configured to match the one used by `NServiceBus.MongoDB`, `type => type.Name`, as demonstrated in the above snippet.

## Subscriptions

In the sbmako implementation subscriptions are stored in the collection named [Subscription](https://github.com/sbmako/NServiceBus.MongoDB/blob/2ffb1c6f653d7e90b6f476ea07c93d40dc64e31a/src/NServiceBus.MongoDB/SubscriptionPersister/MongoSubscriptionPersister.cs#L60). Each document maps to an event type containing a set of subscribers using the type [Subscriber](https://github.com/Particular/NServiceBus/blob/fb96dcc41c7c4d505a099ff2ac6ca1659d582804/src/NServiceBus.Core/Routing/MessageDrivenSubscriptions/Subscriber.cs#L19-L27) from NServiceBus core.

The following migration script iterates through the documents and insert each subscriber value as a new document.

```javascript
db.subscription.find().forEach(type => {
   type.Subscribers.forEach(subscription => {
       db.eventsubscription.insert({
           MessageTypeName: type._id.TypeName,
           TransportAddress: subscription.TransportAddress,
           Endpoint: subscription.Endpoint
       });
   });
});
```

## Saga data class changes

[Saga data classes](/nservicebus/sagas/#long-running-means-stateful) no longer need to implement [`IHaveDocumentVersion`](https://github.com/sbmako/NServiceBus.MongoDB#sagas). If the saga data class extends [`ContainMongoSagaData`](https://github.com/sbmako/NServiceBus.MongoDB#sagas), it no longer needs to do so. In cases where `IHaveDocumentVersion` has been explicitly implemented by the saga data class, the `DocumentVersion` and `ETag` properties may be safely removed from saga data class implementations.

```diff

- class MySagaData : IContainSagaData, IHaveDocumentVersion
+ class MySagaData : IContainSagaData
{
	public Guid Id { get; set; }
	public string OriginatingMessageId { get; set; }
	public string Originator { get; set; }
-       public int DocumentVersion { get; set; }
-       public int ETag { get; set; }
}

```

If the `ETag` property is not removed, it will no longer be updated by the persister.

### How Document Versioning Works

include: document-version

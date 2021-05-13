---
title: Migrating from NServiceBus.MongoDB
component: mongodb
versions: '[1,)'
related:
 - persistence/mongodb/migrating-from-tekmaven
reviewed: 2021-05-11
---

This package was designed to be fully compatible with the community [`NServiceBus.MongoDB`](https://github.com/sbmako/NServiceBus.MongoDB) package with some minor configuration.

include: migration-warning


## NServiceBus upgrade

`NServiceBus.Storage.MongoDB` is available for NServiceBus Version 7 and later. It is recommended to upgrade endpoints to NServiceBus Version 7 before migrating to the `NServiceBus.Storage.MongoDB` package.


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


### Saga data compatibility mode

Use the following compatibility API to configure the package to work with existing saga data:

snippet: MongoDBSBMakoCompatibility

The `VersionElementName` value must match the element name used for the `DocumentVersion` property from the community persister.

include: must-apply-conventions-for-version

In addition, the collection naming convention for sagas must be configured to match the one used by `NServiceBus.MongoDB`, `type => type.Name`, as demonstrated in the above snippet.


## Subscriptions

Subscriptions are recreated by restarting the subscribing endpoints. Alternatively, existing subscriptions can be migrated to the new data format.


## Migrating subscriptions

In the [Carlos Sandoval](https://github.com/sbmako) implementation subscriptions are stored in the collection named `Subscription`. Each document maps to an event type containing a set of subscribers using the type `Subscriber` from the NServiceBus package.

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

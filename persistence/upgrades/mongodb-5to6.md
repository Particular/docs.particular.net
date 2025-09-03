---
title: MongoDB Persistence Upgrade Version 5 to 6
summary: Migration instructions on how to upgrade to MongoDB Persistence version 6
reviewed: 2025-09-02
component: mongodb
related:
- persistence/mongodb
isUpgradeGuide: true
---

## Minimum required client version

The minimum required MongoDB client version has been raised to [3.4.3](https://www.nuget.org/packages/MongoDB.Driver/3.4.3).

## TimeToKeepOutboxDeduplicationData method moved

The `TimeToKeepOutboxDeduplicationData` method has been moved to the outbox settings:

snippet: MongoDBOutboxCleanup

## Installer support

In previous versions, indexes were created automatically for all storage types, regardless of whether the installers were disabled. Starting with version 6, indexes are created only when the installers are enabled.

This enables the possibility to take full control over the index creation by leveraging the [Ops Manager](https://www.mongodb.com/docs/ops-manager/current/data-explorer/indexes/) or any other preferred deployment mechanism.

When installers are disabled, or when installers are enabled but the persistence installers are disabled with:

snippet: MongoDBDisableInstaller

the persistence assumes that all required infrastructure (including indexes) is already in place. If the necessary indexes are missing, system performance and reliability may be affected.

## Outbox record storage layout changes

Outbox records no longer use the message ID alone as the `_id`. In previous versions, this caused message loss in publish/subscribe scenarios when multiple endpoints shared the same database, since all subscribers wrote to the same outbox record.

Starting with this version, outbox records include a partition key (defaulting to the endpoint name) as part of a structured `_id { pk, mid }`. This prevents the message loss by applying the deuplication per endpoint.

- The implementation is backwards compatible meaning that the existing outbox records remain readable. The persistence performs backward-compatible reads for older entries, but all the new entries use the new structured format.
- Old records will continue to expire according to the configured time to keep deduplication data.
- If desired, the fallback reads can be disabled once no legacy records remain.

This change prepares outbox collections for future scaling scenarios, including sharding.

For the new endpoints, it is recommended to disable the fallback reads.

For the existing endpoints, fallback reads should be enabled until at least the configured time to keep deduplication data has passed. Note that:

- Dispatched records using the old format will expire after the configured retention time.
- Undispatched records do not expire and may remain in the outbox collection longer.

The fallback reads should only be disabled once:

1. All dispatched entries using the old format have expired.
2. No undispatched entries using the old format remain.

One possible approach to get an understanding of the state of outbox collection is to execute the following MongoDB shell queries:

```bash
db.getCollection('outboxrecord').aggregate(
  [
    {
      $match: {
        _id: { $type: 'string' },
        Dispatched: { $ne: null }
      }
    },
    { $count: 'total_count_of_dispatched_records' }
  ],
  { maxTimeMS: 60000, allowDiskUse: true }
);
```

```bash
db.getCollection('outboxrecord').aggregate(
  [
    {
      $match: {
        _id: { $type: 'string' },
        Dispatched: { $eq: null }
      }
    },
    { $count: 'total_count_of_undispatched_records' }
  ],
  { maxTimeMS: 60000, allowDiskUse: true }
);
```
---
title: RavenDB Persistence Upgrade from 4 to 5
summary: Instructions on how to upgrade NServiceBus.RavenDB 4 to 5
component: Raven
related:
 - nservicebus/upgrades/6to7
reviewed: 2019-06-19
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

include: dtc-warning

include: cluster-configuration-warning

As part of this update [NServiceBus Version 7](/nservicebus/upgrades/6to7/) will be required.


## DTC no longer supported

Because the [RavenDB implementation of DTC transactions contains a bug that can lead to data loss](/persistence/ravendb/dtc.md), and because RavenDB 4.0 will remove support for DTC transactions entirely, RavenDB Persistence no longer supports DTC transactions.

If the `TransportTransactionMode` is set to `TransportTransactionMode.TransactionScope`, which enables DTC transactions, RavenDB Persistence will throw the following exception.

> RavenDB Persistence does not support Distributed Transaction Coordinator (DTC) transactions. You must change the TransportTransactionMode in order to continue.

Only [changing the transaction mode](/transports/transactions.md) is not sufficient for most for working systems, as more is required to guarantee consistency between messaging operations and data persistence. For more details, see [DTC not supported for RavenDB Persistence](/persistence/ravendb/dtc.md).


## Non-versioned subscriptions

in Version 4 and below, subscription documents were, by default, stored using a scheme that included the major version of the message assembly, leading to errors in situations where message assembly versions were automatically incremented by the build process.

In Version 5, selection of subscription versioning scheme is now mandatory, so that support for versioned subscriptions can be removed in a later release. If no option is selected, an exception will be thrown:

> RavenDB subscription storage requires using either `persistence.DisableSubscriptionVersioning()` or `persistence.UseLegacyVersionedSubscriptions()` to determine whether legacy versioned subscriptions should be used.

New projects and projects where the subscriptions have been converted to the new format should disable subscription versioning:

snippet: 4to5-DisableSubscriptionVersioning

When converting an older project, the old versioned subscriptions behavior can be maintained until such a time when subscriptions can be converted to the new format:

snippet: 4to5-LegacySubscriptionVersioning

When using this setting, RavenDB Persistence will log a warning at each endpoint startup:

> RavenDB Persistence is using legacy versioned subscription storage. This capability will be removed in NServiceBus.RavenDB 6.0.0. Subscription documents need to be converted to the new unversioned format, after which `persistence.DisableSubscriptionVersioning()` should be used.

For more details, see [Subscription versioning](/persistence/ravendb/subscription-versioning.md?version=raven_5).

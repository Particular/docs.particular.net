---
title: Outbox with RavenDB persistence
component: Raven
reviewed: 2016-08-24
versions: '[2.0,)'
tags:
 - Outbox
related:
- nservicebus/outbox
redirects:
- nservicebus/ravendb/outbox
reviewed: 2017-08-21
---

include: dtc-warning

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store the messages and enable deduplication.


## Extra collections created by the RavenDB Outbox persistence

To keep track of duplicate messages, the RavenDB implementation of Outbox creates a special collection of documents called `OutboxRecord`.


## Deduplication record lifespan

The RavenDB implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the settings dictionary:

snippet: OutboxRavendBTimeToKeep

## Effect on RavenDB DocumentStore

When the Outbox is enabled, the default [transport transaction level](/transports/transactions.md) will be set so that the endpoint does not utilize distributed transactions. As a result of this, the RavenDB persistence will alter the RavenDB DocumentStore so that it also does not enlist in distributed transactions.

Although this will automatically occur when the endpoint initializes, it's still recommended to set disable RavenDB enlistment in DTC transactions in order to be explicit:

snippet: OutboxDisableDocStoreDtc

When the RavenDB Outbox is enabled none of the following properties, on the RavenDB DocumentStore, should be set. They are only used by RavenDB's DTC implementation:

* `ResourceManagerId`
* `TransactionRecoveryStorage`

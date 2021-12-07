---
title: Outbox with RavenDB persistence
component: Raven
reviewed: 2021-12-03
versions: '[2.0,)'
related:
- nservicebus/outbox
redirects:
- nservicebus/ravendb/outbox
---

include: dtc-warning

include: cluster-configuration-info

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store messages and enable deduplication.

## Extra collections created by the RavenDB Outbox persistence

To keep track of duplicate messages, the RavenDB implementation of Outbox creates a special collection of documents called `OutboxRecord`.

## Deduplication record lifespan

partial: cleanup

partial: effect-on-docstore

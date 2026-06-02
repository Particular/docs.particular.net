---
title: Outbox with RavenDB persistence
component: Raven
reviewed: 2026-06-02
versions: '[2.0,)'
related:
- nservicebus/outbox
redirects:
- nservicebus/ravendb/outbox
---

include: cluster-configuration-info

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store messages and enable deduplication.

## Storage format

The persister stores outbox data for all endpoints in a [collection](https://ravendb.net/docs/article-page/7.0/csharp/client-api/faq/what-is-a-collection) called `OutboxRecords`. To separate data for individual endpoints, stored documents will have their endpoint name embedded in the document ID using the following format: `Outbox/{Endpoint-name}/{Message-id}`.

## Deduplication record lifespan

The RavenDB persistence retains deduplication records for 7 days by default and runs the cleanup operation every minute.

These settings can be modified by specifying the desired values in the settings dictionary:

snippet: OutboxRavendBTimeToKeep

Starting with NServiceBus.RavenDB version 7.0, cleanup is disabled by default and it is recommended to rely on [document expiration](https://ravendb.net/docs/article-page/latest/csharp/server/extensions/expiration) instead.

If document expiration cannot be used it is possible to enable the outbox purging by specifying:

snippet: OutboxRavendBEnableCleaner

If document expiration cannot be used, to improve efficiency it is advised to run cleanup on only one endpoint instance per RavenDB database, by explicitely disabling clean up on all other endpoint instances:

snippet: OutboxRavendBDisableCleanup

> [!WARNING]
> If document expiration is not used when operating in [multi-tenant mode](/persistence/ravendb/#multi-tenant-support), cleanup must be handled manually, since NServiceBus is unaware of the databases in use.
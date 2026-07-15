---
title: SQL Server Native Publish Subscribe
summary: Describes the native publish subscribe implementation in the SQL Server transport
reviewed: 2026-07-15
component: SqlTransport
versions: '[7,)'
related:
 - transports/sql/sql-statements
 - transports/sql/connection-settings
 - nservicebus/messaging/publish-subscribe
---

The SQL Server transport implements the publish-subscribe pattern and handles subscription information natively, so a separate persistence is not required. A dedicated subscription routing table, shared by all endpoints, holds subscription information for each event type. When an endpoint subscribes to an event, an entry is created in the subscription routing table. When an endpoint publishes an event, the subscription routing table is queried to find all of the subscribing endpoints.

The transport creates this table at installation time, when installers are enabled. When installers are disabled, the table must be created before the endpoint starts. See [creating table structure in production](/transports/sql/sql-statements.md#installation-creating-table-structure-in-production).

## Configure subscription caching

Subscription information can be cached for a given period of time so that it does not have to be loaded every single time an event is being published. The longer the cache period is, the higher the chance that new subscribers miss some events.

The default behavior is to cache subscription information for 5 seconds. This value is selected to be high enough to prevent excessive database lookups in high throughput scenarios when hundreds of messages are published each second.

If the default value is not suitable for a particular endpoint it can be changed. To configure it, use following API:

snippet: configure-subscription-cache

In systems where events are subscribed and unsubscribed regularly (e.g. desktop applications unsubscribe when shutting down) it makes sense to keep the caching period short or to disable the caching altogether:

snippet: disable-subscription-cache

## Configure subscription table

A single subscription table is used by all endpoints. By default this table is named `SubscriptionRouting` and is created in the `dbo` schema of the catalog specified in the connection string. When `DefaultSchema` or `DefaultCatalog` is configured on the transport, those values are used instead. To change where this table is created and how it is named, use the following API:

snippet: configure-subscription-table

> [!WARNING]
> All endpoints in the system must be configured to use the same subscription table. In a multi-schema or multi-catalog system the subscription table needs to be in a shared schema and catalog.

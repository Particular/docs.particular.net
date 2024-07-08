---
title: PostgreSQL Native Publish Subscribe
summary: Describes the native publish subscribe implementation in the PostgreSQL transport
reviewed: 2024-05-28
component: PostgreSqlTransport
---

The PostgreSQL transport implements the publish-subscribe pattern. The transport creates a dedicated subscription routing table, shared by all endpoints, which holds subscription information for each event type. When an endpoint subscribes to an event, an entry is created in the subscription routing table. When an endpoint publishes an event, the subscription routing table is queried to find all of the subscribing endpoints.


## Configure subscription caching

Subscription information can be cached for a given period of time so that it does not have to be loaded every single time an event is being published. The longer the cache period is, the higher the chance that new subscribers miss some events.

The default behavior is to cache subscription information for 5 seconds. This value is comparable to the average time it takes a subscription message to reach the destination endpoint when using message-driven publish-subscribe (as in versions 4 and below). On the other hand, the value is high enough to prevent excessive database lookups in high throughput scenarios when hundreds of messages are published each second.

If the default value is not suitable for a particular endpoint it can be changed. To configure it, use following API:

snippet: configure-subscription-cache

In systems where events are subscribed and unsubscribed regularly (e.g. desktop applications unsubscribe when shutting down), it makes sense to keep the caching period short or to disable the caching altogether:

snippet: disable-subscription-cache


## Configure subscription table

A single subscription table is used by all endpoints. By default this table will be named `SubscriptionRouting` and be created in the default schema of the catalog specified in the connection string. To change where this table is created and how it is named, use the following API:

snippet: configure-subscription-table

> [!WARNING]
> All endpoints in the system must be configured to use the same subscription table. In a multi-schema system the subscription table needs to be in a shared schema.

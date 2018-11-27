---
title: Subscription Persister
component: SqlPersistence
reviewed: 2018-05-23
redirects:
 - nservicebus/sql-persistence/subscriptions
versions: '[2,)'
---


## Caching

The storage of subscription information is required for [unicast transports](/transports/types.md#unicast-only-transports).

Subscription information can be cached for a given period of time so that it does not have to be loaded every single time an event is being published. The longer the cache period is, the higher the chance that new subscribers miss some events. It happens when a subscription message arrives after the subscription information has been loaded into memory.

Because of that there is no good default value for the subscription caching period. It has to be specified by the user. In systems where subscriptions are static the caching period can be relatively long. To configure it, use following API:

snippet: subscriptions_CacheFor

In systems where events are subscribed and unsubscribed regularly (e.g. desktop applications unsubscribe when shutting down) it makes sense to keep the caching period short or to disable the caching altogether:

snippet: subscriptions_Disable

partial: connection
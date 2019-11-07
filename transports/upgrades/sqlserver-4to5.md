---
title: SQL Server Transport Upgrade Version 4 to 5
reviewed: 2019-11-06
component: SqlTransport
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---


## Native delayed delivery

In version 5, native delayed delivery is always enabled. The configuration APIs for native delayed delivery have moved:

snippet: 4to5-configure-native-delayed-delivery

## Native publish subscribe

SQL Server transport version 5 introduces native support for the publish subscribe pattern. It does so via a dedicated subscription routing table which holds subscription information for each event type. When an endpoint subscribes to an event, an entry is created in the subscription routing table. When an endpoint publishes an event, the subscription routing table is queried to find all of the subscribing endpoints.

Upgrade a single endpoint at a time. As each endpoint is upgraded to version 5, configure it to operate in backwards compatibility mode to enable it to participate in publish subscribe operations with endpoints that are running on version 4.x or below. An endpoint operating in backwards compatibility mode will check the configured persistence and the native subscriptions table for subscribers when publishing a message. The transport will only send a single copy of the message to each subscriber.

snippet: 4to5-enable-message-driven-pub-sub-compatibility-mode

Configuration APIs to register publishers for event types has been moved to the compatibility mode settings object.

snippet: 4to5-configure-message-driven-pub-sub-routing

NOTE: The `routing.DisablePublishing()` API has been deprecated and should be removed. This API was created to allow an endpoint to run without a configured subscription persistence. On version 5 and above, a subscription persistence is not required unless the endpoint runs in backwards compatibility mode.

Once all endpoint have been upgraded to version 5 and deployed, compatibility mode can be disabled on endpoints, one at a time.


### Configure subscription caching

Subscription information can be cached for a given period of time so that it does not have to be loaded every single time an event is being published. The longer the cache period is, the higher the chance that new subscribers miss some events.

Because of that there is no good default value for the subscription caching period. It has to be specified by the user. In systems where subscriptions are static the caching period can be relatively long. To configure it, use following API:

snippet: 4to5-configure-subscription-cache

In systems where events are subscribed and unsubscribed regularly (e.g. desktop applications unsubscribe when shutting down) it makes sense to keep the caching period short or to disable the caching altogether:

snippet: 4to5-disable-subscription-cache

### Configure subscription table

A single subscription table is used by all endpoints. By default this table will be named `[SubscriptionRouting]` and be created in the `[dbo]` schema of the catalog specified in the connection string. To change where this table is created and how it is named, use the following API:

snippet: 4to5-configure-subscription-table

WARNING: All endpoints in the system must be configured to use the same subscription table.

NOTE: If the endpoint is explicitly configured to use a schema, then the schema for the subscription table must also be explicitly set. 


### Operations

The snippet below shows the T-SQL script that creates the subscriptions table:

snippet: 4to5-CreateSubscriptionTableTextSql
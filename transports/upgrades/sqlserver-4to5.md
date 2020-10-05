---
title: SQL Server Transport Upgrade Version 4 to 5
reviewed: 2019-11-06
component: SqlTransport
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

Upgrading from SQL Server transport version 4 to version 5 is a major upgrade and requires careful planning. Read the entire upgrade guide before beginning the upgrade process.


## Native delayed delivery

In version 5, native delayed delivery is always enabled. The configuration APIs for native delayed delivery have moved:

snippet: 4to5-configure-native-delayed-delivery


## Native publish subscribe

SQL Server transport version 5 introduces [native support for the publish subscribe pattern](/transports/sql/native-publish-subscribe.md). Endpoints running on version 5 and above are able to publish and subscribe to events without requiring a separate persistence.

Before they are upgraded, endpoints running on older versions of the transport are not able to aceess the subscription data provided by native publish-subscribe. They will continue to send subscribe and unsubscribe control messages and will manage their own subscription storage, as described in [Message-driven publish-subscribe](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based).

The transport provides a compatibility mode that allows the endpoint to use both forms of publish-subscribe at the same time. When it is enabled and the endpoint publishes an event, the native subscription table and the message-driven subscription persistence are checked for subscriber information. This subscriber information is deduplicated before the event it published, so even if a subscriber appears in both places it will only receive a single copy of each event.


### Upgrading

Upgrade a single endpoint to version 5 at a time. Each upgraded endpoint should be configured to run in backwards compatibility mode and be deployed into production before upgrading the next endpoint. At startup, the upgraded endpoint will add it's subscription information to the native subscriptions table. It will also send subscribe control messages to each of it's configured publishers. 

NOTE: The first endpoint to be deployed to each environment will need to create the shared native subscriptions table. This can be done by [enabling installers on the endpoint](/nservicebus/operations/installers.md) or by using the script found below.

Once all endpoints in the system have been upgraded to version 5, the code that enables compatibility mode can be safely removed from each endpoint. It is recommended to run the entire system in backwards compatibility mode for a day or two before beginning to remove backwards compatibility mode. This allows all of the subscription control messages sent at endpoint startup to reach their destination and be fully processed. After removing backwards compatibility mode from all the endpoints the subscription data managed by the persisters is no longer needed and can be safely removed.


#### Native subscriptions configuration

The native publish-subscribe feature can be configured with a cache duration for subscription information or have the cache explicitly disabled. 

snippet: 4to5-subscription-caching

If the endpoint before the upgrade used [SQL persistence](/persistence/sql/) configure the subscription caching the same way as it was configured in the persistence. If the endpoint used [NHibernate persistence](/persistence/nhibernate/) it is best to rely on default settings of subscription caching.

The native publish-susbcribe feature relies on a subscriptions table shared across all endpoints. The name, schema, and catalog for this table can be configured.

snippet: 4to5-subscription-table

WARNING: To prevent message-loss, all endpoints must be configured to use the same subscriptions table.

If the endpoints use different schemas and/or catalogs, the subscription table name needs to be explicitly set to the same value in each endpoint.

#### Backwards compatibility configuration

snippet: 4to5-enable-message-driven-pub-sub-compatibility-mode

Message-driven publish-subscribe works by having the subscriber send control messages to the publisher to subscribe (or unsubscribe) from a type of event. When the publisher receives one of these subscription related control messages, it updates its private subscription persistence. When a publisher publishes an event, it checks its private subscription storage for a list of subscribers for that event type and sends a copy of the event to each subscriber.

A subscriber endpoint running in backwards compatibility mode will send subscription related control messages when subscribing to or unsubscribing from event types. The feature must be configured to associate each event type with the endpoint that publishes it. The configuration APIs to do this have been moved from the routing component to the compatibility component.

snippet: 4to5-configure-message-driven-pub-sub-routing

NOTE: Once the publishing endpoint has been upgraded, this configuration can optionally be removed. 

A publisher endpoint running backwards compatibility mode will also handle incoming subscription related control messages to update both the native subscription table and the private subscription persistence. Incoming subscription related control messages can be authorized using an API that has moved from the transport component to the compatibility component.

snippet: 4to5-configure-message-driven-pub-sub-auth

WARNING: This API only applies to subscribe messages sent by endpoints which still use message-driven publish-subscribe. Under native subscription management, each endpoint writes it's own subscription data into the shared subscription table directly. 

NOTE: The `routing.DisablePublishing()` API has been deprecated and should be removed. This API was created to allow an endpoint to run without a configured subscription persistence. In version 5 and above, a subscription persistence is not required unless the endpoint runs in backwards compatibility mode.


### Operations

The snippet below shows the T-SQL script that creates the subscriptions table:

snippet: 4to5-CreateSubscriptionTableTextSql

### ServiceControl Transport Adapter

The [Transport Adapter](/servicecontrol/transport-adapter/) needs to be upgraded to version 2.0.1 to work with the SQL Server transport 5.x.

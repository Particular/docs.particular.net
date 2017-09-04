---
title: SQL Server Native Delayed Delivery
summary: Describes the native delayed delivery implementation in the SQL Server transport
reviewed: 2017-06-05
component: SqlTransport
versions: '[3,)'
---

The SQL Server transport can take advantage of native [delayed delivery](/nservicebus/messaging/delayed-delivery.md) without then need to run the [timeout manager](/nservicebus/messaging/timeout-manager.md). Instead, the transport creates infrastructure which can delay messages using native SQL Server transport features. To enable the native delayed delivery use the following API:

snippet: EnableNativeDelayedDelivery

NOTE: In this mode the timeout manager will still be running in order to process all outstanding delayed messages. Refer to the [Disabling the timeout manager](/transports/sql/native-delayed-delivery.md#backwards-compatibility-disabling-the-timeout-manager) section for details on how to disable the timeout manager entirely.


## Configuration

The default values for the settings described in this section are used in snippets. The settings can be fine-tuned to fit a particular system's characteristic, e.g. in case the expired timeout messages need to be picked up more frequently to result in a more precise approximation of expiry time.

For upgrade guidance refer to the [dedicated upgrade guide](/transports/upgrades/sqlserver-3to31.md).

The transport creates an additional table that stores delayed messages. The table name has the format _`endpoint-name.suffix`_, using the suffix specified in the configuration:

snippet: DelayedDeliveryTableSuffix

SQL Server transport polls for expired messages at the specified intervals:

snippet: DelayedDeliveryProcessingInterval

then it picks and dispatches messages in batches of the specified size:

snippet: DelayedDeliveryBatchSize

When the delay time lapses, SQL Server transport moves a batch of messages to the destination queue, i.e. the endpoint's input queue. Note that this means the exact time of delivering delayed message is always approximate.


## Backwards compatibility

When upgrading to a version of the transport that supports native delayed delivery, it is safe to operate a combination of endpoints using native delayed delivery and endpoints using timeout manager at the same time:

 * Endpoints with native delayed delivery can send delayed messages to endpoints using timeout manager. 
 * Endpoints with native delayed delivery can continue to receive delayed messages from endpoints using timeout manager.


### Disabling the timeout manager

To assist with the upgrade process, the timeout manager is still enabled by default. Any delayed messages stored in the endpoint's persistence database before the upgrade will be sent when their timeouts expire. Any delayed messages sent after the upgrade will be sent through the native delayed delivery infrastructure, even though the timeout manager is enabled.

Once an endpoint has no more delayed messages in its persistence database, there is no more need for the timeout manager. It can be disabled by calling:

snippet: DelayedDeliveryDisableTM

At this point, all the `.Timeouts` and `.TimeoutsDispatcher` tables for the endpoint can be deleted from the database. In addition, the endpoint no longer requires timeout persistence, so those storage entities can be removed from the persistence database as well.

NOTE: Newly created endpoints that have never been deployed without native delayed delivery should also disable Timeout Manager from running.

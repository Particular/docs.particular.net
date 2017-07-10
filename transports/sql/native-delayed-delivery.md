---
title: SQL Server Native Delayed Delivery
summary: Describes the native delayed delivery implementation in the SQL Server transport
reviewed: 2017-06-05
component: SqlServer
versions: '[3.1,)'
---

In Versions 3.1 and above, the SQL Server transport can take advantage of native [delayed delivery](/nservicebus/messaging/delayed-delivery.md) without then need to run the [timeout manager](/nservicebus/messaging/timeout-manager.md). Instead, the transport creates infrastructure which can delay messages using native SQL Server transport features. To enable the native delayed delivery use the following API:

snippet: EnableNativeDelayedDelivery

NOTE: In this mode the timeout manager will still be running in order to process all outstanding delayed messages. Refer to the [Disabling the timeout manager](#disabling-the-timeout-manager) section for details on how to disable the timeout manager entirely.

## How it works

The transport creates an additional queue that stores delayed messages. The table name has the format `endpoint-name.suffix`, using the suffix specified in the configuration:

snippet: DelayedDeliveryTableSuffix

The timeouts poller checks for expired messages at specified interval:

snippet: DelayedDeliveryProcessingInterval

The poller picks and dispatches messages in batches of specified size:

snippet: DelayedDeliveryBatchSize

When the delay time lapses, SQL Server transport moves a batch of messages to the input queue. Note that this means the exact time of delivering delayed message is always approximate.


## Backwards compatibility

When upgrading to a version of the transport that supports native delayed delivery, it is safe to operate a combination of endpoints using native delayed delivery and endpoints using persistence-based delayed delivery at the same time:
- Endpoints with native delayed delivery can send delayed messages to endpoints using persistence-based delayed delivery. 
- Endpoints with native delayed delivery can continue to receive delayed messages from endpoints using persistence-based delayed delivery.


### Disabling the timeout manager

To assist with the upgrade process, the timeout manager is still enabled by default. Any delayed messages stored in the endpoint's persistence database before the upgrade will be sent when their timeouts expire. Any delayed messages sent after the upgrade will be sent through the native delayed delivery infrastructure, even though the timeout manager is enabled.

Once an endpoint has no more delayed messages in its persistence database, there is no more need for the timeout manager. It can be disabled by calling:

snippet: DelayedDeliveryDisableTM

At this point, all the `.Timeouts` and `.TimeoutsDispatcher` tables for the endpoint can be deleted from the database. In addition, the endpoint no longer requires timeout persistence, so those storage entities can be removed from the persistence database as well.

NOTE: Newly created endpoints that have never been deployed without native delayed delivery should also disable Timeout Manager from running.

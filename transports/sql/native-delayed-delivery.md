---
title: SQL Server Native Delayed Delivery
summary: Describes the native delayed delivery implementation in the SQL Server transport
reviewed: 2019-03-07
component: SqlTransport
versions: '[3,)'
---

The SQL Server transport can take advantage of native [delayed delivery](/nservicebus/messaging/delayed-delivery.md) without the need to run the [timeout manager](/nservicebus/messaging/timeout-manager.md). Instead, the transport creates infrastructure which can delay messages using native SQL Server transport features.

NOTE: The native delayed delivery of SQL Server transport is available only in endpoints that are not configured as [send-only](/nservicebus/hosting/#self-hosting-send-only-hosting).

partial: enable

## Configuration

The default values for the settings described in this section are used in snippets. The settings can be fine-tuned to fit a particular system's characteristic, e.g. in case the expired timeout messages need to be picked up more frequently to result in a more precise approximation of expiry time.

For upgrade guidance refer to the [dedicated upgrade guide](/transports/upgrades/sqlserver-3to31.md).

The transport creates an additional table that stores delayed messages. The table name has the format _`endpoint-name.suffix`_, using the suffix specified in the configuration:

snippet: DelayedDeliveryTableSuffix

SQL Server transport polls for expired messages every second by default. Polling interval can be configured:

snippet: DelayedDeliveryProcessingInterval

then it picks and dispatches messages in batches of the specified size:

snippet: DelayedDeliveryBatchSize

When the delay time lapses, SQL Server transport moves a batch of messages to the destination queue, i.e. the endpoint's input queue. Note that this means the exact time of delivering delayed message is always approximate.

## Backwards compatibility

When upgrading to a version of the transport that supports native delayed delivery, it is safe to operate a combination of endpoints using native delayed delivery and endpoints using timeout manager at the same time:

* Endpoints with native delayed delivery can send delayed messages to endpoints using timeout manager.
* Endpoints with native delayed delivery can continue to receive delayed messages from endpoints using timeout manager.

partial: timeoutmanager

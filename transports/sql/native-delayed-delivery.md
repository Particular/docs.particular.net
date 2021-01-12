---
title: SQL Server Native Delayed Delivery
summary: Describes the native delayed delivery implementation in the SQL Server transport
reviewed: 2021-01-12
component: SqlTransport
versions: '[3,)'
---

The SQL Server transport can take advantage of native [delayed delivery](/nservicebus/messaging/delayed-delivery.md) without the need to run the [timeout manager](/nservicebus/messaging/timeout-manager.md). Instead, the transport creates dedicated infrastructure which delays messages using native SQL Server transport features.

When a message delay time lapses, SQL Server transport moves a batch of messages to the destination queue. Note that this means the exact time of delivering a delayed message is always an approximation.

NOTE: The native delayed delivery of SQL Server transport is available only in endpoints that are not configured as [send-only](/nservicebus/hosting/#self-hosting-send-only-hosting).

partial: enable

## Configuration

The settings described in this section have default values as shown in the snippets. The settings can be fine-tuned to fit a particular system's needs, e.g. messages are checked for expiration more frequently resulting in more accurate delivery times.

NOTE: Native delayed delivery mechanism has been introduced in version 3 of the transport. Older endpoints that use [Timeout Manager](/nservicebus/messaging/timeout-manager.md) can be migrated to the new mechanism by following the [upgrade guide](/transports/upgrades/sqlserver-3to31.md).

### Table suffix

Delayed messages are stored in a dedicated table named _`endpoint-name.suffix`_. The value of the suffix can be specified in the configuration:

snippet: DelayedDeliveryTableSuffix

### Polling Interval

Messages are checked for expiration every second. The polling interval can be configured using:

snippet: DelayedDeliveryProcessingInterval

### Polling Batch Size

On each query, a batch of messages is picked and dispatched. The maximal size of the batch can be specified with:

snippet: DelayedDeliveryBatchSize

## Backwards compatibility

When upgrading to a version of the transport that supports native delayed delivery, it is safe to run a set of endpoints that include both endpoints using native delayed delivery as well as the timeout manager:

* Endpoints with native delayed delivery can send delayed messages to endpoints using the timeout manager.
* Endpoints with native delayed delivery can continue to receive delayed messages from endpoints using timeout manager.

partial: timeoutmanager

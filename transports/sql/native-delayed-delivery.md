---
title: SQL Server Native Delayed Delivery
summary: Describes the native delayed delivery implementation in the SQL Server transport
reviewed: 2025-05-02
component: SqlTransport
versions: '[3,)'
---

The SQL Server transport supports [delayed delivery](/nservicebus/messaging/delayed-delivery.md) by storing the delayed messages in a dedicated table. When a message delay time lapses, SQL Server transport moves messages to the destination queues in batches.
This means that the exact delivery time for a delayed message is always by approximation.

> [!NOTE]
> The native delayed delivery feature of the SQL Server transport is not available to [send-only](/nservicebus/hosting/#self-hosting-send-only-hosting) endpoints.

## Configuration

The settings described in this section have default values as shown in the snippets. The settings can be fine-tuned to fit a particular system's needs, e.g., messages are checked for expiration more frequently, resulting in more accurate delivery times.

> [!NOTE]
> Native delayed delivery mechanism has been introduced in version 3 of the transport. Older endpoints that use [Timeout Manager](/nservicebus/messaging/timeout-manager.md) can be migrated to the new mechanism by following the [upgrade guide](/transports/upgrades/sqlserver-3to31.md).

### Table suffix

Delayed messages are stored in a dedicated table named _`endpoint-name.suffix`_. The suffix is set to _`Delayed`_ by default, but can be overwritten using:

snippet: DelayedDeliveryTableSuffix

partial: batchprocessing

## Backwards compatibility

When upgrading to a version of the transport that supports native delayed delivery, it is safe to run a set of endpoints that include both endpoints using native delayed delivery as well as the timeout manager:

* Endpoints with native delayed delivery can send delayed messages to endpoints using the timeout manager.
* Endpoints with native delayed delivery can continue to receive delayed messages from endpoints using timeout manager.

partial: timeoutmanager

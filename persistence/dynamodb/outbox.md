---
title: Outbox with DynamoDB persistence
summary: Configure the DynamoDB Outbox table, its required time-to-live attribute, and how long deduplication data is kept.
component: DynamoDB
reviewed: 2026-10-26
related:
- persistence/dynamodb
- nservicebus/outbox
---

## Configure the Outbox table

The Outbox data table can be configured via:

snippet: DynamoOutboxTableConfiguration

> [!NOTE]
> When using the same table for Saga and Outbox data, use the [shared table configuration API](/persistence/dynamodb/#usage-customizing-the-table-used) instead.

The Outbox uses [time-to-live](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/TTL.html) to clean up records of successfully processed messages, so the Outbox table requires a `TableConfiguration.TimeToLiveAttributeName` value. When installers are enabled, NServiceBus creates the table with the correct time-to-live configuration. Otherwise, the table must be [created and configured](/persistence/dynamodb/table-creation.md) before the endpoint starts.

## Outbox cleanup

When the Outbox successfully processes a message, messages with the same message ID will be ignored (deduplicated) for 7 days by default. To customize the deduplication timespan, use the following API:

snippet: DynamoDBOutboxCleanup

> [!NOTE]
> Increasing the deduplication timeframe increases the amount of data stored in the Outbox table.

For the read and write capacity the Outbox consumes per message, see [capacity planning](/persistence/dynamodb/capacity-planning.md#outbox).


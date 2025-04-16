---
title: Outbox (DynamoDB)
summary: How to configure Outbox persistence in DynamoDB
component: DynamoDB
reviewed: 2025-04-08
related:
- persistence/dynamodb
- nservicebus/outbox
---

## Configure the Outbox table

The Outbox data table can be configured via:

snippet: DynamoOutboxTableConfiguration

> [!NOTE]
> When using the same table for Saga and Outbox data, use the [shared table configuration API](/persistence/dynamodb/#usage-customizing-the-table-used) instead.

The Outbox uses [time-to-live](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/TTL.html) to ensure cleanup of successfully processed message, therefore, the Outbox table requires a `TableConfiguration.TimeToLiveAttributeName` value to be set. When installers are enabled, NServiceBus will create the table with the proper time-to-live configuration, otherwise users will need to ensure that the table is properly configured.

## Outbox cleanup

When the Outbox successfully processes a message, messages with the same message ID will be ignored (deduplicated) for 7 days by default. To customize the deduplication timespan, use the following API:

snippet: DynamoDBOutboxCleanup

> [!NOTE]
> Increasing the deduplication timeframe increases the amount of data stored in the Outbox table.


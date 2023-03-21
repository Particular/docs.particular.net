---
title: AWS DynamoDB persistence
summary: How to use NServiceBus with AWS DynamoDB
component: DynamoDB
reviewed: 2023-03-16
related:
---

Uses the [AWS DynamoDB](https://aws.amazon.com/pm/dynamodb/) NoSQL database service for storage.

## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Supported storage types    |Sagas, Outbox
|Transactions               |Using `TransactWriteItems`
|Concurrency control        |Optimistic concurrency, optional pessimistic concurrency
|Scripted deployment        |Not supported
|Installers                 |Table is created by installers.

## Usage

Add a NuGet package reference to `NServiceBus.Persistence.DynamoDB`. Configure the endpoint to use the persistence through the following configuration API:

snippet: DynamoDBUsage

### Customizing the table used

By default, the persister will store records in a table named `NServiceBus.Storage`.

Customize the table name and other table attributes using the following configuration API:

snippet: DynamoDBTableCustomizationShared

This configures the table settings for both Saga and Outbox persistence. Each storage type can be configured individually too:

snippet: DynamoDBTableCustomizationSplit

## Transactions

TODO

## Outbox cleanup

When the outbox is enabled, the deduplication data is kept for seven days by default. To customize this time frame, use the following API:

snippet: DynamoDBOutboxCleanup

Outbox cleanup depends on the DynamoDB time-to-live feature.
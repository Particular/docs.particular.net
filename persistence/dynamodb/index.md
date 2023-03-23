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

By default, the persister will store outbox and saga records in a shared table named `NServiceBus.Storage`.

Customize the table name and other table attributes using the following configuration API:

snippet: DynamoDBTableCustomizationShared

Outbox and Saga data can be stored in separate tables, see the [Saga](/persistence/dynamodb/sagas.md) and [Outbox](/persistence/dynamodb/outbox.md) configuration documentation for further details.

#### Table creation

When [installers](/nservicebus/operations/installers.md) are enabled, NServiceBus will try to create the configured tables if they do not exist. Table creation can explicitly be disabled even with installers remaining enabled using the `DisableTablesCreation`setting:

snippet: DynamoDBDisableTableCreation

## Transactions

Outbox and Saga persistence commit their changes transactionally, using the [DynamoDB TransactWriteItems](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/transactions.html) API. Message handlers can add further operations to this transaction via the synchronized session:

snippet: DynamoDBSynchronizedSession

Transactions can contain a maximum of 100 operations. This limit is shared with operations enlisted by NServiceBus. Each saga will use one operation. Outbox will use `1 + <amount of outgoing messages>` operations.
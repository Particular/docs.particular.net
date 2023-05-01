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

When [installers](/nservicebus/operations/installers.md) are enabled, NServiceBus will try to create the configured tables if they do not exist. Table creation can explicitly be disabled even with installers remaining enabled using the `DisableTablesCreation` setting:

snippet: DynamoDBDisableTableCreation

### Customizing the AmazonDynamoDBClient provider

In cases when the `AmazonDynamoDBClient` is configured and used via dependency injection a custom provider can be implemented

snippet: DynamoDBCustomClientProvider

and registered on the container

snippet: DynamoDBCustomClientProviderRegistration

## Provisioned throughput rate-limiting

When using [provisioned throughput](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ProvisionedThroughput.html) it is possible for the DynamoDB service to rate-limit usage, resulting in "provisioned throughput exceeded" exceptions indicated by the 429 status code.

WARN: When using the Dynamo DB persistence with the outbox enabled, "provisioned throughput exceeded" errors may result in handler re-execution and/or duplicate message dispatches depending on which operation is throttled.

INFO: AWS provides [guidance](https://docs.aws.amazon.com/amazondynamodb/latest/developerguide/ProvisionedThroughput.html#ProvisionedThroughput.Troubleshooting) on how to diagnose and troubleshoot provisioned throughput exceeded exceptions.

The Dynamo DB SDK provides a mechanism to automatically retry operations when rate-limiting occurs. Besides changing the provisioned capacity or switching to autoscaling or the on-demand capacity mode, those settings can be adjusted to help prevent messages from failing during spikes in message volume.

These settings may be set when initializing the `AmazonDynamoDBClient` via the [`AmazonDynamoDBConfig`](https://docs.aws.amazon.com/sdk-for-net/v3/developer-guide/retries-timeouts.html) properties:

snippet: DynamoDBConfigureThrottlingWithClientConfig

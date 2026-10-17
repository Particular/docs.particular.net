---
title: Using Outbox with Cosmos DB
summary: Demonstrates how the Outbox handles duplicate messages using RabbitMQ and Cosmos DB hosted in Docker containers.
reviewed: 2026-10-16
component: Core
related:
- transports/rabbitmq
- persistence/cosmosdb
---

Integrates the [RabbitMQ Server transport](/transports/rabbitmq/) with [Cosmos DB persistence](/persistence/cosmosdb/).

This sample demonstrates how the [Outbox](/nservicebus/outbox/) feature works to ensure the atomic processing of a message in Cosmos DB, ensuring that messages sent and received are kept consistent with any modifications made to business data in a database.

This sample uses [Docker Compose](https://docs.docker.com/compose/) to provide dependencies. It is not necessary to have installed instances of RabbitMQ or Cosmos DB.

downloadbutton

## Prerequisites

The Cosmos DB emulator image used by this sample requires an x64 machine. It [does not support Apple silicon or Microsoft ARM chips](https://learn.microsoft.com/en-us/azure/cosmos-db/emulator#differences-between-the-emulator-and-cloud-service).

Alternatively, the sample can use an Azure Cosmos DB for NoSQL account. Replace the connection string in **Program.cs** with the account's connection string, and use the account's Data Explorer in the Azure portal to inspect the data.

1. Install [Docker](https://www.docker.com/products/docker-desktop/).
2. Install [Docker Compose](https://docs.docker.com/compose/install/).
3. If running Docker on Windows, [set Docker to use Linux containers](https://docs.docker.com/desktop/features/wsl/).
4. In the sample directory, execute the following to set up the RabbitMQ and Cosmos DB emulator instances:

```shell
> docker compose up --detach
```

Wait for the emulator to finish starting, then [import its TLS/SSL certificate](https://learn.microsoft.com/en-us/azure/cosmos-db/how-to-develop-emulator#import-the-emulators-tlsssl-certificate) into the host certificate store before running the sample.

Once complete, the RabbitMQ administration can be reached via [http://localhost:15672/](http://localhost:15672/) with username `rabbitmq` and password `rabbitmq`.

## Running the project

The code consists of a single NServiceBus endpoint project, which simulates receiving duplicated messages (normally received due to at-least-once delivery guarantees of the message broker) and processing them under three different circumstances.

1. Without protection, resulting in duplicated processing of messages.
2. Using the Outbox but with a maximum message concurrency of `1`.
3. Using the Outbox but with multiple messages being processed simultaneously, relying on Cosmos DB rejecting a second Outbox record for the same message to ensure exactly-once successful processing of messages.

Selecting each step is accomplished via commenting and uncommenting code in the **Program.cs** file:

snippet: SampleSteps

Each `ProcessOrder` message is sent twice with the same message ID. The handler stores a `ProcessedOrder` document for every message it processes, as a simulation of writing business data.

### Inspecting the data

The business data and the Outbox records are stored in the `Orders` container of the `Samples.Database.Demo` database. To see them, open the emulator's Data Explorer at [https://localhost:8081/_explorer/index.html](https://localhost:8081/_explorer/index.html), select **Samples.Database.Demo** > **Orders** > **Items** and run these queries:

```sql
SELECT c.OrderId, c.MessageId, c.ProcessedAt FROM c WHERE IS_DEFINED(c.ProcessedAt)
```

```sql
SELECT c.id, c.OrderId, c.Dispatched FROM c WHERE IS_DEFINED(c.Dispatched)
```

The first query returns the business data and the second returns the Outbox records.

Delete the `Samples.Database.Demo` database in Data Explorer between steps to start each step with empty data. The endpoint recreates it on startup.

### Step 1: No protection

First, run the sample as-is. It's easy to see from the console output that each MessageId is processed twice. The endpoint has no way to know that it's handling duplicated messages.

```
Endpoint started. Press Enter to send 5 sets of duplicate messages...

Processing MessageId 7d744980-1f81-4dee-afbe-d963f5394bf5 for OrderId 8e56f749-670c-4dde-880d-7338b9619d06
Processing MessageId 7d744980-1f81-4dee-afbe-d963f5394bf5 for OrderId 8e56f749-670c-4dde-880d-7338b9619d06
Processing MessageId 5b8fc799-7ddb-44f4-a1b5-0c2f134da5a8 for OrderId f9510135-d716-47b2-96ea-80456208fb6f
Processing MessageId 5b8fc799-7ddb-44f4-a1b5-0c2f134da5a8 for OrderId f9510135-d716-47b2-96ea-80456208fb6f
Processing MessageId 4a43319f-a0f2-4773-92e9-b2c82130d867 for OrderId 4736dc4c-8a1b-428d-b4c0-f875dfb91bc2
Processing MessageId 4a43319f-a0f2-4773-92e9-b2c82130d867 for OrderId 4736dc4c-8a1b-428d-b4c0-f875dfb91bc2
Processing MessageId 500df132-bd68-4ffc-91b2-a2999847a417 for OrderId 9beabff4-2fbc-43d7-ac60-0cc17cc01cfd
Processing MessageId 500df132-bd68-4ffc-91b2-a2999847a417 for OrderId 9beabff4-2fbc-43d7-ac60-0cc17cc01cfd
Processing MessageId 707bbf6c-be7a-48bc-94a6-19163f68ec15 for OrderId ca7db7db-2cf5-4380-b32f-abfe6c98c73b
Processing MessageId 707bbf6c-be7a-48bc-94a6-19163f68ec15 for OrderId ca7db7db-2cf5-4380-b32f-abfe6c98c73b
Press any key to exit
```

The business data is duplicated as well. There are two `ProcessedOrder` documents for each order, and there are no Outbox records:

| OrderId | MessageId |
|--------------------------------------|--------------------------------------|
| 8e56f749-670c-4dde-880d-7338b9619d06 | 7d744980-1f81-4dee-afbe-d963f5394bf5 |
| 8e56f749-670c-4dde-880d-7338b9619d06 | 7d744980-1f81-4dee-afbe-d963f5394bf5 |
| f9510135-d716-47b2-96ea-80456208fb6f | 5b8fc799-7ddb-44f4-a1b5-0c2f134da5a8 |
| f9510135-d716-47b2-96ea-80456208fb6f | 5b8fc799-7ddb-44f4-a1b5-0c2f134da5a8 |
| 4736dc4c-8a1b-428d-b4c0-f875dfb91bc2 | 4a43319f-a0f2-4773-92e9-b2c82130d867 |
| 4736dc4c-8a1b-428d-b4c0-f875dfb91bc2 | 4a43319f-a0f2-4773-92e9-b2c82130d867 |
| 9beabff4-2fbc-43d7-ac60-0cc17cc01cfd | 500df132-bd68-4ffc-91b2-a2999847a417 |
| 9beabff4-2fbc-43d7-ac60-0cc17cc01cfd | 500df132-bd68-4ffc-91b2-a2999847a417 |
| ca7db7db-2cf5-4380-b32f-abfe6c98c73b | 707bbf6c-be7a-48bc-94a6-19163f68ec15 |
| ca7db7db-2cf5-4380-b32f-abfe6c98c73b | 707bbf6c-be7a-48bc-94a6-19163f68ec15 |

### Step 2: Outbox, 1 message at a time

Next, uncomment the line in **Program.cs** commented as Step 2, which enables the Outbox feature.

It's clear from the console output that each MessageId is only processed a single time, and the message handler does not execute for the duplicate messages:

```
Endpoint started. Press Enter to send 5 sets of duplicate messages...

Processing MessageId 0c9c6684-bb0c-4c10-ae57-f271f6c97f67 for OrderId a96bf294-1b59-47bc-b405-c55acf6dfab8
Processing MessageId f28f4a67-9c04-4dd9-a596-ed78c15034f7 for OrderId bf3d0499-09aa-44ca-9cbf-8f97c1ababd3
Processing MessageId 2cc7adaa-c165-446a-90f8-304f6aecb289 for OrderId 77250e35-b446-4c4d-a252-a390db176b6c
Processing MessageId 72f5d96a-a2b4-403a-abb0-0322f8c0fde1 for OrderId 44ea4967-b83d-48f9-9f62-70a8521fd818
Processing MessageId 3a10dffe-c332-4e5c-b5f3-7fd4d240f200 for OrderId 0ed22c90-79fb-4fc4-967d-610ed0fa57a3
Press any key to exit
```

The same is true in the database. There is one `ProcessedOrder` document and one Outbox record for each order:

| OrderId | MessageId |
|--------------------------------------|--------------------------------------|
| a96bf294-1b59-47bc-b405-c55acf6dfab8 | 0c9c6684-bb0c-4c10-ae57-f271f6c97f67 |
| bf3d0499-09aa-44ca-9cbf-8f97c1ababd3 | f28f4a67-9c04-4dd9-a596-ed78c15034f7 |
| 77250e35-b446-4c4d-a252-a390db176b6c | 2cc7adaa-c165-446a-90f8-304f6aecb289 |
| 44ea4967-b83d-48f9-9f62-70a8521fd818 | 72f5d96a-a2b4-403a-abb0-0322f8c0fde1 |
| 0ed22c90-79fb-4fc4-967d-610ed0fa57a3 | 3a10dffe-c332-4e5c-b5f3-7fd4d240f200 |

| id | OrderId | Dispatched |
|--------------------------------------|--------------------------------------|------------|
| 0c9c6684-bb0c-4c10-ae57-f271f6c97f67 | a96bf294-1b59-47bc-b405-c55acf6dfab8 | true |
| f28f4a67-9c04-4dd9-a596-ed78c15034f7 | bf3d0499-09aa-44ca-9cbf-8f97c1ababd3 | true |
| 2cc7adaa-c165-446a-90f8-304f6aecb289 | 77250e35-b446-4c4d-a252-a390db176b6c | true |
| 72f5d96a-a2b4-403a-abb0-0322f8c0fde1 | 44ea4967-b83d-48f9-9f62-70a8521fd818 | true |
| 3a10dffe-c332-4e5c-b5f3-7fd4d240f200 | 0ed22c90-79fb-4fc4-967d-610ed0fa57a3 | true |

The Outbox record uses the message ID as its `id` and is stored in the same partition as the business data. Once the outgoing messages are dispatched, `Dispatched` is set to `true` and the record expires after the [deduplication period](/persistence/cosmosdb/#outbox-outbox-cleanup).

### Step 3: Outbox with concurrent processing

Keep the Outbox enabled and comment out `LimitMessageProcessingConcurrencyTo(1)` in **Program.cs**, then run the sample again.

Duplicate messages may now enter the handler concurrently, so the same MessageId may appear more than once in the console. Only one attempt can commit. Cosmos DB rejects the transactional batch of the other attempt because its Outbox record already exists, so none of its business data is stored. The rejected message is retried, finds the Outbox record and is skipped.

```
Endpoint started. Press Enter to send 5 sets of duplicate messages...

Processing MessageId 8f4995da-9790-48cf-a7ce-5f99b578a149 for OrderId f2275578-cabc-4828-a055-155d13a73ea7
Processing MessageId 5aeb3bd1-aac6-4038-b46b-33c4dea4bfb8 for OrderId 11a1a04c-8d54-4645-b4b1-40612180933d
Processing MessageId 139a4273-bd0f-4f8a-8c7e-3afcf60ccfbc for OrderId 379a8e40-502b-43f3-8772-e01052e471d8
Processing MessageId 4bfdd93a-781a-47d2-b921-3fc016268bbb for OrderId ec112068-c48a-410f-a987-58e7c05b7c0c
Processing MessageId ebb2346a-b53f-4200-8c3e-abc06fcbe058 for OrderId bae59c3e-ebde-4a61-a3be-5b9870cc97fa
Processing MessageId 4bfdd93a-781a-47d2-b921-3fc016268bbb for OrderId ec112068-c48a-410f-a987-58e7c05b7c0c
Processing MessageId 139a4273-bd0f-4f8a-8c7e-3afcf60ccfbc for OrderId 379a8e40-502b-43f3-8772-e01052e471d8
info: NServiceBus.ImmediateRetry[0]
      Immediate Retry is going to retry message '4bfdd93a-781a-47d2-b921-3fc016268bbb' because of an exception:
      NServiceBus.Persistence.CosmosDB.TransactionalBatchOperationException: Concurrency conflict.
info: NServiceBus.ImmediateRetry[0]
      Immediate Retry is going to retry message '139a4273-bd0f-4f8a-8c7e-3afcf60ccfbc' because of an exception:
      NServiceBus.Persistence.CosmosDB.TransactionalBatchOperationException: Concurrency conflict.
Press any key to exit
```

The data is the same as in step 2: one `ProcessedOrder` document and one Outbox record for each order, even though the handler ran more than once.

## Code walk-through

In **Program.cs**, an NServiceBus endpoint is created and configured to use the RabbitMQ transport, connecting to the broker instance hosted in Docker:

snippet: ConfigureTransport

Next, [Cosmos DB persistence](/persistence/cosmosdb/) is configured to connect to the local Cosmos DB emulator instance hosted in Docker. The client uses gateway mode, which connects through the emulator's HTTPS port, `8081`. With installers enabled, the database and the `Orders` container are created on startup.

snippet: ConfigurePersistence

The partition key is the order ID, which is part of the message body. It is extracted from the deserialized message:

snippet: PartitionKeyFromMessage

Because the partition key is only available after the message is deserialized, the Outbox check happens in the logical stage of the pipeline instead of the physical stage. See [specifying the partition key using the message contents](/persistence/cosmosdb/transactions.md#specifying-the-partitionkey-to-use-for-the-transaction-using-the-message-contents) for details.

The message carries the order ID:

snippet: Message

**ProcessOrderHandler.cs** contains the message handler:

snippet: Handler

The message handler:

1. Logs the `MessageId` and `OrderId` to the console.
2. Adds a `ProcessedOrder` document to the transactional batch that NServiceBus uses for the Outbox record.

snippet: BusinessData

> [!NOTE]
> Business data must be written using the [transactional batch provided by NServiceBus](/persistence/cosmosdb/transactions.md#sharing-the-transaction), not with a separate `Container` call. The Outbox relies on storing business data and the Outbox record in the same transactional batch to guarantee consistency between messaging operations and database changes within the scope of processing a message. A transactional batch can only span a single partition of a single container, which is why both are stored in the `Orders` container and partitioned by order.

## Cleaning up

Once finished with the sample, the RabbitMQ and local Cosmos DB instances can be cleaned up using:

```shell
> docker compose down
```

---
title: Using Outbox with CosmosDB
summary: Demonstrates how the Outbox handles duplicate messages using RabbitMQ and CosmosDb hosted in Docker containers.
reviewed: 2025-02-24
component: Core
related:
- transports/rabbitmq
- persistence/cosmosdb
---

Integrates the [RabbitMQ Server transport](/transports/rabbitmq/) with [CosmosDb persistence](/persistence/cosmosdb/).

This sample demonstrates how the Outbox feature works to ensure the atomic processing of a message in CosmosDb, ensuring that messages sent and received are kept consistent with any modifications made to business data in a database.

This sample uses [Docker Compose](https://docs.docker.com/compose/) to provide dependencies. It is not necessary to have installed instances of RabbitMQ or SQL Server.

downloadbutton

## Prerequisites

1. Install [Docker](https://www.docker.com/products/docker-desktop).
2. Install [Docker Compose](https://docs.docker.com/compose/install/).
3. If running Docker on Windows, [set Docker to use Linux containers](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers).
4. In the sample directory, execute the following to set up the RabbitMQ and CosmosDb emulator instances:

```shell
> docker compose up --detach
```

Once complete, the RabbitMQ administration can be reached via [http://localhost:15672/](http://localhost:15672/) with username `rabbitmq` and password `rabbitmq`.

## Running the project

The code consists of a single NServiceBus endpoint project, which simulates receiving duplicated messages (normally received due to at-least-once delivery guarantees of the message broker) and processing them under three different circumstances.

1. Without protection, resulting in duplicated processing of messages.
2. Using the Outbox but with a maximum message concurrency of `1`.
3. Using the Outbox but with multiple messages being processed simultaneously, relying on the concurrency exception thrown by the database to ensure exactly-once successful processing of messages.

Selecting each step is accomplished via commenting and uncommenting code in the **Program.cs** file:

snippet: SampleSteps


### Step 1: No protection

First, run the sample as-is. It's easy to see from the console output that each MessageId is processed twice. The endpoint has no way to know that it's handling duplicated messages.

```
Endpoint started. Press Enter to send 5 sets of duplicate messages...

Processing MessageId f4589be7-efaa-4d7b-8fc7-414ac6e5ddfa
Processing MessageId f4589be7-efaa-4d7b-8fc7-414ac6e5ddfa
Processing MessageId 78c4be6d-02f1-4dc6-9aa0-e1298dcb28f9
Processing MessageId 78c4be6d-02f1-4dc6-9aa0-e1298dcb28f9
Processing MessageId 96ed249b-ac80-4eea-be16-eede5f368b09
Processing MessageId 96ed249b-ac80-4eea-be16-eede5f368b09
Processing MessageId 95c6f325-22e2-4832-8a2a-0988ce318f40
Processing MessageId 95c6f325-22e2-4832-8a2a-0988ce318f40
Processing MessageId 1333e38d-b076-41e1-92d6-a2b7a699f62f
Processing MessageId 1333e38d-b076-41e1-92d6-a2b7a699f62f
Press any key to exit
```

The message handler also writes the received MessageId to the **BusinessObject** table in the database, as a simulation of writing business data. To see it, execute `select * from BusinessObject` when connected to the database:

| Id | MessageId |
|----|--------------------------------------|
| 1 | f4589be7-efaa-4d7b-8fc7-414ac6e5ddfa |
| 2 | f4589be7-efaa-4d7b-8fc7-414ac6e5ddfa |
| 3 | 78c4be6d-02f1-4dc6-9aa0-e1298dcb28f9 |
| 4 | 78c4be6d-02f1-4dc6-9aa0-e1298dcb28f9 |
| 5 | 96ed249b-ac80-4eea-be16-eede5f368b09 |
| 6 | 96ed249b-ac80-4eea-be16-eede5f368b09 |
| 7 | 95c6f325-22e2-4832-8a2a-0988ce318f40 |
| 8 | 95c6f325-22e2-4832-8a2a-0988ce318f40 |
| 9 | 1333e38d-b076-41e1-92d6-a2b7a699f62f |
| 10 | 1333e38d-b076-41e1-92d6-a2b7a699f62f |

### Step 2: Outbox, 1 message at a time

Next, uncomment the line in **Program.cs** commented as Step 2, which enables the Outbox feature.

It's clear from the console output that each MessageId is only processed a single time, and the message handler does not execute for the duplicate messages:

```
Endpoint started. Press Enter to send 5 sets of duplicate messages...

Processing MessageId 4d9b207b-f7e3-48f3-a8a9-8bf00afe0bd5
Processing MessageId cfda075b-2ebd-4256-b127-20fbf719873c
Processing MessageId 48cb433c-2cda-4835-a40c-1f06e7c4ace5
Processing MessageId 93132c07-e39d-4eb1-9d34-db4dee493f17
Processing MessageId 247706a4-c231-441c-b5fd-9f2defe6bb6f
Press any key to exit
```

The same is true in CosmosDb:

| Id | MessageId |
|----|--------------------------------------|
| 11 | 4d9b207b-f7e3-48f3-a8a9-8bf00afe0bd5 |
| 12 | cfda075b-2ebd-4256-b127-20fbf719873c |
| 13 | 48cb433c-2cda-4835-a40c-1f06e7c4ace5 |
| 14 | 93132c07-e39d-4eb1-9d34-db4dee493f17 |
| 15 | 247706a4-c231-441c-b5fd-9f2defe6bb6f |


## Code walk-through

In **Program.cs**, an NServiceBus endpoint is created and configured to use the RabbitMQ transport, connecting to the broker instance hosted in Docker:

snippet: ConfigureTransport

Next, [CosmosDb Persistence](/persistence/cosmosdb/) is configured to connect to the local CosmosDb emulator instance hosted in Docker.

snippet: ConfigurePersistence

**MyHandler.cs** contains the message handler.

snippet: Handler

The message handler:

1. Logs the `MessageId` to the console.

> [!NOTE]
> It's absolutely essential that business data is manipulated using the same connection and transaction that NServiceBus opens to manage the Outbox data. The Outbox feature relies on combining the manipulation of Outbox and business data in the same local database transaction to guarantee consistency between messaging operations and database manipulations within the scope of processing a message.


## Cleaning up

Once finished with the sample, the RabbitMQ and local CosmosDb instances can be cleaned up using:

```shell
> docker compose down
```

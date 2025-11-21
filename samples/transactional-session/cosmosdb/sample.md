---
title: Using Transactional Session with CosmosDB
summary: Transactional Session sample that illustrates how to send messages and modify data with CosmosDB in an atomic manner outside message handlers.
reviewed: 2025-11-21
component: TransactionalSession.CosmosDB
related:
- nservicebus/transactional-session
- nservicebus/transactional-session/persistences/cosmosdb
- persistence/cosmosdb
---

This sample uses the [transactional session](/nservicebus/transactional-session) feature with the [CosmosDB persistence](/persistence/cosmosdb) to achieve transactionally consistent database changes and message operations.

downloadbutton

## Overview

The sample contains frontend and backend services that access the same CosmosDB database instance. When the `ITransactionalSession` instance on the front end is committed, an `Order` document is created with the status `Received`, and an `OrderReceived` event is published. The backend service subscribes to the event and loads the order document to update its status to `Accepted`.

```mermaid
sequenceDiagram
    Frontend->>ITransactionalSession: Commit()
    activate ITransactionalSession
    ITransactionalSession->>CosmosDB: Store(order)
    ITransactionalSession->>Queue: Publish(orderReceived)
    ITransactionalSession-->>Frontend: Committed
    deactivate ITransactionalSession
    Queue->>Backend: Process(orderReceived)
    activate Backend
    Backend->>CosmosDB: Update(order)
    Backend-->>Queue: Processed
    deactivate Backend
```

The database and transport operations are executed atomically when the session is committed. If the session is aborted, all database and transport operations are rolled back.

## Prerequisites

The sample is intended to be used with the [CosmosDB emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=ssl-netstd21) to run locally. Alternatively, a connection string to an existing Azure CosmosDB instance can be provided.

## Configuration

### Frontend

The `Frontend` service needs to enable the transactional session feature in the endpoint configuration. The endpoint is also configured to use the CosmosDB persistence with a default container and partition key path:

snippet: cosmos-txsession-frontend-config

> [!NOTE]
> The [outbox feature](/nservicebus/outbox/) must be enabled to achieve atomicity with the transactional session feature.

### Backend

The `Backend` service contains a message handler for the `OrderReceived` event. The `Backend` endpoint also needs to be configured for CosmosDB:

snippet: cosmos-txsession-backend-persistence

Note that the backend service requires a [partition key mapping](/persistence/cosmosdb/transactions.md#specifying-the-partitionkey-to-use-for-the-transaction) configured for the `OrderReceived` to determine what partition is used to access the order.

## Running the sample

Start the `Frontend` and `Backend` endpoints.

On the Frontend application, press <kbd>s</kbd> to create a new `OrderDocument` and publish an `OrderReceived`. Database and queue operations are not executed until the session is committed. Press the <kbd>s</kbd> key several times to enlist multiple document and message operations in the same transaction.

Press <kbd>c</kbd> to commit the transaction. All previously created orders will now show up in the database with a status of `Received`. The `OrderReceived` events will be published to the `Backend` service. Once the `Backend` service receives the event, it will load the order document and update its status to `Accepted`. Once committed, a new session will be opened in the sample.

Press <kbd>a</kbd> to abort the transaction. All database and queue operations will roll back. A new session will open in the sample.

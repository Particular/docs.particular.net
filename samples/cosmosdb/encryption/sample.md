---
title: Cosmos DB Persistence Usage with Encryption
summary: Using Cosmos DB Persistence to store sagas with encryption
reviewed: 2026-08-18
component: CosmosDB
related:
 - nservicebus/sagas
---

This sample shows how to use [Azure Cosmos DB client-side encryption](https://learn.microsoft.com/azure/cosmos-db/how-to-always-encrypted) to encrypt selected saga properties before they leave the process. The sample uses a client/server scenario in which the server stores saga data with Cosmos DB Persistence.

## Prerequisites

Ensure that an instance of the latest [Azure Cosmos DB Emulator](https://learn.microsoft.com/en-us/azure/cosmos-db/local-emulator) is running.

## Sample structure

This sample contains three projects, `SharedMessages`, `Client` and `Server`.

### SharedMessages

The shared message contracts used by all endpoints.

### Client

* Sends the `StartOrder` message to `Server`.
* Receives and handles the `OrderCompleted` event.

### Server

* Receive the `StartOrder` message and initiate an `OrderSaga`.
* `OrderSaga` requests a timeout with an instance of `CompleteOrder` with the saga data.
* `OrderSaga` publishes an `OrderCompleted` event when the `CompleteOrder` timeout fires.

## Running the sample

1. Start the `Server` project and wait for the endpoint to report that it has started.
1. Start the `Client` project.
1. Press <kbd>S</kbd> in the client window to start an order.
1. After the saga timeout expires, verify that the server reports the completed saga and the client receives the `OrderCompleted` event.

## Implementation highlights

### Persistence config

In Program.cs of the Server project, the endpoint is configured to use Cosmos DB Persistence:

snippet: CosmosDBConfig

In the non-transactional mode, the saga id is used as a partition key, and thus, the container needs to use `/id` as the partition key path.

### Encryption setup

The server wraps the regular `CosmosClient` with the Cosmos DB encryption client before passing it to NServiceBus. It then creates a client encryption key and a container with an encryption policy.

snippet: EncryptionPolicy

The policy encrypts `OrderId` and `OrderDescription` using randomized encryption. The `id` and `PartitionKey` properties remain unencrypted because Cosmos DB needs them for point reads and routing. The NServiceBus persistence metadata also remains unencrypted because pessimistic locking updates a nested property within that object.

The key encryption key resolver stores a generated RSA private key under the server output directory. This is suitable only for demonstrating the encryption flow. Production systems should use a secure key store, such as Azure Key Vault, and must retain the key for as long as encrypted data needs to be read.

NOTE: The server deletes and recreates the `Samples.CosmosDB.Encryption` database each time it starts so that the sample always uses the expected encryption key and policy. Starting the server therefore removes all existing sample data.

## Order saga data

The data stored on the saga is defined in the `OrderSagaData.cs` file in the `Server` project:

snippet: sagadata

## Order saga

The handlers for this data are in the `OrderSaga.cs` file in the `Server` project:

snippet: thesaga

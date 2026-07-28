---
title: CosmosDB partition key and saga timeouts
summary: How to resolve partition keys with saga timeouts when using Azure Cosmos DB
component: CosmosDB
reviewed: 2026-07-27
---

When [sending saga timeouts](/nservicebus/sagas/timeouts.md/) the current partition information is not automatically added to the timeout message. When such a saga timeout message is processed, the partition to be used for the CosmosDB cannot be extracted from the incoming saga timeout message.

Ensure that the timeout data state that is passed to `RequestTimeout` contains the partition key information. The [Cosmos DB Persistence Usage with non-default container sample](/samples/cosmosdb/container/) demonstrates this approach.

Support for automatically resolving partition information for saga timeouts is being considered. If this workaround affects the application, [add a thumbs-up or share the use case on issue #720](https://github.com/Particular/NServiceBus.Persistence.CosmosDB/issues/720).

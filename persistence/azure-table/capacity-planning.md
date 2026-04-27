---
title: Azure Table Persistence capacity planning
summary: Guidance for estimating Azure Table Persistence capacity and costs
component: ASP
reviewed: 2026-04-27
related:
- persistence/azure-table
---

Azure Table Persistence supports two underlying storage services, each with a distinct pricing model:

- [Azure Table storage](https://azure.microsoft.com/en-us/services/storage/tables/): Billed per storage transaction. Each read, write, or delete counts as one transaction at a flat rate, billed per 10,000 operations.
- [Azure Cosmos DB for Table](https://docs.microsoft.com/en-us/azure/cosmos-db/table-support/): Billed based on [Request Units](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units) (RUs). Read and write operations are priced differently, and actual RU consumption depends on the size of the stored entity. See the [rate limiting documentation](/persistence/azure-table/#provisioned-throughput-rate-limiting-with-azure-cosmos-db) and the [Microsoft Cosmos DB capacity calculator](https://cosmos.azure.com/capacitycalculator/) for Azure Cosmos DB Table API sizing guidance.

## Operations per message

The tables below show the number of storage operations performed by the persistence for each message processing scenario. Use these counts together with the expected message throughput to estimate the total daily transactions or the required provisioned RUs.

> [!NOTE]
> [Entity group transactions](https://learn.microsoft.com/en-us/rest/api/storageservices/performing-entity-group-transactions) (batch commits) are billed as one transaction per entity in the batch, not as a single transaction for the entire batch.

### Without Outbox

| Incoming message scenario | Read operations | Write operations | Total per message |
|:--------------------------|:---------------:|:----------------:|:-----------------:|
| No Saga                   | 0               | 0                | **0**             |
| Saga (new)                | 1               | 1                | **2**             |
| Saga (update)             | 1               | 1                | **2**             |
| Saga (complete)           | 1               | 1                | **2**             |

- Read: One `GetEntity` call to load the saga.
- Write: One entity group transaction containing the saga insert, update, or delete.

### With Outbox

| Incoming message scenario | Read operations | Write operations | Total per message |
|:--------------------------|:---------------:|:----------------:|:-----------------:|
| No Saga                   | 1               | 2                | **3**             |
| Saga (new)                | 2               | 3                | **5**             |
| Saga (update)             | 2               | 3                | **5**             |
| Saga (complete)           | 2               | 3                | **5**             |

- Reads: One `GetEntity` for the outbox deduplication check, and one for the saga load.
- Writes: The outbox record insert and saga operation are committed together in one entity group transaction (counted as two individual write transactions). After outgoing messages are dispatched, a separate write updates the outbox record to mark it as dispatched.

> [!NOTE]
> All write types — inserts, updates, and deletes — count equally as write transactions. In the saga complete + outbox scenario, the three write transactions are: the outbox record insert, the saga delete (both in the same entity group transaction), and the outbox set-as-dispatched update.

## Item size estimation

The following sizes can be used as a starting point for storage capacity estimates. Actual sizes depend on the saga data model and the size of outgoing message bodies and headers.

| Record type                       | Estimated base size |
|:----------------------------------|:---------------------------------------------------------:|
| Outbox record (pending dispatch)  | ~300 bytes + outgoing message bodies and headers          |
| Outbox record (dispatched).       | ~200 bytes (operations payload is cleared after dispatch) |
| Saga record                       | ~300 bytes + saga data                                    |

For Azure Cosmos DB Table API, item size directly affects RU consumption. See [request unit considerations](https://learn.microsoft.com/en-us/azure/cosmos-db/request-units#request-unit-considerations) for details.

## Outbox storage growth

Unlike Azure Cosmos DB, Azure Table storage has no native time-to-live (TTL) mechanism. Dispatched outbox records are updated to clear the operations payload but are **not automatically deleted** from the table.

The number of outbox records retained at steady state is proportional to message throughput and the [outbox deduplication period](/nservicebus/outbox/#outbox-expiration-duration):

```text
Retained outbox records ≈ Message throughput (per second) × Deduplication period (seconds)
```

With the default 7-day deduplication period (604,800 seconds), an endpoint processing 10 messages/second accumulates approximately 6 million outbox records. A cleanup process should be scheduled to delete outbox records older than the configured deduplication period.

## Monitoring

### Azure Table storage

[Azure Monitor Storage Metrics](https://learn.microsoft.com/en-us/azure/storage/common/monitor-storage-reference) provides per-table transaction counts observable from the Azure portal. Use these metrics to validate estimated operation counts against real traffic and to detect unexpected growth.

### Azure Cosmos DB Table API

The [Azure Cosmos DB Diagnostic Settings](https://learn.microsoft.com/en-us/azure/cosmos-db/monitor-resource-logs) can route diagnostic logs to an Azure Log Analytics workspace, where they can be queried to observe RU consumption per operation. The [Cosmos DB capacity calculator](https://cosmos.azure.com/capacitycalculator/) can also be used with the operation counts from the tables above to estimate the required provisioned throughput.

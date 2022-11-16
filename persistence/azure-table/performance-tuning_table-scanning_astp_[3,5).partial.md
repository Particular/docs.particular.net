## Opting out of the compatibility mode

By default the persister runs with the compatibility mode enabled. The consequences of that is for sagas that are correlated by the correlation property value an additional lookup by the secondary index property might happen for sagas that use a secondary index. It is possible to disable the compatibility mode by calling
[`DisableSecondaryKeyLookupForSagasCorrelatedByProperties`](/persistence/azure-table/configuration.md#saga-configuration).

When targeting Azure Cosmos DB Table API, the saga persister secondary lookups might fail due to an empty row key. In such a case, the persister automatically falls back to do a second retrieve attempt assuming the `RowKey` equal to the `PartitionKey`. It is possible to opt-out and always assume `RowKey = PartitionKey` by specifying [`AssumeSecondaryKeyUsesANonEmptyRowKeySetToThePartitionKey`](/persistence/azure-table/configuration.md#saga-configuration)

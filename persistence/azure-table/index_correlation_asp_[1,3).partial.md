### Saga correlation

NOTE: In Versions 6 and above of NServiceBus, all correlated properties are [unique by default](/nservicebus/upgrades/5to6/handlers-and-sagas.md#saga-api-changes-unique-attribute-no-longer-needed) so there is no longer a configuration setting.

Azure Storage Persistence only supports a single correlation property.

To ensure that only one saga can be created for a given correlation property value, secondary indexes are used. Entities for the secondary index are stored in the same table as a saga. When a saga is completed the secondary index entity is removed as well. It's possible, but highly unlikely, that the saga's completion can leave an orphaned secondary index record. This does not impact the behavior of the persistence as it can detect orphaned records, but may leave a dangling entity in a table with a following `WARN` entry in logs: `Removal of the secondary index entry for the following saga failed: {sagaId}`.

If migrating from Version 6.2.3 or below without applying [saga deduplication](/persistence/upgrades/asp-saga-deduplication.md) a `DuplicatedSagaFoundException` can be thrown when when creating secondary index entities. The exception message will include all the information to track down the error for example:

```
Sagas of type MySaga with the following identifiers 'GUID1', 'GUID2' are considered duplicates because of the violation of the Unique property CorrelationId.
```

Consolidate the [upgrade guide]((/persistence/upgrades/asp-saga-deduplication.md) for more instructions.

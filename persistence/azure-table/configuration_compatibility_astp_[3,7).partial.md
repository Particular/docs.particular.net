### Saga compatibility configuration

snippet: AzurePersistenceSagasCompatibility

The following settings are available for changing the behavior of saga persistence compatibility section:

 * `DisableSecondaryKeyLookupForSagasCorrelatedByProperties`: By default, the persistence operates in compatibility mode and tries to find sagas by using the secondary index property. If no more sagas are using the secondary index property `NServiceBus_2ndIndexKey`, the lookup by the secondary key can be disabled.
 * `AssumeSecondaryKeyUsesANonEmptyRowKeySetToThePartitionKey`: Sagas that have been stored with a secondary index used an empty RowKey on the secondary index entry. By enabling this setting the secondary key lookups will assume that the RowKey equals the PartitionKey, which is crucial for Azure Cosmos DB Table API usage.
 * `AllowSecondaryKeyLookupToFallbackToFullTableScan`: Opt-in to full table scanning for sagas that have been stored with version 1.4 or earlier when running in compatibility mode.

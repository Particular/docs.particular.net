#### Saga compatibility configuration

snippet: AzurePersistenceSagasCompatibility

The following settings are available for changing the behavior of saga persistence compatibility section:

 * `EnableSecondaryKeyLookupForSagasCorrelatedByProperties`: To enable the compatibility mode and force the persister to find sagas by the secondary index property. This is only useful while there are still sagas that use the secondary index property `NServiceBus_2ndIndexKey`.
 * `AssumeSecondaryKeyUsesANonEmptyRowKeySetToThePartitionKey`: Sagas that have been stored with a secondary index used an empty RowKey on the secondary index entry. By enabling this setting the secondary key lookups will assume that the RowKey equals the PartitionKey, which is crucial for Azure Cosmos DB Table API usage.
 * `AllowSecondaryKeyLookupToFallbackToFullTableScan`: Opt-in to full table scanning for sagas that have been stored with version 1.4 or earlier when running in compatibility mode.

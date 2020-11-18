For sagas, subscriptions and for timeouts:

snippet: AzurePersistenceSubscriptionsAllConnectionsCustomization

### Saga configuration

snippet: AzurePersistenceSagasCustomization

The following settings are available for changing the behavior of saga persistence section:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing saga information.
 * `DisableSecondaryKeyLookupForSagasCorrelatedByProperties`: By default the persistence operates in migration mode to remain backwards compatible and tries to find sagas by using the secondary index property. If all sagas have been migrated, the lookup by secondary key can be disabled.
 * `AssumeSecondaryKeyUsesANonEmptyRowKeySetToThePartitionKey`: Sagas that have been stored with a secondary index used an empty RowKey on the secondary index entry. By enabling this setting the secondary key lookups will assume that the RowKey equals the PartitionKey.
 * `AllowSecondaryKeyLookupToFallbackToFullTableScan`: Opt-in to full table scanning for sagas that have been stored with version 1.4 or earlier when running in migration mode.

### Subscription configuration

snippet: AzurePersistenceSubscriptionsCustomization

The following settings are available for changing the behavior of subscription persistence:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing subscription information.

TO COMPLETE

### Customizing the Client provider

### Table name settings

The default table name will be used for Sagas, Outbox and Subscription storage and can be set as follows:

snippet: SetDefaultTable

#### Configuring the table name

When the default table is not set, the table information needs to be provided as part of the message handling pipeline.

A behavior at the level of the`ITransportReceiveContext`:

snippet: CustomTableNameUsingITransportReceiveContextBehavior

A behavior at the level of the `IIncomingLogicalMessageContext` can be used as well:

snippet: CustomTableNameUsingIIncomingLogicalMessageContextBehavior


For sagas and subscriptions:

snippet: AzurePersistenceAllConnectionsCustomization

### Saga configuration

snippet: AzurePersistenceSagasCustomization

The following settings are available for changing the behavior of saga persistence section:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing saga information.
 * `UseCloudTableClient`: Allows to set a fully pre-configured Cloud Table client instead of using a connection string.

#### Saga compatibility configuration

snippet: AzurePersistenceSagasCompatibility

The following settings are available for changing the behavior of saga persistence compatibility section:

 * `DisableSecondaryKeyLookupForSagasCorrelatedByProperties`: By default the persistence operates in compatibility mode and tries to find sagas by using the secondary index property. If no more sagas are using the secondary index property `NServiceBus_2ndIndexKey`, the lookup by secondary key can be disabled.
 * `AssumeSecondaryKeyUsesANonEmptyRowKeySetToThePartitionKey`: Sagas that have been stored with a secondary index used an empty RowKey on the secondary index entry. By enabling this setting the secondary key lookups will assume that the RowKey equals the PartitionKey, which is crucial for Azure Cosmos DB Table API usage.
 * `AllowSecondaryKeyLookupToFallbackToFullTableScan`: Opt-in to full table scanning for sagas that have been stored with version 1.4 or earlier when running in compatibility mode.

### Subscription configuration

snippet: AzurePersistenceSubscriptionsCustomization

The following settings are available for changing the behavior of subscription persistence:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing subscription information.
 * `UseCloudTableClient`: Allows to set a fully pre-configured Cloud Table client instead of using a connection string.

### Configuring a Cloud Table Client Provider

A fully preconfigured CloudTableClient can be registered in the container through a custom provider.

Create a customer provider:

snippet: CustomClientProvider

Then register the provider in the container:

snippet: CustomClientProviderRegistration

### Table name configuration and creation

The default table name will be used for Sagas, Outbox and Subscription storage and can be set as follows:

snippet: SetDefaultTable

#### Configuring the table name

To provide a table at runtime or override the default table, the table information needs to be set as part of the message handling pipeline.

A behavior at the stage of the`ITransportReceiveContext`:

snippet: CustomTableNameUsingITransportReceiveContextBehavior

A behavior at the stage of the `IIncomingLogicalMessageContext` can be used as well:

snippet: CustomTableNameUsingIIncomingLogicalMessageContextBehavior

#### Enabling automatic table creation

To enable table creation on endpoint start or during runtime, the `EnableInstallers` API needs to be called on the endpoint configuration.

Note that when the default table is set, the table will be created on endpoint-start. When the table information is provided as part of the message handling pipeline, the tables will be created at runtime.

snippet: EnableInstallersConfiguration

#### Opting out from table creating when installers are enabled

In case installers are enabled, but there's a need to opt out from creating the tables, the `DisableTableCreation`-API may be used:

snippet: EnableInstallersConfigurationOptingOutFromTableCreation

### Partitioning and compatibility mode helpers

During a given message handling pipeline, multiple data storage operations may occur. To commit them atomically, in a single transaction, they must share a partition key. In conversations involving a saga, the saga ID is a good candidate for a partition key. Unfortunately, saga IDs are determined late in the message handling process and are not exposed to user code.

This makes it difficult to:

* Enable transactional saga data storage for existing sagas that were stored with the previous version of the persister.
* Allow other data operations to participate in the saga data transaction.

To support the above scenarios, `IProvidePartitionKeyFromSagaId` may be injected into behaviors at the logical pipeline stage:

snippet: BehaviorUsingIProvidePartitionKeyFromSagaId

`IProvidePartitionKeyFromSagaId` does the folllowing:

* Sets the partition key on the `IIncomingLogicalMessageContext` based on the following algorithm:
  * Use the saga ID header value if present. Otherwise:
  * When compatibility mode is enabled, and the correlation property is not `SagaCorrelationProperty.None`, look up the saga ID either using the secondary index if present, or by table scanning the saga data if that is [enabled](/persistence/azure-table/configuration.md#saga-configuration). Otherwise:
  * Calculate the saga ID based on the specified correlation property.
* If the table in which all data, including saga data, will be stored is not [already set](/persistence/azure-table/configuration.md#table-name-configuration-and-creation), set it using the saga data name as the table name.

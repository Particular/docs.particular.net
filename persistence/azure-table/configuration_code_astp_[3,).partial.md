For sagas and subscriptions:

snippet: AzurePersistenceAllConnectionsCustomization

### Saga configuration

snippet: AzurePersistenceSagasCustomization

The following settings are available for changing the behavior of saga persistence section:

 * `ConnectionString`: Sets the connection string for the storage account to be used for storing saga information.
 * `UseCloudTableClient`: Allows to set a fully pre-configured Cloud Table client instead of using a connection string.
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

### Partitioning and compatibility helpers

To atomically store data within the same transaction, all data written as part of the same message handling pipeline needs to share the same partition key.
When storing sagas, saga IDs are by default deterministically derived from the saga data: the correlation property name and the correlation property value. Therefore, in conversations concerning a saga as part of the message handling pipeline, this deterministically derived ID is a perfect fit to be used as a partition key. Unfortunately, the saga ID assignment happens quite late during the message handling process and is not exposed to the user code.

This makes it difficult to:

* Support transactional processing for existing sagas that were stored with the previous version of the persister and may have in-flight messages
* Expose the deterministic saga ID early enough to enable business operations to participate in the transaction before the saga has been created

To support the above scenarios the `IProvidePartitionKeyFromSagaId` interface implementation can be injected into behaviors in the logical pipeline stage.

snippet: BehaviorUsingIProvidePartitionKeyFromSagaId

This component enables users to:

* Derive the table in which business- and saga data will be stored by looking for the `TableInformation` or falling back to the default table
* Extract the saga ID header and populate that header value with the partition key for previously created sagas
  * When the header is not available and the compatibility mode is enabled, do the necessary secondary index lookups. Depending on the configuration, fallback to table scanning. Then populate the found saga ID as the partition key
  * When no header information is available and the compatibility mode is disabled, precompute the deterministic saga ID based on the provided correlation property information and populate the precomputed value as the partition key

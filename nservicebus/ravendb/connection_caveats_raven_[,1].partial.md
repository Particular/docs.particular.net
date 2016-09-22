## Conventions

For all NServiceBus 3.x and 4.x versions, RavenDB is the default mechanism for NServiceBus to persist its timeout, saga and subscription data.

Configuring NServiceBus to use RavenDB for persistence can be accomplished by calling `Configure.RavenPersistence()`, however as this is the default configuration this call is not required.

RavenDB persistence for NServiceBus 3 to 4 uses these conventions:

 * If no master node is configured it assumes that a RavenDB server is running at `http://localhost:8080`, the default URL for RavenDB.
 * If a master node is configured, the URL is: `http://{masternode}/:8080`.
 * If a connection string named "NServiceBus/Persistence" is found, the value of the `connectionString` attribute is used.

If NServiceBus detects that any RavenDB related storage is used for sagas, subscriptions, timeouts, etc., it will automatically configure it. There is no need to explicitly configure RavenDB unless it is necessary to override the defaults.


### Using the NServiceBus IDocumentStore for business data is not possible

The RavenDB client is merged and internalized into the NServiceBus assemblies. To use the Raven `IDocumentStore` for business data, reference the Raven client and set up a custom `IDocumentStore`.

NOTE: In NServiceBus 4.x RavenDB is not ILMerged any more. It is embedded instead, using [https://github.com/Fody/Costura\#readme](https://github.com/Fody/Costura#readme).

The embedding enables client updates (but may require binding redirects). It also allows passing a custom instance of `DocumentStore`, thereby providing full configuration flexibility.


### Overriding the defaults

In some situations the default behavior might not be desired:

 * When using RavenDB for business data as it may be necessary to share the connection string. Use the `Configure.RavenPersistence(string connectionStringName)` signature to tell NServiceBus to connect to the server specified in that string. The default connection string for RavenDB is `RavenDB`.
 * To control the database name in code, instead of via the configuration, use the `Configure.RavenPersistence(string connectionStringName, string databaseName)` signature. This can be useful in a multitenant scenario.
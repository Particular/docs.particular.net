The SQL Server transport reads the default catalog from the `Initial catalog` or `Database` properties of the connection string. The following API can be used to override the default catalog for an endpoint when [routing](/nservicebus/messaging/routing.md) is used to find a destination queue table for a message:

snippet: sqlserver-multicatalog-config-for-endpoint

There are several cases when routing is not used and the transport needs specific configuration to find out the catalog for a queue table:

 * [Error queue](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address)
 * [Audit queue](/nservicebus/operations/auditing.md#configuring-auditing)
 * [ServiceControl queue](/monitoring/heartbeats/install-plugin.md)
 * [Overriding the default routing mechanism](/nservicebus/messaging/send-a-message.md#overriding-the-default-routing)
 * Replies to endpoints using SQL Server transport version 2 and earlier

Use the following API to configure the schema for a queue:

snippet: sqlserver-multicatalog-config-for-queue

The configuration above is applicable when sending to a queue or when a queue is passed in the configuration:

snippet: sqlserver-multicatalog-config-for-queue-send

snippet: sqlserver-multicatalog-config-for-queue-error

The entire algorithm for calculating the catalog is the following:

 * If the catalog is configured for a given queue via `UseCatalogForQueue`, the configured value is used.
 * If [logical routing](/nservicebus/messaging/routing.md#command-routing) is used and the catalog is configured for a given endpoint via `UseCatalogForEndpoint`, the configured value is used.
 * If the destination address contains a catalog, the catalog from the address is used.
 * Otherwise the catalog configured as `Initial catalog` or `Database` in the connection string is used.

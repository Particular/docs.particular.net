## Remote processor 

The transactional session supports configuring a remote endpoint as the processing endpoint. The key use case for this is to allow the transactional session to be used in [send-only](/nservicebus/hosting/#self-hosting-send-only-hosting) endpoints.

snippet: configure-remote-processor

> [!WARN]
> Depending on the persister used additional configuration might be needed for the processor endpoint to share the same outbox storage. See below for more details.

### Shared outbox storage

The processor endpoint must use the same outbox storage as the transactional session endpoint.

#### SQL Persistence

Configure the processor to [use the same table prefix](/persistence/sql/install.md#table-prefix) as the transactional session endpoint.

#### CosmosDB

Make sure that the processor [uses the same database name](/persistence/cosmosdb/#usage-customizing-the-database-used) 

If a [default container is configured](/persistence/cosmosdb/#usage-customizing-the-container-used) it must also be configured by the transactional session endpoint.

### RavenDB

Make sure that the processor endpoint:

- [Uses the same database name](/persistence/ravendb/connection.md#database-used) 
- Is [configured use the same endpoint name](/persistence/ravendb/outbox.md#overriding-endpoint-name) as the endpoint it's processing on behalf of.

### Azure Table

TODO

### NHibernate

TODO

### MongoDB

TODOD

### Outbox cleanup

For persisters where [Outbox cleanup](/nservicebus/outbox/#outbox-expiration-duration) is performed by the endpoint instances, only the remote processing endpoint will have the cleanup enabled to prevent concurrent cleanup from happening.

Applies to:

- SQL Persistence

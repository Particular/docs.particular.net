## Remote processor 

The transactional session supports configuring a remote endpoint as the processing endpoint. The key use case for this is to allow the transactional session to be used in [send-only](/nservicebus/hosting/#self-hosting-send-only-hosting) endpoints.

snippet: configure-remote-processor

### Shared outbox storage

The processor endpoint must use the same outbox storage as the transactional session endpoint.

#### SQL Persistence

Configure the processor to [use the same table prefix](/persistence/sql/install.md#table-prefix) as the transactional session endpoint.

### Outbox cleanup

For persisters where [Outbox cleanup](/nservicebus/outbox/#outbox-expiration-duration) is performed by the endpoint only the remote processing endpoint will have the cleanup disabled to prevent concurrent cleanup from happening.

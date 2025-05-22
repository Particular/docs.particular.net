## Send only 

If used in a [send-only](/nservicebus/hosting/#self-hosting-send-only-hosting) endpoint the transactional session must be configured with a remote endpoint that will manage the outbox on behalf of the endpoint. This processor endpoint must have both the outbox and the transactional session enabled.

snippet: configure-remote-processor

> [!WARN]
> Both the send only endpoint and the processor endpoint must be connected to the same database. See [documentation for the individual persisters](/persistence/) for more details.

### Outbox cleanup

For persisters where [Outbox cleanup](/nservicebus/outbox/#outbox-expiration-duration) is performed by the endpoint instances, only the remote processing endpoint will have the cleanup enabled to prevent concurrent cleanup from happening.

Applies to:

- SQL Persistence
- NHibernate

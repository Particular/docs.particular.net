## Send only 

If used in a [send-only](/nservicebus/hosting/#self-hosting-send-only-hosting) endpoint, the transactional session must be configured with a remote endpoint(processor endpoint) that will manage the outbox on behalf of the send-only endpoint.

snippet: configure-remote-processor

> [!WARNING]
> Both the send only endpoint and the processor endpoint must be connected to the same database. See [documentation for the individual persisters](/persistence/) for more details.

> [!NOTE]
> If migrating to this mode from an endpoint couldn't be made send only due to the previous limitations of the transactional session, see the migration guidance below.

### Outbox cleanup

For persisters where [Outbox cleanup](/nservicebus/outbox/#outbox-expiration-duration) is performed by the endpoint instances, only the remote processing endpoint will have the cleanup enabled to prevent concurrent cleanup from happening.

Applies to:

- SQL Persistence
- NHibernate

### Migration to send only endpoint mode

For endpoints that couldn't be send only due the limitations of the transactional session use the following procedure to migrate your endpoint.

#### Preparation

1. Ensure that no message handlers exists in the endpoint
1. Deploy a new processor endpoint
1. Stop the endpoint and ensure that all messages in the input queue is consumed

#### Configuration

1. Make the endpoint send only
1. Configure the endpoint to use the new processor endpoint as shown above
1. Start the endpoint

> [!NOTE]
> The send only endpoint will now use the outbox of the new processor endpoint. Since [message deduplication is not possible when using the transactional session](https://github.com/Particular/NServiceBus.TransactionalSession/issues/97) no data migration is needed

#### Cleanup

1. Remove the no longer used input queue of the send only endpoint
1. As needed remove any database artefacts (tables, documents, etc) related to the send only endpoint.  See [documentation for the individual persisters](/persistence/) for more details.

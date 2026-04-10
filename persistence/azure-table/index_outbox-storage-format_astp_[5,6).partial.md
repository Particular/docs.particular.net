From version 5.0.4, the row key will include the endpoint name to distinguish it from other endpoints processing the same message.

> [!WARNING]
> In versions prior to 5.0.4, when the default partition key is not explicitly set, Outbox rows are not separated by endpoint name. As a result, multiple logical endpoints cannot share the same table since [message identities are not unique across endpoints from a processing perspective](/nservicebus/outbox/#message-identity). To avoid conflicts, either assign each endpoint to a separate table or [override the partition key](transactions.md).
> 
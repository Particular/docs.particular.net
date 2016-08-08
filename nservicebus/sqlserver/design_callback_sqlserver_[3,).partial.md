### Callback queues

If queue individualization is enabled for [callback](/nservicebus/messaging/handling-responses-on-the-client-side.md#message-routing) support, SQL Server transport is using an additional table for the endpoint-specific queue. The name of the table is derived from the endpoint name and the instance ID provided by the user.
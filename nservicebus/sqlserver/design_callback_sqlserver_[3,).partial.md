### Callback queues

If queue individualization is enabled for [callbacks](/nservicebus/messaging/callbacks.md#message-routing), then SQL Server transport is using an additional table for the instance-specific queue. The name of the table is derived from the endpoint name and the instance ID provided by the user.

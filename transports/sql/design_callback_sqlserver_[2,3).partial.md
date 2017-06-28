### Callback queues

If [Callbacks](/nservicebus/messaging/callbacks.md#message-routing) support is not explicitly disabled, SQL Server transport is using an additional table for the endpoint-specific queue. The name of the table is derived from the endpoint name and the host name of the machine running the endpoint program.

Callbacks can be disabled using code API:

snippet: sqlserver-config-disable-secondaries

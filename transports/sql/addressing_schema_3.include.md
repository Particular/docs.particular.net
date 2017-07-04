The SQL Server transport needs to know what schema to use for a queue table when sending messages. The following API can be used to specify the schema for an endpoint when [routing](/nservicebus/messaging/routing.md) is used to find a destination queue table for a message:

snippet: sqlserver-multischema-config-for-endpoint

There are several cases when routing is not used and the transport needs specific configuration to find out the schema for a specific queue table:

  - [Error queue](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address)
  - [Audit queue](/nservicebus/operations/auditing.md#configuring-auditing)
  - [ServiceControl queue](/servicecontrol/plugins/heartbeat.md#configuration-servicecontrol-queue)
  - [Overriding the default routing mechanism](/nservicebus/messaging/send-a-message.md#overriding-the-default-routing)
  - Replies to endpoints using SQL Server transport Version 2 and below

Use the following API to configure the schema for a specific queue:

snippet: sqlserver-multischema-config-for-queue

The entire algorithm for calculating the schema is the following:

 * If schema is configured for a given queue via `UseSchemaForQueue`, the configured value is used.
 * If [logical routing](/nservicebus/messaging/routing.md#command-routing) is used and schema is configured for a given endpoint via `UseSchemaForEndpoint`, the configured schema is used.
 * If destination address contains schema, the schema from address is used.
 * If default schema is configured via `DefaultSchema`, the configured value is used.
 * Otherwise `dbo` is used as a default schema.
---
title: Addressing
summary: How PostgreSQL addresses queues
reviewed: 2024-05-24
component: PostgreSqlTransport
---

## Format

The canonical form of a PostgreSQL transport address is a schema-qualified quoted identifier:

```text
""table"".""schema""
```

Only the table name is mandatory. An address containing only a table name is a valid address, e.g. `MyTable`.

## Resolution

The address is resolved into a fully-qualified table name that includes the table name and its schema.

### Schema

The transport needs to determine which schema to use for a queue table when sending messages. The following API is used to set the schema for an endpoint when [routing](/nservicebus/messaging/routing.md) is used to determine the destination queue for a message:

snippet: postgresql-multischema-config-for-endpoint

There are several cases when routing is not used and the transport requires specific configuration to determine the schema for a queue table:

- [Error queue](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address)
- [Audit queue](/nservicebus/operations/auditing.md#configuring-auditing)
- ServiceControl plugin queues
  - [Heartbeats plugin](/monitoring/heartbeats/install-plugin.md)
  - [Custom Checks plugin](/monitoring/custom-checks/install-plugin.md)
- [Overriding the default routing mechanism](/nservicebus/messaging/send-a-message.md#overriding-the-default-routing)

The following API is used to set the schema for a queue:

snippet: postgresql-multischema-config-for-queue

The specified schema is used both when sending to a specific queue, and when a queue is set in endpoint configuration:

snippet: postgresql-multischema-config-for-queue-send

snippet: postgresql-multischema-config-for-queue-error

The following algorithm is used to determine the schema:

- If the schema is set for a given queue using `UseSchemaForQueue`, that schema is used.
- If [logical routing](/nservicebus/messaging/routing.md#command-routing) is used and the schema is set for a given endpoint using `UseSchemaForEndpoint`, that schema is used.
- If the destination address contains a schema, that schema is used.
- If a default schema is set using `DefaultSchema`, that schema is used.
- Otherwise, the default schema `public` is used.

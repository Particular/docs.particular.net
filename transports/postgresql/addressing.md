---
title: Addressing
summary: How PostgreSQL addresses queues
reviewed: 2024-05-24
component: PostgreSqlTransport
---

## Format

The PostgreSQL Transport address canonical form is a schema-qualified quoted identifier:

```
""table"".""schema""
```

## Resolution

The address is resolved into a fully-qualified table name that includes the table name and its schema. In the address, the table name is the only mandatory part. An address containing only a table name is a valid address, e.g. `MyTable`.

### Schema

The PostgreSQL transport needs to know what schema to use for a queue table when sending messages. The following API can be used to specify the schema for an endpoint when [routing](/nservicebus/messaging/routing.md) is used to find a destination queue table for a message:

snippet: postgresql-multischema-config-for-endpoint

There are several cases when routing is not used and the transport needs specific configuration to find out the schema for a queue table:

- [Error queue](/nservicebus/recoverability/configure-error-handling.md#configure-the-error-queue-address)
- [Audit queue](/nservicebus/operations/auditing.md#configuring-auditing)
- ServiceControl plugin queues
  - [Heartbeats plugin](/monitoring/heartbeats/install-plugin.md)
  - [Custom Checks plugin](/monitoring/custom-checks/install-plugin.md)
- [Overriding the default routing mechanism](/nservicebus/messaging/send-a-message.md#overriding-the-default-routing)

Use the following API to configure the schema for a queue:

snippet: postgresql-multischema-config-for-queue

The configuration above is applicable when sending to a queue or when a queue is passed in the configuration:

snippet: postgresql-multischema-config-for-queue-send

snippet: postgresql-multischema-config-for-queue-error

The entire algorithm for calculating the schema is the following:

* If the schema is configured for a given queue via `UseSchemaForQueue`, the configured value is used.
* If [logical routing](/nservicebus/messaging/routing.md#command-routing) is used and schema is configured for a given endpoint via `UseSchemaForEndpoint`, the configured schema is used.
* If destination address contains a schema, the schema from address is used.
* If default schema is configured via `DefaultSchema`, the configured value is used.
* Otherwise, `public` is used as a default schema.


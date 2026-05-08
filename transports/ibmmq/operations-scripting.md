---
title: IBM MQ Transport Scripting
summary: Command-line tool and scripts for managing IBM MQ transport infrastructure
reviewed: 2026-02-19
component: IBMMQ
related:
 - nservicebus/operations
---

The IBM MQ transport includes a command-line tool for creating and managing transport infrastructure (queues, topics, and subscriptions) without writing code.

## Command-line tool

The `NServiceBus.Transport.IBMMQ.CommandLine` package provides the `ibmmq-transport` CLI tool for managing IBM MQ resources.

### Installation

Install the tool globally:

```bash
dotnet tool install -g NServiceBus.Transport.IBMMQ.CommandLine
```

### Connection options

All commands accept the following connection options. These can also be provided via environment variables:

|Option|Environment variable|Default|
|:---|---|---|
|`--host`|`IBMMQ_HOST`|`localhost`|
|`--port`|`IBMMQ_PORT`|`1414`|
|`--channel`|`IBMMQ_CHANNEL`|`DEV.ADMIN.SVRCONN`|
|`--queue-manager`|`IBMMQ_QUEUE_MANAGER`|(empty)|
|`--user`|`IBMMQ_USER`|(none)|
|`--password`|`IBMMQ_PASSWORD`|(none)|

### Create endpoint infrastructure

Creates the input queue for an endpoint:

```bash
ibmmq-transport endpoint create <name> \
    --host mq-server.example.com \
    --queue-manager QM1
```

### Create a queue

Creates a local queue with a configurable maximum depth:

```bash
ibmmq-transport queue create <name> --max-depth 5000
```

### Delete a queue

Deletes a local queue:

```bash
ibmmq-transport queue delete <name>
```

> [!WARNING]
> Deleting a queue permanently removes it and any messages it contains.

### Subscribe an endpoint to an event

Creates the topic and durable subscription needed for an endpoint to receive a published event type:

```bash
ibmmq-transport endpoint subscribe <name> <event-type> \
    --topic-prefix DEV
```

For example:

```bash
ibmmq-transport endpoint subscribe OrderService \
    "MyCompany.Events.OrderPlaced" \
    --topic-prefix PROD
```

This command:

1. Creates a topic object named `PROD.MYCOMPANY.EVENTS.ORDERPLACED` (if it does not exist).
2. Creates a durable subscription linking the topic to the `OrderService` input queue.

#### Polymorphic subscriptions

To subscribe to a base class or interface and all its concrete implementations, provide the `--assembly` option with the path to the assembly containing the event types:

```bash
ibmmq-transport endpoint subscribe OrderService \
    "MyCompany.Events.IOrderEvent" \
    --topic-prefix PROD \
    --assembly /path/to/MyCompany.Events.dll
```

The CLI loads the assembly, discovers all types that derive from or implement the specified event type, and creates a subscription for each.

Without `--assembly`, only a single subscription for the exact type name is created.

### Unsubscribe an endpoint from an event

Removes the durable subscription for an event type:

```bash
ibmmq-transport endpoint unsubscribe <name> <event-type> \
    --topic-prefix DEV
```

The `--assembly` option can also be used with `unsubscribe` to remove subscriptions for all derived types at once. Given the following event hierarchy:

```csharp
public interface IOrderEvent { }
public class OrderPlaced : IOrderEvent { }
public class OrderBilled : IOrderEvent { }
public class OrderCancelled : IOrderEvent { }
public class PriorityOrderPlaced : OrderPlaced { }
```

Unsubscribing from `OrderPlaced` with the `--assembly` option:

```bash
ibmmq-transport endpoint unsubscribe OrderService \
    "MyCompany.Events.OrderPlaced" \
    --topic-prefix PROD \
    --assembly /path/to/MyCompany.Events.dll
```

This removes subscriptions for `OrderPlaced` **and** `PriorityOrderPlaced` (its derived type), but leaves subscriptions for `OrderBilled`, `OrderCancelled`, and `IOrderEvent` intact.

> [!WARNING]
> Using `--assembly` to unsubscribe from a type in the middle of a hierarchy removes subscriptions for that type and **all types below it**. This can silently stop event delivery if other parts of the system still expect those events to arrive. Prefer unsubscribing from each concrete event type individually:
>
> ```bash
> ibmmq-transport endpoint unsubscribe OrderService \
>     "MyCompany.Events.OrderPlaced" --topic-prefix PROD
> ibmmq-transport endpoint unsubscribe OrderService \
>     "MyCompany.Events.PriorityOrderPlaced" --topic-prefix PROD
> ```
>
> This makes each removal explicit and avoids accidentally unsubscribing from derived types that are still needed.

### Using environment variables

For repeated use, connection details can be provided via environment variables:

```bash
export IBMMQ_HOST=mq-server.example.com
export IBMMQ_PORT=1414
export IBMMQ_QUEUE_MANAGER=QM1
export IBMMQ_USER=admin
export IBMMQ_PASSWORD=passw0rd

ibmmq-transport endpoint create OrderService
ibmmq-transport endpoint subscribe OrderService "MyCompany.Events.OrderPlaced"
```

## IBM MQ native administration

For operations not covered by the CLI tool, use native IBM MQ administration commands.

### Using runmqsc

```
# Create a local queue
DEFINE QLOCAL(ORDERSERVICE) MAXDEPTH(5000) DEFPSIST(YES)

# Display queue depth
DISPLAY QLOCAL(ORDERSERVICE) CURDEPTH

# Display active connections
DISPLAY CONN(*) APPLTAG

# Create a topic
DEFINE TOPIC(DEV.ORDERPLACED) TOPICSTR('dev/mycompany.events.orderplaced/')
```

### Using PCF commands

The transport uses PCF (Programmable Command Format) commands internally for queue and topic creation. The same approach can be used in deployment scripts:

- `MQCMD_CREATE_Q` - Create a local queue
- `MQCMD_CREATE_TOPIC` - Create a topic object
- `MQCMD_DELETE_SUBSCRIPTION` - Delete a durable subscription

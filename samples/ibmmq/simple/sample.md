---
title: IBM MQ simple sender/receiver
summary: Sending commands from a send-only endpoint to a receiving endpoint using the IBM MQ transport.
reviewed: 2026-03-25
component: IBMMQ
related:
- transports/ibmmq
- transports/ibmmq/connection-settings
- transports/ibmmq/topology
---

This sample demonstrates the basics of sending and receiving messages with the IBM MQ transport. A send-only **Sender** endpoint dispatches `MyMessage` commands to a **Receiver** endpoint, which processes each message with a simulated delay.

The sample includes:

- A **Sender** console application configured as a send-only endpoint that sends a user-specified number of messages
- A **Receiver** console application that processes incoming messages
- A **Shared** library containing the message contract

## Prerequisites

The sample requires a running IBM MQ broker. A Docker Compose file is included:

```bash
docker compose up -d
```

This starts IBM MQ with queue manager `QM1` on port `1414`. The management console is available at `https://localhost:9443/ibmmq/console` (credentials: `admin` / `passw0rd`).

## Running the sample

1. Start the **Receiver** project first. It calls `EnableInstallers()` on startup, which creates any missing queues automatically.
2. Start the **Sender** project.
3. Enter a number in the Sender console and press Enter. That many `MyMessage` commands are sent to the Receiver.
4. The Receiver logs `Start <data>` when each message begins processing, then `End: <data>` after a simulated 200 ms delay.

## Code walk-through

### Sender configuration

Both endpoints initialise an `IBMMQTransport` instance with the connection details for the local broker:

snippet: SenderConfig

The connection properties correspond to the Docker Compose defaults: queue manager `QM1`, host `localhost`, port `1414`, channel `DEV.ADMIN.SVRCONN`. The Sender is also configured with `SendOnly()`, which prevents NServiceBus from creating an input queue for the Sender process.

### Creating a message

The message being sent is the following record type, inheriting `IMessage` type from NServiceBus. This sample uses a record type but NServiceBus supports [interfaces and classess as message types](/nservicebus/messaging/#messaging-concepts-in-nservicebus).

snippet: Message

### Sending a message

snippet: SendingMessage

### Receiver configuration

snippet: ReceiverConfig

`EnableInstallers()` creates the `DEV.RECEIVER` queue on the broker if it does not already exist. Delayed retries are disabled so that any processing failures move messages directly to the error queue without cycling through retry delays.

### Message handler

snippet: MyHandler

`MyHandler` simulates a 200 ms processing delay. The `CancellationToken` from `context.CancellationToken` is forwarded to `Task.Delay` so the handler responds promptly to endpoint shutdown.

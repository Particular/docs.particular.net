---
title: Simple PostgreSQL transport usage
summary: Learn how to configure NServiceBus with the PostgreSQL transport to send commands and publish events between endpoints using native publish-subscribe.
reviewed: 2026-03-03
component: PostgreSqlTransport
related:
- transports/postgresql
---

## Prerequisites

Ensure a PostgreSQL server is running and accessible. This sample connects to `localhost` on port `54320` using the credentials in the connection string. The database named `nservicebus` must exist before running the sample.

## Running the sample

1. Start both the Sender and Receiver projects.
1. Press <kbd>c</kbd> to send a [command](/nservicebus/messaging/messages-events-commands.md), or <kbd>e</kbd> to publish an [event](/nservicebus/messaging/messages-events-commands.md), from Sender to the Receiver.
1. The Receiver handles the message.

## Code walkthrough

### Configuring the PostgreSQL transport

snippet: TransportConfiguration

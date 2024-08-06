---
title: Simple PostgreSQL transport usage
summary: A simple send and receive scenario with the PostgreSQL transport.
reviewed: 2024-05-27
component: PostgreSqlTransport
related:
- transports/postgresql
---

## Prerequisites

This sample uses a database named `nservicebus`. Ensure it exists before running the sample.

## Running the sample

1. Start both the Sender and Receiver projects.
1. Press <kbd>c</kbd> to send a [command](/nservicebus/messaging/messages-events-commands.md), or <kbd>e</kbd> to publish an [event](/nservicebus/messaging/messages-events-commands.md), from Sender to the Receiver.
1. The Receiver handles the message.

## Code walk-through

### Configuring the PostgreSQL transport

snippet: TransportConfiguration

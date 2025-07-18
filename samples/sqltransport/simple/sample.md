---
title: Simple SQL Server transport usage
summary: A simple send and receive scenario with the SQL Server transport.
reviewed: 2024-02-01
component: SqlTransport
related:
- transports/sql
---

## Prerequisites

include: sql-prereq

The sample creates a database named `SQLServerSimple`.

## Running the sample

1. Start both the Sender and Receiver projects. Both need to be run at least once before sending a command to create the required database tables.
1. Press <kbd>c</kbd> to send a [command](/nservicebus/messaging/messages-events-commands.md), or <kbd>e</kbd> to publish an [event](/nservicebus/messaging/messages-events-commands.md), from Sender to the Receiver.
1. The Receiver handles the message.

## Code walk-through

### Configuring the SQL Server transport

snippet: TransportConfiguration

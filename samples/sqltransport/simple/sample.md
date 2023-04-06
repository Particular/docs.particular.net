---
title: Simple SQL Server transport usage
summary: A simple send and receive scenario with the SQL Server transport.
reviewed: 2021-06-10
component: SqlTransport
related:
- transports/sql
---

## Prerequisites

include: sql-prereq

The sample creates a database named `SQLServerSimple`.

## Running the sample

 1. Start both the Sender and Receiver projects.
 1. Press <kbd>c</kbd> to send a [command](/nservicebus/messaging/messages-events-commands.md), or <kbd>e</kbd> to publish an [event](/nservicebus/messaging/messages-events-commands.md), from Sender to Receiver.
 1. Receiver handles the message in the matching handler.

## Code walk-through

### Configuring the SQL Server transport

snippet: TransportConfiguration

---
title: Raw messaging using NServiceBus
summary: How to send and receive raw messages using NServiceBus transport infrastructure
component: Core
reviewed: 2026-02-26
---

The transport infrastructure can be used directly without the need to spin up a full NServiceBus endpoint. This is especially useful when integrating with third-party systems and when building message gateways or bridges.

## Configuration

Configuration of the messaging infrastructure is done via the `Initialize` method:

snippet: Configuration

## Sending

The following code sends a message to another endpoint using an `IMessageDispatcher` that is part of the initialized infrastructure:

snippet: Sending

## Receiving

The following code starts the configured receiver (identified by ID `"Primary"`). Each infrastructure object can contain multiple receivers and each receiver can be started separately. Once stopped, receivers cannot be restarted; if pause-like functionality is required, it is necessary to create a new infrastructure object on each pause resume.

snippet: Receiving

The `TransportTransaction` object can be passed from the receiving context to the dispatcher, in order to ensure transactions span both send and receive operations. It's important to ensure that the underlying transport infrastructure supports the [`SendsAtomicWithReceive` transaction mode](/transports/transactions.md#transaction-modes-transport-transaction-sends-atomic-with-receive) when using this option.

## Shutting down

snippet: Shutdown

Before shutting down the infrastructure, be sure to stop all the receivers.

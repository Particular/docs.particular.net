---
title: Raw messaging using NServiceBus
summary: How to send and receive raw messages using NServiceBus transport infrastructure
component: RawMessaging
tags:
 - Raw
 - Messaging
---

NServiceBus.Raw allows sending and receiving raw messages using NServiceBus transport infrastructure. It does not offer the rich APIs and powerful features of full NServiceBus but in exchange offers complete freedom in terms of message manipulation.

## Configuration

Configuration of raw endpoints is very straightforward and follows the same patterns as regular NServiceBus endpoint configuration

snippet:Configuration

## Sending

The following code sends a message to another endpoint

snippet:Sending

## Receiving

The following code implements the callback invoked when a message arrives at a raw endpoint

snippet:Receiving

Notice the method gets a `dispatcher` object which can be used to send messages. The transport transaction object can be passed from the receiving context to the dispatcher to ensure transactions spans both send and receive if the underlying transport infrastructure supports [such mode](/nservicebus/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive).
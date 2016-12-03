---
title: Raw messaging using NServiceBus
summary: How to send and receive raw messages using NServiceBus transport infrastructure
component: RawMessaging
reviewed: 2016-12-03
tags:
 - Raw
 - Messaging
---

`NServiceBus.Raw` allows sending and receiving raw messages using [NServiceBus transport infrastructure](/nservicebus/transports/). It is flexible in terms of message manipulation, therefore is a good fit for integrations with 3rd party systems, building gateways and bridges.


## Configuration

Configuration of raw endpoints is similar to the standard NServiceBus [endpoint configuration](/nservicebus/endpoints/specify-endpoint-name.md):

snippet:Configuration


## Sending

The following code sends a message to another endpoint:

snippet:Sending


## Receiving

The following code implements the callback invoked when a message arrives at a raw endpoint:

snippet:Receiving

Notice the method gets a `dispatcher` object which can be used to send messages. The transport transaction object can be passed from the receiving context to the dispatcher, in order to ensure transactions span both send and receive. Make sure that the underlying transport infrastructure supports the [`SendsAtomicWithReceives` mode](/nservicebus/transports/transactions.md#transactions-transport-transaction-sends-atomic-with-receive), when using this option.
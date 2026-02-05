---
title: Transaction Support
reviewed: 2025-12-19
component: ASBS
versions: '[1,)'
---


The following [`TransportTransactionMode` levels](/transports/transactions.md) are supported

- `SendsAtomicWithReceive` (default)
- `ReceiveOnly`
- `None`

## Sends atomic with receive

The `SendsAtomicWithReceive` guarantee is achieved by leveraging [Azure Service Bus transaction processing capability](https://learn.microsoft.com/en-us/azure/service-bus-messaging/service-bus-transactions).

All outgoing messages sent as part of an incoming message will be forwarded to their destinations by Azure Service Bus only when the complete operation is called on the received message. The message won't be forwarded if the lock duration limit is exceeded (5 minutes by default) or if the message is explicitly abandoned.

The receive throughput of this transaction mode can be significantly slower due to the sender and receiver needing to share the same underlying AMQP connection.

## Receive only

The `ReceiveOnly` guarantee is based on the Azure Service Bus ['PeekLock' mode](https://learn.microsoft.com/en-us/azure/service-bus-messaging/message-transfers-locks-settlement#peeklock).

The message is not removed from the queue directly after receiving, but it's hidden for the duration of the peek-lock (5 minutes by default). This prevents other instances from receiving the message. If the receiver fails to process the message within that time frame and [auto-lock-renewal](configuration.md#lock-renewal) was not enabled or explicitly abandons the message, then the message will become visible again for the same or competing consumers.

## Unreliable (transactions disabled)

When transactions are disabled in NServiceBus, the transport uses the Azure Service Bus ['ReceiveAndDelete' mode](https://learn.microsoft.com/en-us/azure/service-bus-messaging/message-transfers-locks-settlement#receiveanddelete).

The message is deleted from the queue directly after the receive operation completes and before it is processed, so it's not possible to retry that message in case of processing failures. As transient exceptions occur regularly when integrating with online services, disabling retries when in unreliable mode is not recommended. This mode should only be used when message loss is acceptable.

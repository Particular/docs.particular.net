---
title: Transaction Support
reviewed: 2020-12-07
component: ASBS
versions: '[1,)'
---


The following [`TransportTransactionMode` levels](/transports/transactions.md) are supported

 * `SendsAtomicWithReceive` (default)
 * `ReceiveOnly`
 * `None`


## Sends atomic with receive

The `SendsAtomicWithReceive` guarantee is achieved by setting the `ViaEntityPath` on outbound message senders. Its value is set to the receiving queue.

If the `ViaEntityPath` is not empty, then messages will be added to the receive queue. The messages will be forwarded to their destinations (inside the broker) only when the complete operation is called on the received brokered message. The message won't be forwarded if the lock duration limit is exceeded (5 minutes by default) or if the message is explicitly abandoned.

The receive throughput of this transaction mode can be significantly slower due to the sender and receiver needing to share the same TCP connection.

## Receive only

The `ReceiveOnly` guarantee is based on the Azure Service Bus [Peek-Lock mode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.receivemode).

The message is not removed from the queue directly after receive, but it's hidden for 5 minutes (by default). This prevents other instances from receiving the message. If the receiver fails to process the message within that time frame or explicitly abandons the message, then the message will become visible again. Other instances will be able to pick it up (effectively rolling back the incoming message).


## Unreliable (transactions disabled)

When transactions are disabled in NServiceBus, the transport uses the Azure Service Bus ['ReceiveAndDelete' mode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.receivemode).

The message is deleted from the queue directly after the receive operation completes and before it is processed, so it's not possible to retry that message in case of processing failures. As transient exceptions occur regularly when integrating with online services, disabling retries when in unreliable mode is not recommended. This mode should only be used in very specific situations, when message loss is acceptable.

NOTE: For a full explanation of the transactional behavior in Azure, refer to [Understanding internal transactions and delivery guarantees](/transports/azure-service-bus/legacy/understanding-transactions-and-delivery-guarantees.md).

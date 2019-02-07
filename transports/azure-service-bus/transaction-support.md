---
title: Transaction Support
reviewed: 2019-02-07
component: ASBS
versions: '[7,)'
tags:
 - Azure
 - Transactions
---


The following [`TransportTransactionMode` levels](/transports/transactions.md) are supported

 * `SendsAtomicWithReceive` (default)
 * `ReceiveOnly`
 * `None`


## Sends atomic with Receive

include: send-atomic-with-receive-note

The `SendsAtomicWithReceive` guarantee is achieved by setting the `ViaEntityPath` on outbound message senders. Its value is set to the receiving queue.

If the `ViaEntityPath` is not empty, then messages will be added to the receive queue. The messages will be forwarded to their destinations (inside the broker) only when the complete operation is called on the received brokered message. The message won't be forwarded if the lock duration limit is exceeded (30 seconds by default) or if the message is explicitly abandoned.


## Receive Only

The `ReceiveOnly` guarantee is based on the Azure Service Bus [Peek-Lock mode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.receivemode).

The message is not removed from the queue directly after receive, but it's hidden for, by default, 5 minutes. This prevents other instances from receiving the message. If the receiver fails to process the message within that time frame or explicitly abandons the message, then the message will become visible again. Other instances will be able to pick it up (effectively that rolls back the incoming message).


## Unreliable (Transactions Disabled)

When transactions are disabled in NServiceBus then the transport uses the Azure Service Bus ['ReceiveAndDelete' mode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.receivemode).

The message is deleted from the queue directly after the receive operation completes, before it is processed, meaning that it's not possible to retry that message in case of processing failures. As transient exceptions occur regularly when integrating with online services, disabling retries when in unreliable mode is not recommended. This mode should only be used in very specific situations, when message loss is acceptable.

NOTE: For a full explanation of the transactional behavior in Azure, refer to [Understanding internal transactions and delivery guarantees](/transports/azure-service-bus/legacy/understanding-transactions-and-delivery-guarantees.md).

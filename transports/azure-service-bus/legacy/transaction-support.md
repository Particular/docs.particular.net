---
title: Transaction Support
reviewed: 2020-12-07
component: ASB
versions: '[7,)'
redirects:
 - nservicebus/azure-service-bus/transaction-support
 - transports/azure-service-bus/transaction-support
---

include: legacy-asb-warning


The following [`TransportTransactionMode` levels](/transports/transactions.md) are supported

 * `SendsAtomicWithReceive`
 * `ReceiveOnly`
 * `None`


## Sends atomic with Receive

include: send-atomic-with-receive-note

The `SendsAtomicWithReceive` guarantee is achieved by setting the `ViaEntityPath` property on outbound message senders. Its value is set to the receiving queue.

If the `ViaEntityPath` is not empty, then messages will be added to the receive queue. The messages will be forwarded to their destinations (inside the broker) only when the complete operation is called on the received brokered message. The message won't be forwarded if the lock duration limit is exceeded (30 seconds by default) or if the message is explicitly abandoned.

DANGER: The messages will not be delivered to the destination queue that exceeds its maximum size and transaction will **NOT** fail. Instead, transaction will be reported as successful and messages for the destination queue will be dead-lettered by the broker. To avoid this, plan entities [maximum size](/transports/azure-service-bus/legacy/configuration/full.md) to accomodate production throughputs and possible scenarios.


## Receive Only

The `ReceiveOnly` guarantee is based on the Azure Service Bus [Peek-Lock mode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.receivemode).

The message is not removed from the queue directly after receive, but it's hidden for, by default, 30 seconds. This prevents other instances from receiving the message. If the receiver fails to process the message within that time frame or explicitly abandons the message, then the message will become visible again. Other instances will be able to pick it up (effectively that rolls back the incoming message).


## Unreliable (Transactions Disabled)

When transactions are disabled in NServiceBus then the transport uses the Azure Service Bus ['ReceiveAndDelete' mode](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.receivemode).

The message is deleted from the queue directly after the receive operation completes, before it is processed, meaning that it's not possible to retry that message in case of processing failures. As transient exceptions occur regularly when integrating with online services, disabling retries when in unreliable mode is not recommended. This mode should only be used in very specific situations, when message loss is acceptable.

NOTE: For a full explanation of the transactional behavior in Azure, refer to [Understanding internal transactions and delivery guarantees](understanding-transactions-and-delivery-guarantees.md).


## Duplicate Detection

Using the supported transaction levels, the transport only provides an **`at-least-once`** delivery model. Azure Service Bus provides a [duplicate detection feature](https://docs.microsoft.com/en-us/azure/service-bus-messaging/duplicate-detection) which will ensure the message is received exactly once if enabled. However, Duplicate Detection comes at the expense of throughput and is time constrained.

WARN: The Azure Service Bus duplicate detection feature is not compatible with the [second level retries](/nservicebus/recoverability/#delayed-retries). Enabling duplicate detection and second level retries together can cause message loss.

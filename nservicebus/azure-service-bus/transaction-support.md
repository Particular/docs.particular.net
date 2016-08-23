---
title: Azure Service Bus Transport Transaction Support
reviewed: 2016-04-20
tags:
- Azure
- Cloud
- Transactions
---

## Transaction Support

Azure Service Bus Transport supports `SendAtomicWithReceive`, `ReceiveOnly` and `None` transaction mode levels.

### Sends atomic with Receive

include: send-atomic-with-receive-note

The `SendAtomicWithReceive` guarantee is achieved by setting the `ViaEntityPath` property on outbound message senders. Its value is set to the receiving queue.

If the `ViaEntityPath` is not empty, then messages will be added to the receive queue. The messages will be forwarded to their actual destination (inside the broker) only when the complete operation is called on the received brokered message. The message won't be forwarded if the lock duration limit is exceeded (30 seconds by default) or if the message is explicitly abandoned.

### Receive Only

The `ReceiveOnly` guarantee is based on the Azure Service Bus Peek-Lock mechanism.

The message is not removed from the queue directly after receive, but it's hidden for, by default , 30 seconds. That prevents other instances from picking it up. If the receiver fails to process the message withing that timeframe or explicitly abandons the message, then the message will become visible again. Other instances will be able to pick it up (effectively, rolls back the incoming message).


### Unreliable (Transactions Disabled)

When transactions are disabled in NServiceBus then the transport uses the [ASB's 'ReceiveAndDelete' mode](https://msdn.microsoft.com/en-us/library/microsoft.servicebus.messaging.receivemode.aspx).

The message is deleted from the queue directly after receive operation completes, before it is processed, meaning that no retries will occur. As transient exceptions occur regularly when integrating with online services, having retries off in unreliable mode is highly unrecommended. This mode should only be used in very specific situations!

NOTE: For a full explanation of the transactional behavior, refer to [Understanding internal transactions and delivery guarantees](understanding-transactions-and-delivery-guarantees.md).

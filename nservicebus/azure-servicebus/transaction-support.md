---
title: Azure Service Bus Transaction Support
summary: Azure ServiceBus Transaction Support
tags:
- Azure
- Cloud
- Configuration
---

## Transactions and delivery guarantees

NServiceBus Azure Service Bus transport relies on the underlying Azure Service Bus library which requires the use of the `Serializable` isolation level (the most restrictive isolation level that does not permit `dirty reads`, `phantom reads` and `non repeatable reads`; will block any reader until the writer is committed. For more information refer to [Transaction Isolation Levels Explained in Details](http://dotnetspeak.com/2013/04/transaction-isolation-levels-explained-in-details) article.

NServiceBus AzureServiceBus transport configuration is hard-coded to `Serializable` isolation level. Users can't override it.


### Version 6 and above

Azure Service Bus Transport supports `SendAtomicWithReceive`, `ReceiveOnly` and `Unreliable` levels.


#### SendAtomicWithReceive

Note: `SendAtomicWithReceive` level is supported only when destination and receive queues are in the same namespace.

The `SendAtomicWithReceive` guarantee is achieved by using `ViaEntityPath` property on outbound messages. It's value is set to the receiving queue.

If the `ViaEntityPath` is not empty, then messages will be added to the receive queue. The messages will be forwarded to their actual destination (inside the broker) only when the complete operation is called on the received brokered message. The message won't be forwarded if the lock duration limit is exceeded (30 seconds by default) or if the message is explicitly abandoned.


#### ReceiveOnly

The `ReceiveOnly` guarantee is based on the Azure Service Bus Peek-Lock mechanism. 

The message is not removed from the queue directly after receive, but it's hidden by default for 30 seconds. That prevents other instances from picking it up. If the receiver fails to process the message withing that timeframe or explicitly abandons the message, then the message will become visible again. Other instances will be able to pick it up.


#### Unreliable (Transactions Disabled)

When transactions are disabled then NServiceBus uses the [ASB's ReceiveAndDelete mode](https://msdn.microsoft.com/en-us/library/microsoft.servicebus.messaging.receivemode.aspx).

The message is deleted from the queue directly after receive operation completes, before it is processed.


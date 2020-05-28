---
title: Transaction Support
summary: A description of the transaction modes supported by the Azure Storage Queues transport
component: ASQ
versions: '[6,]'
reviewed: 2020-03-23
redirects:
 - nservicebus/azure-storage-queues/transaction-support
 ---

The following [`TransportTransactionMode` levels](/transports/transactions.md) are supported

 * `ReceiveOnly`
 * `Unreliable`


## Transport transaction - receive only

Azure Storage Queues use a `Peek-Lock` model to overcome the lack of transactions. The message is not removed from the queue directly after it is received, but it is hidden for 30 seconds. This prevents other instances from picking it up. If the receiver fails to process the message within that timeframe or if it explicitly abandons the message, the message will become visible again and other instances will be able to pick it up. `ReceiveOnly` **[transport transaction mode](/transports/transactions.md)** provides an `at-least-once` delivery model.


## Unreliable (transactions disabled)

The message is deleted from the queue directly after the receive operation completes and before it is processed.
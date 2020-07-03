---
title: Discarding expired messages
summary: Using native MSMQ features to discard messages if they have not been processed within a provided time window.
reviewed: 2020-06-17
component: MsmqTransport
related:
- nservicebus/messaging/discard-old-messages
---

The MSMQ transport can handle messages with a [Time-To-Be-Received (TTBR)](/nservicebus/messaging/discard-old-messages.md) set in two ways. 

## Native

When a message with a TTBR value is sent, NServiceBus translates the value to the [native TTBR property](https://docs.microsoft.com/en-us/dotnet/api/system.messaging.message.timetobereceived) of the MSMQ message. MSMQ continuously checks the Time-To-Be-Received of all queued messages. As soon as the message has expired, it is removed from the queue, and disk space gets reclaimed. 

NOTE: MSMQ enforces a single Time-To-Be-Received value for all messages in a transaction. If multiple messages enlist in a single transaction, then the TTBR from the first message will be used for all messages, leading to potentially unintentional message expiration. To prevent message loss, TTBR is not supported for endpoints with [transaction mode](/transports/transactions.md) `SendsAtomicWithReceive` or `TransactionScope` by default.

partial: ttbr-send

## Non-native

NServiceBus also annotates outgoing messages with an `NServiceBus.TimeToBeReceived` [header](/nservicebus/messaging/headers.md).

partial: ttbr-receive

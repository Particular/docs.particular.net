---
title: Discarding expired messages
summary: Using native MSMQ features to discard messages not processed within a specified time window.
reviewed: 2025-05-19
component: MsmqTransport
related:
  - nservicebus/messaging/discard-old-messages
---

The MSMQ transport can discard messages that exceed a configured [Time-To-Be-Received (TTBR)](/nservicebus/messaging/discard-old-messages.md) in two ways: **natively** or through **non-native** handling by NServiceBus.

## Native

When a message with a TTBR value is sent, NServiceBus maps that value to the [native `TimeToBeReceived` property](https://docs.microsoft.com/en-us/dotnet/api/system.messaging.message.timetobereceived) of the MSMQ message. MSMQ continuously checks the TTBR of queued messages. Once a message has expired, it is automatically removed from the queue and the corresponding disk space is reclaimed.

> [!NOTE]
> MSMQ enforces a single Time-To-Be-Received value for all messages within a transaction. If multiple messages participate in the same transaction, the TTBR from the **first** message is applied to all. This can lead to unintended expiration and message loss.
>
> To prevent this, TTBR is **not supported by default** for endpoints using [transaction modes](/transports/transactions.md) `SendsAtomicWithReceive` or `TransactionScope`.

partial: ttbr-send

## Non-native

In addition to using MSMQ's native behavior, NServiceBus also includes a `NServiceBus.TimeToBeReceived` [header](/nservicebus/messaging/headers.md) on outgoing messages.

partial: ttbr-receive

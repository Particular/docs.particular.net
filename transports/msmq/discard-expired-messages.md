---
title: Discarding expired messages
summary: Using native MSMQ features to discard messages not processed within a provided time window.
reviewed: 2023-06-02
component: MsmqTransport
related:
- nservicebus/messaging/discard-old-messages
---

The MSMQ transport can handle messages with a [Time-To-Be-Received (TTBR)](/nservicebus/messaging/discard-old-messages.md) set in two ways:

| Transaction mode       | MSMQ TTBR mode |
|------------------------|----------------|
| TransactionScope       | Non-native     |
| SendsAtomicWithReceive | Non-native     |
| ReceiveOnly            | Native         |
| None                   | Native         |

## Native

When a message with a TTBR value is sent, NServiceBus translates the value to the [native TTBR property](https://docs.microsoft.com/en-us/dotnet/api/system.messaging.message.timetobereceived) of the MSMQ message. MSMQ continuously checks the TTBR of all queued messages. As soon as the message has expired, it is removed from the queue, and disk space gets reclaimed.

> [!NOTE]
> MSMQ enforces a single Time-To-Be-Received value for all messages in a transaction. If multiple messages enlist in a single transaction, then the TTBR from the first message will be used for all messages, leading to potentially unintentional message expiration. To prevent message loss, TTBR is not supported for endpoints with [transaction mode](/transports/transactions.md) `SendsAtomicWithReceive` or `TransactionScope` by default.

partial: ttbr-send

> [!WARNING]
> Due to a bug in Version 6 `TransportTransactionMode.ReceiveOnly` wrongly enlisted all outgoing messages in the same transaction causing the issues described above.

### Validation

The MSMQ tranport forces the same TimeToBeReceived on all messages in a transaction. If OverrideTimeToBeReceived is detected when using MSMQ an exception will be thrown with the following text:

```txt
Setting a custom OverrideTimeToBeReceived for audits is not supported on transactional MSMQ
```

## Non-native

NServiceBus also annotates outgoing messages with an `NServiceBus.TimeToBeReceived` [header](/nservicebus/messaging/headers.md).

## Ignore the `NServiceBus.TimeToBeReceived` header

partial: ttbr-receive

---
title: Transaction support
summary: Transaction modes supported by the IBM MQ transport
reviewed: 2026-02-19
component: IbmMq
---

The IBM MQ transport supports the following [transport transaction modes](/transports/transactions.md):

- Transport transaction - Sends atomic with receive
- Transport transaction - Receive only (default)
- Unreliable (transactions disabled)

`TransactionScope` mode is not supported because the IBM MQ .NET client does not participate in distributed transactions.

> [!NOTE]
> Exactly-once message processing without distributed transactions can be achieved with any transport using the [Outbox](/nservicebus/outbox/) feature.

## Sends atomic with receive

In `SendsAtomicWithReceive` mode, the message receive and all outgoing send/publish operations are committed or rolled back as a single unit of work.

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
});
transport.TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive;
```

> [!NOTE]
> Messages sent outside of a handler (e.g., via `IMessageSession`) are not included in the atomic operation.

## Receive only

In `ReceiveOnly` mode, the message receive is transactional. Successfully processed messages are committed; failed messages are returned to the queue. Outgoing send and publish operations are not part of the receive transaction.

This is the default transaction mode.

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
});
transport.TransportTransactionMode = TransportTransactionMode.ReceiveOnly;
```

> [!WARNING]
> If the connection to the queue manager is lost after processing succeeds but before the commit, the message will be redelivered. This can result in duplicate processing. Use the [Outbox](/nservicebus/outbox/) feature to guarantee exactly-once processing.

## Unreliable (transactions disabled)

In `None` mode, messages are consumed without any transactional guarantees. If processing fails, the message is lost.

```csharp
var transport = new IbmMqTransport(options =>
{
    // ... connection settings ...
});
transport.TransportTransactionMode = TransportTransactionMode.None;
```

> [!CAUTION]
> This mode should only be used when message loss is acceptable, such as for non-critical telemetry or logging messages.

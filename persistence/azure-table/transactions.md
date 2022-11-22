---
title: Transactions in Azure Table persistence
component: ASP
versions: 'ASTP:[3,]'
related:
- nservicebus/outbox
reviewed: 2020-11-19
---

partial: intro

A custom [behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) must be introduced to identify and insert the `TableEntityPartitionKey` value into the pipeline context for use by storage operations that occur during the processing of a given message.

INFO: Do not use a [message mutator](/nservicebus/pipeline/message-mutators.md) to identify the partition key. Message mutators do not offer the necessary control or timing to reliably interact with this persistence.

The custom behavior can be introduced in one of the two stages:

## `ITransportReceiveContext` stage

This is the earliest stage in the message processing pipeline. At this stage only the message ID, the headers and a byte array representation of the message body are available.

snippet: ITransportReceiveContextBehavior

### Interaction with outbox

This stage occurs before the [outbox](/nservicebus/outbox) logic is executed. Identifying the `TableEntityPartitionKey` during this stage allows the outbox feature to work entirely as intended. In the case where the `TableEntityPartitionKey` cannot be identified using only the message headers, a behavior in the `IIncomingLogicalMessageContext` stage can be introduced instead.

## `IIncomingLogicalMessageContext` stage

This is the first stage in the pipeline that allows access to the deserialized message body. At this stage both the message headers and deserialized message object are available.

snippet: IIncomingLogicalMessageContextBehavior

### Interaction with outbox

[Outbox](/nservicebus/outbox) message guarantees work by bypassing the remaining message processing pipeline when an outbox record is located. Since this stage occurs after the normal bypass logic is executed, no `TableEntityPartitionKey` is available to locate an existing outbox record.

Azure Table persistence introduces a new `LogicalOutboxBehavior` to locate the outbox record and bypass the remaining message processing pipeline in the same `IIncomingLogicalMessageContext` stage as the custom `TableEntityPartitionKey` behavior. As a result, the custom behavior must be inserted into the pipeline _before_ the `LogicalOutboxBehavior`.

To specify the ordering for the custom `TableEntityPartitionKey` behavior:

snippet: InsertBeforeLogicalOutbox

To register the custom `TableEntityPartitionKey` behavior:

snippet: RegisterLogicalBehavior

WARN: Caution must be used when custom behaviors have been introduced in the pipeline that [dispatch messages immediately](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately). If these behaviors execute before the `LogicalOutboxBehavior` the [outbox message guarantees](/nservicebus/outbox/#how-it-works) may be broken.

partial: sharing
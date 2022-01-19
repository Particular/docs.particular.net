A custom [behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) must be introduced to identify and insert the [`PartitionKey`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.documents.partitionkey?view=azure-dotnet) value into the pipeline context for use by storage operations that occur during the processing of a given message.

INFO: Do not use a [message mutator](/nservicebus/pipeline/message-mutators.md) to identify the partition key. Message mutators do not offer the necessary control or timing to reliably interact with this persistence.

The custom behavior can be introduced in one of the two stages:

## `ITransportReceiveContext` stage

This is the earliest stage in the message processing pipeline. At this stage only the message ID, the headers and a byte array representation of the message body are available.

snippet: CosmosDB-ITransportReceiveContextBehavior

### Interaction with outbox

This stage occurs before the [outbox](/nservicebus/outbox) logic is executed. Identifying the [`PartitionKey`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.documents.partitionkey?view=azure-dotnet) during this stage allows the outbox feature to work entirely as intended. In the case where the `PartitionKey` cannot be identified using only the message headers, a behavior in the `IIncomingLogicalMessageContext` stage can be introduced instead.

## `IIncomingLogicalMessageContext` stage

This is the first stage in the pipeline that allows access to the deserialized message body. At this stage both the message headers and deserialized message object are available.

snippet: CosmosDB-IIncomingLogicalMessageContextBehavior

### Interaction with outbox

[Outbox](/nservicebus/outbox) message guarantees work by bypassing the remaining message processing pipeline when an outbox record is located. Since this stage occurs after the normal bypass logic is executed, no [`PartitionKey`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.documents.partitionkey?view=azure-dotnet) is available to locate an existing outbox record.

Cosmos DB persistence introduces a new `LogicalOutboxBehavior` to locate the outbox record and bypass the remaining message processing pipeline in the same `IIncomingLogicalMessageContext` stage as the custom `PartitionKey` behavior. As a result, the custom behavior must be inserted into the pipeline _before_ the `LogicalOutboxBehavior`.

To specify the ordering for the custom `PartitionKey` behavior:

snippet: CosmosDB-InsertBeforeLogicalOutbox

To register the custom `PartitionKey` behavior:

snippet: CosmosDBRegisterLogicalBehavior

WARN: Caution must be used when custom behaviors have been introduced in the pipeline that [dispatch messages immediately](/nservicebus/messaging/send-a-message.md#dispatching-a-message-immediately). If these behaviors execute before the `LogicalOutboxBehavior` the [outbox message guarantees](/nservicebus/outbox/#how-it-works) may be broken.

## Using a behavior to specify the `Container`

The container name can be provided using a custom behavior at the `ITransportReceiveContext` stage:

snippet: CustomContainerNameUsingITransportReceiveContextBehavior

or at the `IIncomingLogicalMessageContext` stage

snippet: CustomContainerNameUsingIIncomingLogicalMessageContextBehavior

include: containeroverride
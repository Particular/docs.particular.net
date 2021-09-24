---
title: Upgrade NServiceBus pipeline extensions from Version 7 to 8
summary: Instructions on how to upgrade NServiceBus pipeline extensions do from version 7 to version 8.
reviewed: 2020-02-20
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

Information on breaking changes affecting maintainers of pipeline extensions like pipeline behaviors and message mutators.

## IManageUnitOfWork

`IManageUnitOfWork` interface is no longer recommended. The unit of work pattern is more straightforward to implement in a pipeline behavior, where the using keyword and try/catch blocks can be used.

[Custom unit of work sample](/samples/pipeline/unit-of-work/) is an example of the the recommended approach.

## Message mutators with Dependancy Injection (DI)

Message mutators that operate on serialized messages (`IMutateIncomingTransportMessages` and `IMutateOutgoingTransportMessages`) in NServiceBus version 8 represent the message payload as `ReadOnlyMemory<byte>` instead of `byte[]`. Therefore, it is no longer possible to change individual bytes. Instead, a modified copy of the payload must be provided.

In NServiceBus version 7 and below message mutators could be registered in two ways: using a dedicated `endpointConfiguration.RegisterMessageMutator` API and via a dependency injection container. In version 8 only the dedicated API is supported. Mutators registered in the container are ignored.

## Removing a behavior from the pipeline is obsolete

The `Remove` command is no longer available in `PipelineSettings`. In order to disable a behavior, [replace the behavior](/nservicebus/pipeline/manipulate-with-behaviors.md?version=core_8#disable-an-existing-step) with an empty one.

## Pipeline delivery constraints

The `TryGetDeliveryConstraint` method on the `NServiceBus.Extensibility.ContextBag` property has been removed. In order to access delivery constraints from within the pipeline, use `NServiceBus.Extensibility.ContextBag.TryGet` instead.

```csharp
var extensions = context.Extensions;
if (extensions.TryGet(out DiscardIfNotReceivedBefore constraint))
{
    timeToBeReceived = constraint.MaxTime;
}
```

### Pipeline context types changes

Throughput the pipeline all contexts have been updated to use `ReadOnlyMemory<byte>` (e.g. context for behaviors, stage connectors, and message mutators) and affects the following types:

* MutateIncomingTransportMessageContext
* MutateOutgoingTransportMessageContext
* ConnectorContextExtensions
* IIncomingPhysicalMessageContext
* IncomingPhysicalMessageContext
* IOutgoingPhysicalMessageContext
* OutgoingPhysicalMessageContext
* SerializeMessageConnector

## Message bodies only available in scope of incoming or outgoing messages

Messages bodies available via `ReadOnlyMemory<byte>` are only valid while in scope of incoming or outgoing message processing. Once out of scope the data referenced is no longer valid.

If access is still required outside the processing scope it is required to copy the message body while in scope.

### Message Mutators: Updating of message bodies

The message mutator API for changing the message body has changed. Instead, of `UpdateMessage(byte[] body)` method `MutateIncomingTransportMessageContext` and `MutateOutgoingTransportMessageContext` expose `Body` property of type `ReadOnlyMemory<byte>`.

Message mutators that replace the whole message body might be better of to be converted to a behavior. With a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md) it is possible to reduce allocations via [`ArrayPool<byte>`](https://docs.microsoft.com/en-us/dotnet/api/system.buffers.arraypool-1) or packages like [RecyclableMemoryStream](https://github.com/Microsoft/Microsoft.IO.RecyclableMemoryStream).

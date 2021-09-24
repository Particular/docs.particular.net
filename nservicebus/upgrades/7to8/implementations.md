---
title: Upgrade NServiceBus downstreams from Version 7 to 8
summary: Instructions on how to upgrade NServiceBus do from version 7 to version 8.
reviewed: 2020-02-20
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

Information on breaking changes affecting maintainers of downstream components like custom/community transports, persistence, and message serializers.

## Serializers: Deserialization using ReadOnlyMemory of byte

Deserialization updated to use `ReadOnlyMemory<byte>` instead of `Stream` with  on `Deserialize` API method on `IMessageSerializer`.

```csharp
object[] Deserialize(ReadOnlyMemory<byte> body, IList<Type> messageTypes = null);
```

If serializers do not support `ReadOnlyMemory<byte>` usage of `.ToArray` must be avoided as this will copy the data and increases memory allocations. Instead include a shim that provides read-only access to the underlying data or use the `ReadOnlyMemoryExtensions.AsStream` from the [Microsoft.Toolkit.HighPerformance](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.highperformance.extensions.readonlymemoryextensions.asstream).

## Transports

### Transports: Outgoing message TransportOperation API

The outgoing message body passed to the transport via `TransportOperation` has a new constructor:

```csharp
public TransportOperation(string messageId, DispatchProperties properties, ReadOnlyMemory<byte> body, Dictionary<string, string> headers)
```

### Transports: Incoming message MessageContext API

A message body passed by the transport to the core using `ReadOnlyMemory<byte>` instead of `byte[]`. The `MessageContext` becomes:

```csharp
public MessageContext(string nativeMessageId, Dictionary<string, string> headers, ReadOnlyMemory<byte> body, TransportTransaction transportTransaction, ContextBag context)
```

For transports that use low allocation types, this allows passing message body without additional memory allocations. Secondly, this ensures that the body passed by the transport cannot be changed by code executing in the pipeline (immutable message body). 

## Persisters

### Persister Outbox API: TransportOperation based on `ReadOnlyMemory<byte>`

The outbox `TransportOperation` is using `ReadOnlyMemory<byte>` instead of `byte[]`.

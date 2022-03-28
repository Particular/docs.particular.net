---
title: Upgrade NServiceBus downstreams from Version 7 to 8
summary: Instructions on how to upgrade NServiceBus do from version 7 to version 8.
reviewed: 2022-03-24
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

This documentation contains information about breaking changes that may affect components like custom transports, persistence, and message serializers.

## Serializers: Deserialization using ReadOnlyMemory of byte

Deserialization is updated to use `ReadOnlyMemory<byte>` instead of `Stream`. The `Deserialize` method on `IMessageSerializer` becomes:

```csharp
object[] Deserialize(ReadOnlyMemory<byte> body, IList<Type> messageTypes = null);
```

Serializers that do not support `ReadOnlyMemory<byte>` as deserialization input should avoid `.ToArray` calls to prevent unnecessary memory allocations. Instead, it's recommended to implement a shim exposing read-only data as types supported by the serializer APIs or use existing implementations like `ReadOnlyMemoryExtensions.AsStream` from the [Microsoft.Toolkit.HighPerformance](https://docs.microsoft.com/en-us/dotnet/api/microsoft.toolkit.highperformance.extensions.readonlymemoryextensions.asstream) package.

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

For transports that use low allocation types, this allows passing message body without additional memory allocations. Secondly, this ensures that the message body is immutable and cannot be changed by the code executing in the pipeline. 

## Persisters

### Persister Outbox API: TransportOperation based on `ReadOnlyMemory<byte>`

The outbox `TransportOperation` is using `ReadOnlyMemory<byte>` instead of `byte[]`.

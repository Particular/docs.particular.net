---
title: Backwards Compatibility
summary: Describes the requirements for backward compatibility with the legacy Azure Service Bus
component: ASBS
reviewed: 2024-02-20
---

The Azure Service Bus transport is backward compatible with the legacy Azure Service Bus transport under certain conditions.

## Conditions

### Forwarding topology

The Azure Service Bus transport only supports the [forwarding topology](/transports/azure-service-bus/topology.md).

### Single namespace

The Azure Service Bus transport only supports a single namespace.

### Topic path must match

Both transports must be configured using the same topic path for publishing to work properly. The Azure Service Bus transport must use the same topic as the old Azure Service Bus transport. Also, only one topic path is allowed.

Therefore, for publishers that are using the legacy transport version 7 or lower, the number of entities in a bundle must be restricted to one using:

```csharp
forwardingTopology.NumberOfEntitiesInBundle(1);
```

### Namespace alias is not used

The Azure Service Bus transport doesn't support namespace aliases.

### Sanitization rules must be aligned

If the legacy transport [sanitizes](/transports/azure-service-bus/configuration.md#entity-creation) entity names, the sanitization logic must be updated to be compatible with the new transport.

For example, for the `ValidateAndHashIfNeeded` strategy, the sanitization functions must include the strategy logic to preserve the same entity names.

snippet: asb-sanitization-compatibility

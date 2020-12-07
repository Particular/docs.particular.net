---
title: Backwards Compatibility
summary: Describes the requirements for backward compatibility with legacy Azure Service Bus
component: ASBS
reviewed: 2020-12-07
---

The Azure Service Bus transport is backward compatible with the legacy Azure Service Bus transport under certain conditions.

## Conditions

### Forwarding topology

The Azure Service Bus transport only supports the [forwarding topology](/transports/azure-service-bus/legacy/topologies.md#versions-7-and-above-forwarding-topology), an entity layout where a topic is used for publishing between the endpoints.

### Single namespace

The Azure Service Bus transport only supports a single namespace.

### Topic path must match

Both transports must be configured using the same topic path for publishing to work properly. This implies that the topic used by the endpoints using the Azure Service Bus transport must match the topic used by the endpoints on legacy Azure Service Bus. See [bundle prefix](/transports/azure-service-bus/legacy/configuration/full.md#configuring-the-topology-forwarding-topology) for details.

In addition, only one topic path is supported. For publishers that are using the legacy Azure Service Bus transport version 7 or lower, the number of entities in a bundle must be restricted to one using:

```
forwardingTopology.NumberOfEntitiesInBundle(1);
```

### Namespace alias is not used

The Azure Service Bus transport doesn't support [namespace aliases](/transports/azure-service-bus/legacy/securing-connection-strings.md).

### Sanitization rules must be aligned

If the legacy transport [sanitizes](/transports/azure-service-bus/legacy/sanitization.md) entity names, the sanitization logic must be updated to be compatible with the new transport.

For example, for the `ValidateAndHashIfNeeded` strategy, the sanitization functions must include the strategy logic to preserve the same entity names.

snippet: asb-sanitization-compatibility

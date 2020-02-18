---
title: Backwards Compatibility
summary: Describes the requirements for backward compatibility with legacy Azure Service Bus
component: ASBS
tags:
 - Azure
reviewed: 2019-02-07
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

### Sanitization rules need to be aligned

If the usage of the legacy transport involved [sanitization](/transports/azure-service-bus/legacy/sanitization.md) of entity names the sanitization logic needs to be made compatible.

For example, if the `ValidateAndHashIfNeeded` strategy was used then the sanitization functions need to include the strategy logic to preserve the same entity names.

snippet: asb-sanitization-compatibility

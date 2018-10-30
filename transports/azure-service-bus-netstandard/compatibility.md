---
title: Backwards Compatibility
summary: Describes the requirements for backward compatibility with legacy Azure Service Bus
component: ASBS
tags:
 - Azure
reviewed: 2018-06-21
---

The Azure Service Bus transport is backward compatible with the legacy Azure Service Bus transport under certain conditions.

## Conditions

### Forwarding topology

The Azure Service Bus transport only supports the [forwarding topology](/transports/azure-service-bus/topologies/#versions-7-and-above-forwarding-topology), an entity layout where a topic is used for publishing between the endpoints.

### Single namespace

The Azure Service Bus transport only supports a single namespace.

### Topic path must match

Both transports must be configured using the same topic path for publishing to work properly. This implies that the topic used by the endpoints using the Azure Service Bus transport must match the topic used by the endpoints on legacy Azure Service Bus. See [bundle prefix](/transports/azure-service-bus/configuration/full.md#configuring-the-topology-forwarding-topology) for details.

### Namespace alias is not used

The Azure Service Bus transport doesn't support [namespace aliases](/transports/azure-service-bus/securing-connection-strings.md).

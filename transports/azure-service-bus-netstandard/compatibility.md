---
title: Backwards Compatibility
summary: Describes the requirements for backwards compatibility with ASB transport
component: ASBS
tags:
 - Azure
reviewed: 2018-06-21
---

The Azure Service Bus .NET Standard transport is backwards compatible with the Azure Service Bus transport under a certain set of preconditions.

## Preconditions

### Forwarding topology

The Azure Service Bus .NET Standard transport only supports the [Forwarding topology](/transports/azure-service-bus/topologies/#versions-7-and-above-forwarding-topology), an entity layout where a topic is used for publishing between the endpoints.

### Single namespace

The Azure Service Bus .NET Standard transport only supports a single namespace.

### Topic path must match

Both transports must be configured using the same topic path for publishing to work properly. This implies that the topic used by the endpoints using Azure Service Bus for .NET Standard transport must match topic used by the endpoints on Azure Service Bus (see [bundle prefix](/transports/azure-service-bus/configuration/full.md#configuring-the-topology-forwarding-topology) for details).

### Namespace alias not used

The Azure Service Bus .NET Standard transport doesn't support [namespace aliases](/transports/azure-service-bus/securing-connection-strings.md).

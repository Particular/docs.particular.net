---
title: Compatibility
summary: Describes the requirements for backward compatibility with ASB transport
component: ASBS
tags:
 - Azure
reviewed: 2018-06-19
---

## Backward compatibility

The Azure Service Bus .Net Standard transport is backward compatible with the Azure Service Bus transport under a certain set of preconditions.

### Preconditions

#### Forwarding topology

The Azure Service Bus .Net Standard transport only supports the forwarding topology, an entity layout where a topic is used for publishing between the endpoints.

#### Single namespace

The Azure Service Bus .Net Standard transport only supports a single namespace.

#### Topic path must match

Both transports must be configured using the same topic path for publishing to work properly. This implies that the [bundle prefix](https://docs.particular.net/transports/azure-service-bus/configuration/full#configuring-the-topology-forwarding-topology) on the ASB transport side must match the topic path (TODO: add link once config is there) configured on the ASBS side.

#### Namespace alias not used

The Azure Service Bus .Net Standard transport doesn't support namespace aliases.
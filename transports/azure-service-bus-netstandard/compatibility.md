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

The Azure Service Bus .Net Standard transport only supports the forwarding topology, an entity layout where a shared set of topics is used for publishing between the endpoints.

#### Single namespace

The Azure Service Bus .Net Standard transport only supports a single namespace at the moment.

#### Topic prefix must be matching

Both transports must be configured using the same topic prefix for publishing to work properly.

#### Namespace alias not used

The Azure Service Bus .Net Standard transport currently doesn't support namespace aliases.
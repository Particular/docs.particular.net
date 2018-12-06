---
title: Migration off Endpoint-Oriented topology
summary: Migrating system off Endpoint-Oriented topology to Forwarding topology
component: ASB
versions: '[9.1,)'
tags:
- Azure
- Transport
reviewed: 2018-08-29
related:
 - transports/azure-service-bus/legacy/topologies
---

include: legacy-asb-warning

Endpoint-Oriented topology is the first topology that was introduced in the Azure Service Bus transport. The topology design is influenced by the concept of [message-driven publish-subscribe](/nservicebus/messaging/publish-subscribe/#mechanics-message-driven-persistence-based) and imposes several [restrictions](/transports/azure-service-bus/legacy/topologies#versions-7-and-above-forwarding-topology-topologies-comparison) such as coupling of subscribers to the publishers. The ForwardingTopology was introduced to take advantage of the broker nature of the Azure Service Bus and to leverage its native capabilities. It is the recommended option for new projects. Thus the new Azure Service Bus transport only supports the ForwardingTopology.

Customers that are using the Endpoint-Oriented Topology planning to migrate their endpoints to the ForwardingTopology and eventually to the new transport, a gradual and step by step migration approach was introduced. The migration approach described here allows migrating endpoints in any order to the ForwardingTopology, allowing to take mission-critical endpoints into account without applying a big-bang approach.

## Side-by-side migration

describe steps


## Moving off the legacy transport

What's needed to move to the new transport? transports/azure-service-bus/compatibility


## Finalizing migration (cleanup stage)

What needs to be done to remove parts of old topology that is no longer utilized


## Nerdy section - how it works at the high livel

High level description of how migration operates. Similar to transports/rabbitmq/delayed-delivery.md#how-it-works
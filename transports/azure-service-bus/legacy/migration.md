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

INFO: Endpoints that have the migration mode enabled require manage rights to create entities on the broker.

Before endpoints can be moved to the ForwardinTopology, all endpoints need to have the migration mode enabled. The following steps are required

1. Pick an endpoint that is using the Endpoint-Oriented Topology indicated by `transport.UseEndpointOrientedTopology()` and update the `NServiceBus.Azure.Transports.WindowsAzureServiceBus` nuget package reference to Version 9.1 or higher
1. Enable the migration mode by calling `EnableMigrationToForwardingTopology()` on the Endpoint-Oriented Topology, leave all the other configuration as is

It is possible to apply the above steps to as many endpoints at once as required or deemed feasible. Once all the endpoints that need to be updated have the migration mode enabled deploy the updated endpoints to production. Apply the update, configuration and release to production cycle as many times necessary until all endpoints that are using the Endpoint-Oriented Topology have the migration mode enabled.

Once all endpoints have the migration mode enabled and have been running in that mode in production for at least a day or until all existing messages have been fowarded to the receiving endpoints input queue, the endpoints can be moved to the ForwardingTopology either on the current transport or by switching to the new Azure Service Bus transport by following the guidance in the next section.

## Moving off the legacy transport

What's needed to move to the new transport? transports/azure-service-bus/compatibility


## Finalizing migration (cleanup stage)

What needs to be done to remove parts of old topology that is no longer utilized


## Nerdy section - how it works at the high livel

High level description of how migration operates. Similar to transports/rabbitmq/delayed-delivery.md#how-it-works
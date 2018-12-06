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

Once all endpoints have the migration mode enabled and have been running in that mode in production for at least a day or until all existing messages have been fowarded to the receiving endpoints input queue, the endpoints can be moved to the ForwardingTopology by switching to the new Azure Service Bus transport as described in the next section.

## Moving off the legacy transport

INFO: It is possible to move to the ForwardingTopology on the old transport first before moving to the new transport. Follow the guideline in the this section but skip the new package installation step and replace `transport.UseEndpointOrientedTopology()` with `transport.UseForwardingTopology()`.

With all endpoints running in migration mode or already on the ForwardingTopology on the legacy transport the migration to the new transport is just around the corner. Before the switch to the new transport is started it is suggested to check compatibility with the new transport described in the [compatibility guidance](/transports/azure-service-bus/compatibility).

1. Uninstall the package  `NServiceBus.Azure.Transports.WindowsAzureServiceBus`
1. Install the package `NServiceBus.Transports.AzureServiceBus`
1. Delete the non compiling code starting with `transport.UseEndpointOrientedTopology()` and remove any routing configuration code described in [Publishers name configuration](/transports/azure-service-bus/legacy/publisher-names-configuration)
1. If more [advanced configuration options](/transports/azure-service-bus/legacy/configuration/full) have been used on the old transport switch them over to the [new configuration options](/transports/azure-service-bus/configuration) if available. For configuration options not available on the new transport rest assured sensible production ready defaults are being used in the new transport.

It is possible to apply the above steps to as many endpoints at once as required or deemed feasible. Once all the endpoints that need to be updated have been moved to the new transport using the ForwardingTopology deploy the updated endpoints to production. Apply the update, configuration and release to production cycle as many times necessary until all endpoints that are using the Endpoint-Oriented Topology with migration mode have been moved to the ForwardingTopology. Endpoints using Endpoint-Oriented topology with migration mode enabled can co-exists and seamlessly communicate with endpoints using the ForwardingTopology.

## Finalizing migration (cleanup stage)

What needs to be done to remove parts of old topology that is no longer utilized


## Nerdy section - how it works at the high livel

High level description of how migration operates. Similar to transports/rabbitmq/delayed-delivery.md#how-it-works
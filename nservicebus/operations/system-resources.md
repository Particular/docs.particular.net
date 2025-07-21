---
title: System resources
summary: System resources consist of all infrastructure pieces needed for the endpoint to work properly
reviewed: 2025-07-21
component: core
related:
 - nservicebus/operations
---

System resources (pieces of infrastructure) required by endpoints to run can be deployed automatically using [installers](installers). When deploying infrastructure manually without using installers (i.e., with Infrastructure as Code), the following elements need to be handled:

Components:

- What versions of NServiceBus are used
- What message types are used (including inheritance hierarchy)
- What assemblies are used (including dynamic assemblies and automatically scanned assemblies)
- What Platform components are used (i.e., monitoring instance, error instance, audit instance, etc.)
- Whether MassTransit Connector is used
- Whether transport adapters are used
- Whether message bridges are used
- Whether gateways are used

Core components:

- Name of the endpoint
- Discriminators the endpoint uses
- Queues required by the endpoint (i.e., input queue, error queue, audit queue, queues for metrics, queues for heartbeats, etc.)
- Subscriptions created by the endpoint (i.e., subscription for every event type, manually created subscriptions, etc.)
- Whether outbox is used
- Whether auditing is enabled
- Whether saga auditing is enabled
- Whether satellites are used
- Message types processed by this endpoint (including inheritance hierarchy, dynamically laoded types, and assembly scanning)

Transport specific:

- Topology (i.e., bundle topics, hierarchy bundle, filtering for subscriptions, etc.)
- Attributes of the infrastructure (i.e., quotas, delivery count, TTL, etc.)
- Attributes of the subscription (i.e., topics, table prefixes, table names, etc.)
- Queues required by the transport (i.e., timeout queues, delay queues, dead letter queues, etc.)

Persistence specific:

- Outbox details (i.e., prefixes and table names, etc.)
- Saga details (i.e., prefixes and table names, etc.)
- (Legacy) Timeout details (i.e., prefix and table names for timeout manager, etc.)
- (Legacy) Subscription cache (i.e., prefix and table names, etc.)

Platform specific:

MassTransit connector specific:

Message bridge specific:

Transport adapter specific:

Gateway specific:


> [!NOTE]
> Different endpoints may have different requirements regarding the infrastructure (i.e., subscriptions or tables). Every endpoint needs to be considered when capturing the list of infrastructure pieces. Special attention should be payed to different versions of the transports as they may require different topologies or attributes.

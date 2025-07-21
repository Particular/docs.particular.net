---
title: Deployment-required system resources
summary: Describes all the system resources and infrastructure components needed for endpoints to work properly
reviewed: 2025-07-21
component: core
related:
 - nservicebus/operations
---

System resources (infrastructure components) required by endpoints to run can be deployed automatically using [installers](/nservicebus/operations/installers.md). When deploying infrastructure manually without using installers (i.e., with Infrastructure as Code), several key areas must be considered.

## Areas

> [!NOTE]
> This section may be incomplete. Review the documentation for each implemented feature or functionality to learn more. This section may also include deprecated and legacy elements, which may be of importance when deploying endpoints using older versions of NServiceBus.

The following list collects general information needed when deploying endpoints:

- What versions of [NServiceBus](/nservicebus/) are used
- What message types are used (including inheritance hierarchy and message assemblies versions)
- What assemblies are used (including dynamic assemblies and automatically scanned assemblies)
- What Platform components are used (i.e., [error instance](/servicecontrol/servicecontrol-instances/), [audit instance](/servicecontrol/audit-instances/), [monitoring instance](/servicecontrol/monitoring-instances/), etc)
- Whether the [MassTransit Connector](/servicecontrol/masstransit/) is used
- Whether any legacy [transport adapter](/servicecontrol/transport-adapter) is used
- Whether the [messaging bridge](/nservicebus/bridge/) is used, how many bridges have been implemented, and for each one of them:
  - What are the endpoints they are responsible for bridging
  - What are the transports they are supporting
- Whether any [gateway](/nservicebus/gateway/) is used and their configured  persistence type
- What [community components](/nservicebus/community/) are used

Core components:

- Name of the endpoint
- If the endpoint is configured to be [uniquely addressable](/nservicebus/messaging/routing#make-instance-uniquely-addressable)
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
- Technology-specific queues (i.e., queues for delays that differ between versions, queues for quorum, etc.)

Persistence specific:

- Outbox details (i.e., prefixes and table names, etc.)
- Saga details (i.e., prefixes and table names, etc.)
- (Legacy) Timeout details (i.e., prefix and table names for timeout manager, etc.)
- (Legacy) Subscription cache (i.e., prefix and table names, etc.)

Platform specific:

- Main instance:
 - Queues (i.e., input queue, error queue, error forwarding, saga audit queue, audit forwarding queue, queue used for Service Pulse, acknowledgement queue, staging queue for Edit & Retry, throughput data, internal errors)
 - Databases (i.e., RavenDB instances)
- Audit instance
 - Queues (i.e., input queue)
- Metrics instance
 - Queues (i.e., input queue)

MassTransit connector specific:

- Queues used by MassTransit (i.e., the queues on "the other end")
- Error queues
- Connector-specific queues (i.e., return queue, poison queue, etc.)

Message bridge specific:

- Queues (i.e., error queue, etc.)

Transport adapter specific:

- Queues (i.e., audit queue, error queue, control queue, poison message queue, etc.)

Gateway specific:

- Queues (i.e., input queue, etc.)

Community components:

- Elements required by community components


> [!NOTE]
> Different endpoints may have different requirements regarding the infrastructure (i.e., subscriptions or tables). Every endpoint needs to be considered when capturing the list of infrastructure pieces. Special attention should be payed to different versions of the transports as they may require different topologies or attributes.


> [!NOTE]
> Attention should be payed to deprecated and legacy elements if older version of NServiceBus is used.


## Automation

[Installers](/nservicebus/operations/installers.md) can be used be used to deploy the infrastructure automatically. For other mechanisms, consult the following public issues and voice your interest accordingly:

- https://github.com/Particular/NServiceBus/issues/7370
- https://github.com/Particular/NServiceBus/issues/7189

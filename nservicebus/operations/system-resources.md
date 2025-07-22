---
title: Deployment requirements for NServiceBus systems
summary: Outlines the infrastructure, configuration, and runtime resources required to successfully deploy an NServiceBus system to production.
reviewed: 2025-07-21
component: core
related:
 - nservicebus/operations
---

System resources (infrastructure components) required by endpoints to run can be deployed automatically using [installers](/nservicebus/operations/installers.md). When deploying infrastructure manually without using installers (e.g., with Infrastructure as Code), several information must be gathered.

> [!NOTE]
> To request other mechanisms for provisioning NServiceBus systems, consult the following public issues and voice your interest accordingly:
>
> - https://github.com/Particular/NServiceBus/issues/7370
> - https://github.com/Particular/NServiceBus/issues/7189

## Required information

> [!NOTE]
> This section may be incomplete. Review the documentation for each implemented feature or functionality to learn more. This section may also include deprecated and legacy elements, which may be of importance when deploying endpoints using older versions of NServiceBus.

The following general information is needed when provisioning endpoints:

- What versions of [NServiceBus](/nservicebus/) are used
- What message types are used (including inheritance hierarchy and message assemblies versions)
- What assemblies defining message [handlers](/nservicebus/handlers/), [sagas](/nservicebus/sagas/), or [features](/nservicebus/pipeline/features.md) are deployed alongside the endpoint. These assemblies may require additional infrastructure
- What Platform components are used (i.e., [error instance](/servicecontrol/servicecontrol-instances/), [audit instance](/servicecontrol/audit-instances/), [monitoring instance](/servicecontrol/monitoring-instances/), etc.)
- Whether the [MassTransit Connector](/servicecontrol/masstransit/) is used
- Whether any legacy [transport adapter](/servicecontrol/transport-adapter.md) is used

### Endpoint details

For each endpoint that needs to be deployed, the following information must be collected to successfully provision infrastructure without using installers:

- Name of the endpoint, which by default determines the [main input queue](/nservicebus/endpoints/specify-endpoint-name.md#input-queue) name
- If the endpoint is configured to be [uniquely addressable](/nservicebus/messaging/routing.md#make-instance-uniquely-addressable)
- Additional queues required by the endpoint (e.g., error, audit, metrics monitoring, heartbeats, and custom checks queues)
- Any custom endpoint [satellite](/nservicebus/satellites/) queue
- Subscriptions required by the endpoint (e.g., subscription for every event type, manually created subscriptions, etc.)
- Whether outbox is enabled
- Message types processed by this endpoint alongside their inheritance hierarchy

### Transport specific details

- Topology (e.g., bundle topics, hierarchy bundle, filtering for subscriptions, etc.)
- Infrastructure entity-specific attributes (e.g., queue quotas and delivery count, message TTL, topic names, table name prefixes and schema, etc.)
- Transport-specific entities (e.g., timeout queues, delay queues, dead letter queues, etc.)

### Persistence specific details

- Outbox configuration details (e.g., table schema, names, and prefixes, etc.)
- Saga details (e.g., table schema, names, and prefixes, etc.)
- (Legacy) Timeout details (e.g., prefix and table names for timeout manager, etc.)
- (Legacy) Subscription cache (e.g., prefix and table names, etc.)

### Platform specific details

- Main instance:
 - Queues (i.e., input queue, error queue, error forwarding, saga audit queue, audit forwarding queue, queue used for ServiceControl, acknowledgement queue, staging queue for Edit & Retry, throughput data, internal platform errors)
 - Database (i.e., the RavenDB instance)
- Audit instance
 - Queues (i.e., input queue)
- Metrics instance
 - Queues (i.e., input queue)

### MassTransit connector details

- The return queue for messages being retried back into production
- [Platform specific queues](#required-information-platform-specific-details)

### Message bridge details

- Queues (e.g., the error queue for the bridge, the [endpoint queues](#required-information-endpoint-details) that the bridge is connecting, etc.)
- Any [subscriptions](#required-information-endpoint-details) that need to be bridged

### Legacy transport adapter details

- Queues (e.g., audit queue, error queue, control queue, poison message queue, etc.)

### Gateway details

- Input queue
- Persistence details
- Incoming ports

### Community component details

- Elements required by community components. Details of these elements can be provided by the community component maintainers.


> [!NOTE]
> Different endpoints may have different requirements regarding the infrastructure (i.e., subscriptions or tables). Every endpoint needs to be considered when capturing the list of infrastructure pieces. Special attention should be payed to different versions of the transports as they may require different topologies or attributes.


> [!NOTE]
> Attention should be payed to deprecated and legacy elements if older version of NServiceBus is used.

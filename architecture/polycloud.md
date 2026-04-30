---
title: Polycloud systems
summary: Architectural guidance for systems that span multiple cloud providers.
reviewed: 2026-04-21
callsToAction: ['solution-architect', 'poc-help']
---

Polycloud systems use services from multiple cloud providers within a single distributed system, selecting from AWS, Azure, Google Cloud, or others based on capability, cost, compliance, or organizational requirements.

## Why polycloud

Organizations adopt polycloud strategies for several reasons:

- Distributing workloads across providers reduces dependency on any single vendor's pricing, roadmap, or availability.
- Different providers excel in different areas. A system might use Azure Service Bus for messaging while running compute workloads on AWS.
- Certain data may need to remain within a specific jurisdiction, while other components can run anywhere.
- Mergers and acquisitions bring existing cloud investments that need to be integrated without a full rewrite.

## Cloud-specific service selection

The general recommendation is to use the native services for each cloud provider.

### On Azure

[For Azure deployments](/architecture/azure/messaging.md), [Azure Service Bus](/transports/azure-service-bus/) is the recommended default. It supports cross-entity transactions on the Premium tier, message sizes up to 100 MB, and native publish/subscribe via topics and subscriptions.

### On AWS

[For components hosted in AWS](/architecture/aws/messaging.md), [Amazon SQS](/transports/sqs/) is the recommended option. It is fully managed, scales automatically, and integrates with other AWS services. NServiceBus uses Amazon SNS alongside SQS to support the publish/subscribe pattern. When messages exceed the SQS size limit (256 KB for events, 1 MiB for commands), the transport can offload payloads to Amazon S3.

### Persistence options

Depending on the scenario, different persistence options can be used for storing saga state and outbox records:

- [Azure Cosmos DB](/persistence/cosmosdb/). See the [simple Cosmos DB sample](/samples/cosmosdb/simple/) to get started.
- [Amazon DynamoDB](/persistence/dynamodb/). See the [simple DynamoDB persistence sample](/samples/aws/dynamodb-simple/) or the [sagas with SQS and Lambda sample](/samples/aws/sagas/) to get started.
- [SQL-based](/persistence/sql)
- [Other storage technologies](/persistence/#supported-persisters)
 
### Cloud-agnostic environments

In environments not tied to a specific cloud provider, other technologies like [RabbitMQ](/transports/rabbitmq/), [PostgreSQL](/transports/postgresql/), or [SQL Server](/transports/sql/) [can be selected based on specific requirements](/transports/selecting.md).

## Messaging Bridge

The [Messaging Bridge pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/MessagingBridge.html) solves cross-transport communication by providing a dedicated component that transfers messages between two or more transports. The bridge is transparent to endpoints on both sides: they send and receive messages to and from logical endpoints as if no bridge were involved.

The [NServiceBus Message Bridge](/nservicebus/bridge/) is a production-ready implementation of this pattern. It supports all NServiceBus transports, making it well-suited for polycloud deployments.

In the following example, endpoints running on AWS (using Amazon SQS) communicate with endpoints running on Azure (using Azure Service Bus) through a bridge:

```mermaid
flowchart LR
 Br(Bridge)
 OrderService[Order Service] <---> Br
 Br <---> BillingService[Billing Service]
 subgraph AWS
   OrderService
 end
 subgraph Azure
   BillingService
 end
```

Because the bridge handles message routing, endpoints on both sides require [no changes](/samples/bridge/simple/) to communicate across clouds.

## Bridge deployment options

### Two-cloud

The simplest topology connects two cloud environments with a single bridge instance. The bridge is configured with one transport on each side, routing messages between them.

```mermaid
flowchart LR
 Br(Bridge)
 A[Endpoint A] <---> Br
 B[Endpoint B] <---> Br
 Br <---> C[Endpoint C]
 Br <---> D[Endpoint D]
 subgraph AWS
   A
   B
 end
 subgraph Azure
   C
   D
 end
```

### Multiple clouds

When endpoints span three or more cloud environments, multiple bridges can be chained or run in parallel. Each bridge instance connects two transports. A message originating in AWS can pass through a bridge to Azure, and from there through a second bridge to another Azure environment.

```mermaid
flowchart LR
    Br1(Bridge 1)
    Br2(Bridge 2)
    A[Endpoint A] <---> Br1
    Br1 <---> B[Endpoint B]
    B <---> Br2
    Br2 <---> C[Endpoint C]
    subgraph AWS
      A
      Br1
    end
    subgraph "Azure<br>Business Unit 1"
      B
      Br2
    end
    subgraph "Azure<br>Business Unit 2"
      C
    end
```

Alternatively, a single bridge instance can be configured with more than two transports, acting as a hub that routes between all connected environments without chaining.

In other scenarios, [multiple bridge instances](/nservicebus/bridge/performance.md) can be deployed depending on the amount of traffic, each responsible for routing messages for a subset of endpoints. This distributes the load without requiring all endpoints to be reconfigured.

### Bridge placement

The bridge must be reachable to the messaging infrastructure of both transports it connects. Placing the bridge in one of the cloud environments (rather than on-premises) typically minimizes latency to at least one side. For low-latency or high-volume scenarios, place the bridge in the environment that handles the most traffic.

## Observability

The Particular Platform tools support polycloud deployments. The NServiceBus Messaging Bridge unifies observability across all clouds by capturing audit and error messages from every endpoint, giving a single view of failed messages, retries, and heartbeats regardless of where they originate.

```mermaid
flowchart LR
    Br(Bridge)
    SC[ServiceControl]
    A["Endpoint A<br>AWS SQS"] -->|audit/error| Br
    B["Endpoint B<br>Azure ASB"] -->|audit/error| Br
    Br --> SC
```

[The bridge forwards audit and error messages](/samples/bridge/service-control/) across transport boundaries. ServicePulse then provides a unified view of all endpoints, failed messages, and message flows across the entire polycloud system.

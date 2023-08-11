---
title: Azure Architectures
summary:
reviewed: 2023-07-18
suppressRelated: true
---



The Azure Architecture Center organizes application architecture fundamentals guidance as a [series of steps](https://learn.microsoft.com/en-us/azure/architecture/guide/#how-this-guidance-is-structured):

## Architecture styles

The choice of [architectural styles](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/) has broad impact on subsequent steps in designing a system architecture.

The Particular Service Platform supports the messaging patterns recommended by the Azure Architecture Center for the following widely used architectural styles:

* [Event driven architecture](/architecture/azure/event-driven-architecture.md)
* [Microservices architecture](/architecture/azure/microservices.md)
* [N-tier architecture](/architecture/azure/n-tier.md)
* [Web-queue-worker architecture](/architecture/azure/web-queue-worker.md)

## Technology choices

The Particular Service Platform supports the Azure services required to build distributed, message-driven systems. These articles help make informed [technology choices](https://learn.microsoft.com/en-us/azure/architecture/guide/#technology-choices) for Azure-focused solutions:

* [Compute](/architecture/azure/compute.md)
* [Data stores](/architecture/azure/data-stores.md)
* [Messaging](/architecture/azure/messaging.md)

Additional technology choices are described in [Technology choices for Azure solutions](https://learn.microsoft.com/en-us/azure/architecture/guide/technology-choices/technology-choices-overview).

Note: Azure services do not support distributed transactions. Read the [consistency guidance](/architecture/consistency.md) for more information about managing consistency in systems without distributed transactions.

## Application architecture

### Reference architectures

The Azure Architecture Center describes several [reference architectures](https://learn.microsoft.com/en-us/azure/architecture/browse/?filter=reference-architecture) that are compatible with the Particular Service Platform. Refer to [architectural styles](#architecture-styles) and [technology choices](#technology-choices) for details of how to apply those architectures using  the Particular Service Platform.

### Design principles

The Particular Service Platform makes helps systems follow the [ten design principles for Azure applications](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/).

* [Self healing](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/self-healing) is provided by the rich set of [recoverability](/architecture/recoverability.md) features like automatic retries, load leveling, throttling and more, which make application services resilient to failures.
* [Redundancy](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/redundancy) is provided by capabilities such as [scaling out](/nservicebus/scaling.md#scaling-out-to-multiple-nodes) and [high availability](/nservicebus/scaling.md#high-availability).
* [Coordination is minimized](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/minimize-coordination) between services by [messaging](/architecture/messaging.md). See [this talk](https://www.youtube.com/watch?v=0TYbHVc2yWI) for more information.
* [Scaling out](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/scale-out) is achieved by various methods such as [competing consumers](/nservicebus/scaling.md#scaling-out-to-multiple-nodes-competing-consumers).
* [Partitioning](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/partition) is achieved by decomposing and offload resource-intensive processes using [workflows](/architecture/workflows.md) and the integration of the Particular Service Platform with technology-specific partitioning capabilities.
* [Operations](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/design-for-operations) tooling for monitoring production environments and error recovery is provided by [ServicePulse](/servicepulse/) and [OpenTelemetry](/architecture/monitoring.md#opentelemetry).
* [Managed services](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/managed-services) such as Azure App Service and Azure Cosmos DB are fully supported by the Particular Service Platform. See [technology choices](#technology-choices).
* [A wide variety of data stores](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/use-best-data-store) are supported by the Particular Service Platform as [persistence choices](/persistence).
* [System evolution](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/design-for-evolution) is aided by:
  * High cohesion within [service boundaries](https://particular.net/webinars/finding-your-service-boundaries-a-practical-guide).
  * Loose coupling and asynchronous operations through [messaging](/architecture/messaging.md)
  * Encapsulating domain knowledge in [message handlers and sagas](/nservicebus/handlers-and-sagas.md).
  * Separating domain logic from infrastructure concerns, such as reliability and recoverability, which are handled by the Particular Service Platform.
  * Deploying services independently through the use of [asynchronous messaging](/nservicebus/messaging/).

[**Talk to a solution architect and learn more about how the Particular Service Platform helps systems follow Azure design principles. →**](https://particular.net/proof-of-concept)

### Design patterns // TODO: adam to continue here

The [Azure cloud design patterns catalog](https://learn.microsoft.com/en-us/azure/architecture/patterns/) lists many proven patterns to address specific challenges. The Particular Platform implements many of the described patterns like [Asynchronous Request-Reply](), [Circuit Breaker](), [Claim Check](), [Competing Consumers](), and more.

[**Talk to a solution architect to help determining the right patterns →**](https://particular.net/proof-of-concept)

## Microsoft Well-Architected Framework

The [Microsoft Azure Well-Architected Framework](https://learn.microsoft.com/en-us/azure/well-architected/) describes five pillars of software quality: Reliability, Security, Cost Optimization, Operational Excellence, and Performance Efficiency.

The Particular Platform helps achieving the five pillars of the well-architected Framwork:
* [Reliability](https://learn.microsoft.com/en-us/azure/well-architected/resiliency/overview)
    * NServiceBus is designed to handle even unexpected failures and enables se-f-healing systems implementing recoverability features.

    * NServiceBus sndpoints provide health metrics which can be monitored using ServicePulse or OpenTelemetry integration.
* [Security](https://learn.microsoft.com/en-us/azure/well-architected/security/security-principles)
    * NServiceBus endpoints are designed support least privilege configurations during application deployment and runtime.
    * Encrypt messages using the official [NServiceBus.Encryption.MessageProperty](/nservicebus/security/property-encryption.md) extension.
* [Cost Optimization](https://learn.microsoft.com/en-us/azure/well-architected/cost/overview)
    * TBD, maybe point to the technology choices documentation and try to provide more cost specific content?
* [Operational Excellence](https://learn.microsoft.com/en-us/azure/well-architected/devops/overview)
    * The Particular Platform supports creation of necessary infrastructure components using dedicated installation APIs or infrastructure scripting tools.
    * ServicePulse provides detailed insights into the operational health of the system.
    * Messaging patterns implemented by NServiceBus enable loosely coupled architectures to enable autonomous and indepndent services.
    * NServiceBus APIs are built for easy testability
* [Performance Effiency](https://learn.microsoft.com/en-us/azure/well-architected/scalability/overview)
    * NServiceBus endpoints can be easily scaled up or out using built-in competing consumers mechanisms and configurable concurrency settings.
    * NServiceBus is built and tested for high-performance and memory effiency.
    * Use ServicePulse to observe individual endpoint performance and identify bottlenecks.

## Additional resources:

* [Microsoft Azure Architecture Center](https://learn.microsoft.com/en-us/azure/architecture/)
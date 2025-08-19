---
title: Azure Architectures
summary: Gives an overview of the architectural styles (microservices, event-driven, n-tier, web-queue-worker) and technology options offered by Azure
reviewed: 2025-07-15
suppressRelated: true
callsToAction: ['architecture-review']
---

The Azure Architecture Center organizes application architecture fundamentals guidance as a [series of steps](https://learn.microsoft.com/en-us/azure/architecture/guide/#how-this-guidance-is-structured).

## Architecture styles

The Particular Service Platform supports the messaging patterns recommended by the Azure Architecture Center for the following [architectural styles](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/):

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

> [!NOTE]
> Azure services do not support distributed transactions. Read the [consistency guidance](/architecture/consistency.md) for more information about managing consistency in systems without distributed transactions.

## Application architecture

### Reference architectures

The Azure Architecture Center describes several [reference architectures](https://learn.microsoft.com/en-us/azure/architecture/browse/?terms=reference-architecture) that are compatible with the Particular Service Platform. Refer to [architectural styles](#architecture-styles) and [technology choices](#technology-choices) for details of how to apply those architectures using  the Particular Service Platform.

### Design principles

The Particular Service Platform makes systems follow the [ten design principles for Azure applications](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/).

* [Self healing](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/self-healing) is provided by the rich set of [recoverability](/architecture/recoverability.md) features like automatic retries, load leveling, throttling and more, which make application services resilient to failures.
* [Redundancy](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/redundancy) is provided by capabilities such as [scaling out](/nservicebus/scaling.md#scaling-out-to-multiple-nodes) and [high availability](/nservicebus/scaling.md#high-availability).
* [Coordination is minimized](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/minimize-coordination) between services by [messaging](/architecture/messaging.md). Watch [_Autonomous microservices don't share data. Period_ (video)](https://www.youtube.com/watch?v=0TYbHVc2yWI) for more recommendations.
* [Scaling out](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/scale-out) is achieved by various methods such as [asynchronous message-based communication](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/architect-microservice-container-applications/asynchronous-message-based-communication) and [competing consumers](/nservicebus/scaling.md#scaling-out-to-multiple-nodes-competing-consumers).
* [Partitioning](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/partition) is achieved by making [suitable technology choices](/architecture/azure/#technology-choices). NServiceBus directly supports and integrates with a range of Azure services that provide partitioning for large-scale systems.
* [Operations](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/design-for-operations) tooling for monitoring production environments and error recovery is provided by [ServicePulse](/servicepulse/) and [OpenTelemetry](/nservicebus/operations/opentelemetry.md).
* [Managed services](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/managed-services) such as Azure App Service, Azure SQL, and Azure Cosmos DB are fully supported by the Particular Service Platform. See [technology choices](#technology-choices).
  * [A wide variety of data stores](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/use-best-data-store) are supported by the Particular Service Platform as [persistence choices](/persistence).
* [Identity and access management](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/identity) is delegated to underlying infrastructure, such as Azure Active Directory or other identity-as-a-service platforms. The Particular Service Platform [builds on top of secure transport and data technologies](/nservicebus/security/), allowing authentication and authorization to be enforced at system boundaries. Custom identity logic inside message handlers is discouraged.
* [System evolution](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/design-for-evolution) is aided by:
  * High cohesion within [service boundaries](https://particular.net/webinars/finding-your-service-boundaries-a-practical-guide).
  * Loose coupling and asynchronous operations through [messaging](/architecture/messaging.md).
  * Encapsulating domain knowledge in [message handlers and sagas](/nservicebus/handlers-and-sagas.md).
  * Separating business logic from infrastructure concerns, such as reliability and recoverability, which are handled by the Particular Service Platform.
  * Enabling independent services deployment by decoupling their communication using [asynchronous messaging](/nservicebus/messaging/).
* [Meet business needs](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/build-for-business) by decomposing workloads into logically separated units using [messaging](/architecture/messaging.md). Allow each component's characteristics and design decisions to be optimized and [monitored](/servicepulse) for the specific business needs at hand (e.g. [resiliency](/architecture/recoverability.md), [scalability](/nservicebus/scaling.md), [consistency](/architecture/consistency.md), etc.).

[**Talk to a solution architect and learn more about how the Particular Service Platform helps systems follow Azure design principles →**](https://particular.net/solution-architect?message=I%27d%20like%20to%20talk%20to%20a%20solution%20architect%20to%20learn%20more%20about%20how%20the%20Particular%20Service%20Platform%20helps%20systems%20follow%20Azure%20design%20principles.)

### Design patterns

[Azure Cloud Design Patterns](https://learn.microsoft.com/en-us/azure/architecture/patterns/) lists many proven patterns for addressing specific challenges. The Particular Service Platform implements many of these such as [Asynchronous Request-Reply](/nservicebus/messaging/reply-to-a-message.md), [Circuit Breaker](/nservicebus/recoverability/#automatic-rate-limiting), [Claim Check](/nservicebus/messaging/claimcheck/), [Competing Consumers](/nservicebus/scaling.md#scaling-out-to-multiple-nodes-competing-consumers), and more.

[**Talk to a solution architect to help identify suitable design patterns →**](https://particular.net/solution-architect?message=I%27d%20like%20to%20talk%20to%20a%20solution%20architect%20to%20help%20identify%20suitable%20design%20patterns%20for%20my%20system.)

## Microsoft Well-Architected Framework

The Particular Service Platform helps achieve the five pillars of software quality described by the [Microsoft Azure Well-Architected Framework](https://learn.microsoft.com/en-us/azure/well-architected/). For example:

* [Reliability](https://learn.microsoft.com/en-us/azure/well-architected/reliability/)
  * NServiceBus handles even unexpected failures and provides the [recoverability features](/nservicebus/recoverability/) required by self-healing systems.
  * NServiceBus provides health metrics which can be monitored using [ServicePulse](/servicepulse) and [OpenTelemetry](/nservicebus/operations/opentelemetry.md).
* [Security](https://learn.microsoft.com/en-us/azure/well-architected/security/security-principles)
  * NServiceBus provides data encryption in transit with [message encryption](/nservicebus/security/property-encryption.md).
  * NServiceBus supports the [least privilege](/nservicebus/operations/installers.md#when-to-run-installers) approach during application deployment and runtime.
* [Cost optimization](https://learn.microsoft.com/en-us/azure/well-architected/cost-optimization/)
  * Costs may be optimized by [choosing the most appropriate technology](#technology-choices).
* [Operational excellence](https://learn.microsoft.com/en-us/azure/well-architected/operational-excellence/)
  * The Particular Service Platform [creates required infrastructure components](/nservicebus/operations/installers.md) using dedicated installation APIs or infrastructure scripting tools.
  * ServicePulse provides [detailed insights](/servicepulse) into the operational health of the system.
  * NServiceBus supports [OpenTelemetry](/nservicebus/operations/opentelemetry.md) to integrate with 3rd-party monitoring and tracing tools.
  * [Messaging](/nservicebus/messaging) allows loosely coupled architectures with autonomous and independent services.
  * NServiceBus APIs are designed for [unit testing](/nservicebus/testing).
* [Performance efficiency](https://learn.microsoft.com/en-us/azure/well-architected/performance-efficiency/)
  * NServiceBus endpoints can be easily scaled out using methods such as the built-in [competing consumers mechanism](/nservicebus/scaling.md#scaling-out-to-multiple-nodes-competing-consumers) and scaled up while tuning for [concurrency](/nservicebus/operations/tuning.md).
  * NServiceBus is designed and tested for [high-performance and memory efficiency](https://particular.net/blog/pipeline-and-closure-allocations).
  * [Monitoring](/monitoring) allows observation of individual endpoint performance and identification of bottlenecks.

## Additional resources

* [Microsoft Azure Architecture Center](https://learn.microsoft.com/en-us/azure/architecture/)
* [Azure services available by region](https://azure.microsoft.com/en-us/explore/global-infrastructure/products-by-region/)

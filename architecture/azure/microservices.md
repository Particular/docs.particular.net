---
title: Microservice architecture style on Azure
summary: Gives an overview of how the Particular Service Platform can help build microservices on Azure
reviewed: 2025-07-15
callsToAction: ['solution-architect', 'ADSD']
---

The Azure Architecture Center describes the [microservice architecture style](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/microservices) as consisting of a collection of small, autonomous services.

The Particular Service Platform implements the [messaging patterns](/nservicebus/messaging/) required to allow each [NServiceBus endpoint](/nservicebus/endpoints) to act as one of these autonomous services.

![Microservices on Azure with NServiceBus](azure-microservices.png)

## Components

* **[NServiceBus endpoint](/nservicebus/endpoints) (service)**

    Each service is an autonomously deployable and scalable unit with a private [data store](data-stores.md).

* **Message bus**

    The message bus provides an asynchronous, reliable, and fault-tolerant communication channel that decouples the services.

* **Gateway**

    A gateway is a facade that allows decoupling between different service layers and/or UI applications. Gateways may provide further operational features but do not contain business logic. Azure offers a [range of gateway services](https://learn.microsoft.com/en-us/azure/architecture/microservices/design/gateway).

## Challenges

### Service boundaries

Finding good service boundaries is one of the biggest challenges within the microservice architectural style. Suboptimal boundaries often lead to a lack of data isolation and excessive inter-service communication. This often leads to high coupling between services that implement business processes, leading to what is known as a ["distributed monolith"](https://particular.net/videos/microservices-and-distributed-monoliths). To define autonomous services, it is crucial to focus on business boundaries rather than technical boundaries.

<iframe allowfullscreen frameborder="0" height="300" mozallowfullscreen src="https://player.vimeo.com/video/113515335" webkitallowfullscreen width="400"></iframe>

In this presentation, Udi Dahan demonstrates the process of finding good service boundaries. He explains the challenges of traditional layered architectures and covers an approach that cuts across all application layers, outlining the natural lines of loose and tight coupling. Finally, Udi shows how these vertical slices collaborate using events, enabling flexible and high-performance business processes.

[**Blog post: Goodbye microservices, hello right-sized services →**](https://particular.net/blog/goodbye-microservices-hello-right-sized-services)

### RPC vs. messaging

Communication between services is much slower and error-prone due to network limitations compared to communication between modules inside a single process. This can lead to higher latency and increased impact of network outages. Asynchronous communication between services helps to mitigate these risks.

[**Read more about this topic: RPC vs. Messaging – which is faster? →**](https://particular.net/blog/rpc-vs-messaging-which-is-faster)

### User interfaces

Users often need to see and interact with data aggregated from multiple services. Several technologies and patterns help to do this while keeping the services decoupled, such as [ViewModel Composition](https://www.viewmodelcomposition.com), [microfrontends](https://en.wikipedia.org/wiki/Microfrontend), and [GraphQL](https://graphql.org/).

## Microservice technologies

Since each service within a microservice architecture style is hosted independently and can use its own data store, one of the major benefits of this architecture is the ability for a team to choose the most appropriate technologies for a given service without impacting other services or the teams working on them. Endpoints built with the Particular Service Platform are generally hosted separately, and each may use a different data store technology. The Particular Service Platform also supports [cross-platform integration with systems or components that do not use NServiceBus](https://particular.net/blog/cross-platform-integration-with-nservicebus-native-message-processing).

In Azure, services following the microservice architecture style often use the following technology options:

* Fully managed service offerings like [Azure Functions](/architecture/azure/compute.md#serverless) and [Azure App Services](/architecture/azure/compute.md#platform-as-a-service-azure-app-services) are popular choices for systems requiring scalable hosting environments with minimal management. These services also offer convenient solutions when building web-facing applications.
* [Containerized applications](/architecture/azure/compute.md#platform-as-a-service-containers) can be hosted in managed container orchestration platforms like Azure Container Apps, Azure Kubernetes Service, and more. They can also be hosted in custom-managed Kubernetes clusters running on [Azure Virtual Machines](https://azure.microsoft.com/en-us/products/virtual-machines). Container-hosted systems may use serverless data stores like [Cosmos DB](/architecture/azure/data-stores.md#azure-cosmos-db) and serverless messaging technologies [Azure Service Bus](/architecture/azure/messaging.md#azure-service-bus).

## Additional resources

* [Azure Architecture Center—Microservice architecture style](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/microservices)
* [Video: Finding service boundaries—a practical guide](https://www.youtube.com/watch?v=655zq4Sdu2w)

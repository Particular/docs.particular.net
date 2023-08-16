---
title: Microservice architecture style on Azure
summary:
reviewed: 2023-07-18
---

The Azure Architecture Center describes the [microservice architecture style](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/microservices) as consisting of a collection of small, autonomous services.

The Particular Service Platform implements the [messaging patterns](/nservicebus/messaging/) required to allow each [NServiceBus endpoint](/nservicebus/endpoints) to act as one of these autonomous services.

![](azure-microservices.png)

## Components

* [NServiceBus endpoint](/nservicebus/endpoints) (service): Each service is an autonomously deployable and scalable unit with a private [data store](data-stores.md).
* Azure Service Bus message queue: The message queue is an asynchronous, reliable, and fault-tolerant communication channel which decouples the services.
* Gateway: A gateway is a facade which allows user technologies such as web browsers to decouple from service implementations. Gateways may also provide further operational facilities, but do not contain domain-specific logic. Azure offers a [range of gateway services](https://learn.microsoft.com/en-us/azure/architecture/microservices/design/gateway).

## Challenges

### Service boundaries

Finding good service boundaries is one of the biggest challenges with the microservice architectural style. Suboptimal boundaries often lead to a lack of data isolation and excessive inter-service communication. This often leads to high coupling between services which implement a business processes, sometimes referred to as a distributed monolith. To define autonomous services, it is crucial to focus on domain (business concept) boundaries rather than technical boundaries.

<iframe allowfullscreen frameborder="0" height="300" mozallowfullscreen src="https://player.vimeo.com/video/113515335" webkitallowfullscreen width="400"></iframe>

In this presentation, Udi Dahan demonstrates the process of finding good service boundaries. He explains the challenges of traditional layered architectures and covers an approach that cuts across all application layers, outlining the natural lines of loose and tight coupling. Finally, Udi shows how these vertical slices collaborate using events, enabling flexible and high performance business processes.

[**Blog post: Goodbye microservices, hello right-sized services →**](https://particular.net/blog/goodbye-microservices-hello-right-sized-services)

### RPC vs. messaging

Communication between services is much slower and error-prone due to network limitations compared to communication between modules inside a single process. This can lead to higher latency and increased impact of network outages. Asynchronous communication between services helps to mitigate these risks.

[**Read more about this topic: RPC vs. Messaging – which is faster?  →**](https://particular.net/blog/rpc-vs-messaging-which-is-faster)

### User interfaces TODO (adam): continue here

Users interacting with the system often need to see data aggregated from multiple services. Maintaining high decoupling across services while providing meaningful user experiences. A wide range of technologies and patterns exist that are focusing on this problem type, e.g. [microfrontends](https://en.wikipedia.org/wiki/Microfrontend), [GraphQL](https://graphql.org/), [ViewModel Composition](https://www.viewmodelcomposition.com), etc.

## Microservice technologies

A major benefit of a microservices architecture is the easy integration of various technologies and services. Since each service runs in its own process and doesn't share the data storage, teams can chose the most appropriate technologies for a service without impacting other services or teams. The Particular Platform supports [Cross-platform integration with native message processing](https://particular.net/blog/cross-platform-integration-with-nservicebus-native-message-processing).

For cloud native microservices architectures, services are commonly hosted using serverless or container hosting models:

### Serverless

Fully managed serverless offerings like [Azure Functions](https://azure.microsoft.com/en-us/products/functions) and [Azure App Services](https://azure.microsoft.com/en-us/products/app-service/) are popular choices when development teams need powerful and scalable hosting environments that require little management effort.
Serverless hosting models also integrate seamlessly into serverless data storage and messaging technologies.

![](azure-functions-host.png)


### Containers

Containerized applications can be hosted in managed container orchestration platforms like Azure Container Apps or Azure Kubernetes Service. However, these applications can can also be hosted in custom managed Kubernetes clusters built upon Azure Virtual Machines. Even in container-focused systems, service communication and data storage might still be handled by serverless services like [Azure Service Bus](https://azure.microsoft.com/de-de/products/service-bus) or [Cosmos DB](https://azure.microsoft.com/de-de/products/cosmos-db/).

![](azure-container-host.png)

## Related content

* [Azure Architecture Center—Microservice architecture style](https://learn.microsoft.com/en-us/azure/architecture/guide/architecture-styles/microservices)

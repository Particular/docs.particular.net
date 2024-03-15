---
title: Microservice architecture style on AWS
summary:
reviewed: 2024-03-13
callsToAction: ['solution-architect', 'ADSD']
---

According to the [AWS guidance](https://aws.amazon.com/microservices/), microservices are an architectural approach where software is composed of small, independent services communicating over well-defined APIs.

The Particular Service Platform makes it easy to use microservices by defining [NServiceBus endpoints](/nservicebus/endpoints/) that act as one of these independent services. These endpoints use [messaging patterns](/nservicebus/messaging/) to ensure the services remain autonomous.

## Components

- [NServiceBus endpoint](/nservicebus/endpoints/) (service): Each service is an autonomously deployable and scalable unit with a private [data store](/architecture/azure/data-stores).
- Message bus: The message bus provides an asynchronous, reliable, and fault-tolerant communication channel which decouples the services.
- Gateway: A gateway is a facade which allows user technologies such as web browsers to decouple from service implementations. Gateways may also provide further operational facilities, but do not contain business logic. In AWS, APIs can be managed with [AWS API Gateway](https://aws.amazon.com/api-gateway/).

## Challenges

### Service boundaries

Finding good service boundaries is one of the biggest challenges with the microservice architectural style. Suboptimal boundaries often lead to a lack of data isolation and excessive inter-service communication. This often leads to high coupling between services that implement business processes, sometimes referred to as a distributed monolith. To define autonomous services, it is crucial to focus on business boundaries rather than technical boundaries.

– EMBED UDI’S PRESENTATION HERE

In this presentation, Udi Dahan demonstrates the process of finding good service boundaries. He explains the challenges of traditional layered architectures and covers an approach that cuts across all application layers, outlining the natural lines of loose and tight coupling. Finally, Udi shows how these vertical slices collaborate using events, enabling flexible and high performance business processes.

[Blog post: Goodbye microservices, hello right-sized services →](https://particular.net/blog/goodbye-microservices-hello-right-sized-services)

### RPC vs. messaging

Communication between services is much slower and error-prone due to network limitations compared to communication between modules inside a single process. This can lead to higher latency and increased impact of network outages. Asynchronous communication between services helps to mitigate these risks.

[**Read more about this topic: RPC vs. Messaging – which is faster? →**](https://particular.net/blog/rpc-vs-messaging-which-is-faster)

### User interfaces

Users often need to see and interact with data aggregated from multiple services. Several technologies and patterns help to do this while keeping the services decoupled, such as [ViewModel Composition](https://www.viewmodelcomposition.com), [microfrontends](https://en.wikipedia.org/wiki/Microfrontend), and [GraphQL](https://graphql.org/).

## Microservice technologies

A major benefit of the microservice architecture style, where each service is hosted independently, with its own private data store, is the ability for a team to choose the most appropriate technologies for a given service without impacting other services or the teams working on them. Endpoints built with the Particular Service Platform are generally hosted separately, and each may use a different data store technology. The Particular Service Platform also supports [cross-platform integration with systems or components which do not use NServiceBus](https://particular.net/blog/cross-platform-integration-with-nservicebus-native-message-processing).

Some common technology options for building microservices in AWS include:

- Fully managed services: Amazon Lambda and Amazon Elastic Beanstalk.
- Container services: Amazon Elastic Container Service (ECS), Amazon Elastic Kubernetes Service (EKS)
- Data stores: Amazon DynamoDB, Amazon Aurora, Amazon Relational Database Service (RDS)
- Messaging: Amazon Simple Queue Service (SQS), Amazon Simple Notification Service (SNS)

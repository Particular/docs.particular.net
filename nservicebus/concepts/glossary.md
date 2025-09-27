---
title: Glossary of messaging terms
summary: A glossary of terms used in distributed systems related to NServiceBus.
reviewed: 2025-09-26
---

A list of commonly used terms in NServiceBus systems.

### [Message](/nservicebus/messaging/)

A Message is the unit of communication for NServiceBus. Messages are sent and received by endpoints. There are two general types of messages:

* [Commands](/nservicebus/messaging/messages-events-commands.md): To request that an action should be taken.
* [Events](/nservicebus/messaging/messages-events-commands.md): To communicate that some action has occurred.

Message types can be set either using their respective interfaces, `ICommand` and `IEvent`, or via [conventions](/nservicebus/messaging/unobtrusive-mode.md) (so-called *unobtrusive mode*).

### Body or message body

The payload of the message is also called the body. It travels between the endpoints in a serialized form (either textual or binary).

### [Headers](/nservicebus/messaging/headers.md) or message headers

Any additional information about a message is communicated over the transport in a collection of key-value pairs. 
Message headers are similar to HTTP headers in the sense that they define how messages should be transmitted and processed. 
NServiceBus uses headers to implement some of its features. Users can also add [custom headers](/nservicebus/messaging/header-manipulation.md).

### [Endpoint](/nservicebus/endpoints/)

An endpoint is a logical entity that communicates with other endpoints via messaging. 
Each endpoint has an identifying name and contains a collection of message handlers and sagas. 
An endpoint can be deployed to a number of machines and environments. Each deployment of an endpoint is an instance.

### [Endpoint Instance](/nservicebus/endpoints/) or instance

An endpoint instance is a run-time object that allows interaction with the bus. Endpoint instances can send, receive, and publish messages. 
They execute associated message handlers and sagas to process incoming messages. An endpoint instance can have a single input queue.

### [Hosting](/nservicebus/hosting)

The act of hosting refers to running an endpoint instance inside some process. 
This can be any .NET process, such as a console application, a website, a Windows service, or a container instance. 
Multiple endpoint instances can be hosted in a single process.

### [Transport](/transports/)

The transport is the mechanism that NServiceBus endpoint instances use to communicate with each other. 
NServiceBus supports many different transports in a pluggable manner. 
For endpoint instances to communicate, they need to share a common transport technology.

### Input Queue

Each endpoint instance is assigned a single input queue. This queue can be shared among instances of the same endpoint but never with other endpoints.

### [Publish Subscribe](/nservicebus/messaging/publish-subscribe) or Pub/Sub

Publish-Subscribe is the interaction of

* Registering interest in being notified about an event (*subscriber*).
* That event being delivered to the endpoint that registered itself (*publisher*).

### [Handler](/nservicebus/handlers/) or message handler

A handler is a piece of code that processes a message of a given type. 
Message handlers are stateless.

### [Saga](/nservicebus/sagas/)

A saga can be thought of as a long-running handler that process multiple messages and shared states. 
It is the NServiceBus equivalent of a [Process Manager](https://www.enterpriseintegrationpatterns.com/patterns/messaging/ProcessManager.html) pattern.

### [Timeout](/nservicebus/sagas/timeouts.md)

A timeout is a message a saga sends to its future self to indicate the fact that some action needs to be performed. 
A timeout can contain the state which provides the context for that action.

### [Recoverability](/nservicebus/recoverability/)

NServiceBus has retry logic surrounding all calls to user code. 
This allows failing business code to be retried in a sensible way in order to resolve any interim problems (such as a database server restart). 
Messages that fail all retries are sent to an error queue for triage for either a retry or discarding.

### [Pipeline](/nservicebus/pipeline/)

The pipeline refers to the series of actions taken when an incoming message is processed and an outgoing message is sent.

### [Behavior](/nservicebus/pipeline/manipulate-with-behaviors.md)

A behavior is a single step in the pipeline.

### [Message Property Encryption](/nservicebus/security/property-encryption.md)

NServiceBus has both built-in encryption and extension points to create fully customized encryption models.

### [Auditing](/nservicebus/operations/auditing.md)

Allows forwarding every message successfully processed by an endpoint to a configured queue.

### [Serialization](/nservicebus/serialization/)

Serialization is the process of converting an in-memory object representation of a message into a stream of bytes to transmit them via the transport. 
For endpoint instances to be able to communicate, they need to use a common serialization technology.

### [Persistence](/persistence/)

Several NServiceBus features rely on persistent storage to function. 
This includes sagas, timeouts, gateway, outbox, and subscriptions (for transports that do not support Pub/Sub natively).

### [Dependency injection](/nservicebus/dependency-injection)

NServiceBus relies heavily on Inversion of Control Containers and Dependency Injection to manage services and state.

### [Assembly Scanning](/nservicebus/hosting/assembly-scanning.md)

NServiceBus leverages assembly scanning to implement several features.

### [Logging](/nservicebus/logging/)

NServiceBus has some sensible defaults for logging built-in and for more advanced scenarios, it supports routing log events to an external logging library.

### [Outbox](/nservicebus/outbox)

An alternative to distributed transactions is to provide exactly-once message processing semantics when accessing user data stored as part of message processing.

### [Ghost Message](/nservicebus/outbox/#the-consistency-problem)

Message handlers that modify business data in a database can run into problems when the messages that are sent become inconsistent with the changes made to business data, resulting in ghost messages or phantom records.

### [Idempotence](https://en.wikipedia.org/wiki/Idempotence)

The ability to call the same message handler more than once without causing inconsistent business results.

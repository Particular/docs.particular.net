---
title: Glossary of messaging terms
summary: A glossary of terms used in distributed systems related to NServiceBus.
reviewed: 2020-10-16
---

A glossary of terms used in distributed systems related to NServiceBus.


### [Message](/nservicebus/messaging/)

A Message is the unit of communication for NServiceBus. Messages are send and received by endpoints. There are two general types of messages:

 * [Command](/nservicebus/messaging/messages-events-commands.md): Used to request that an action should be taken.
 * [Event](/nservicebus/messaging/messages-events-commands.md): Used to communicate that some action has taken place.

Message types can be set either using interfaces `ICommand` and `IEvent` or via [conventions](/nservicebus/messaging/unobtrusive-mode.md) (so called *unobtrusive mode*).


### Body

The payload of the message is also called the body. It travels between the endpoints in a serialized form (either textual or binary).


### [Headers](/nservicebus/messaging/headers.md)

Additional information about a message is communicated over the transport in a collection of key value pairs. Message headers are similar to HTTP headers in the sense that they define how messages should be transmitted and processed. NServiceBus uses headers to implement some of its features. Users are also able to add [custom headers](/nservicebus/messaging/header-manipulation.md).


### [Endpoint](/nservicebus/endpoints/)

An Endpoint is a logical entity that communicates with other Endpoints via messaging. Each Endpoint has an identifying name and contains a collection of Message Handlers and Sagas. An Endpoint can be deployed to a number of machines and environments. Each deployment of an endpoint is an instance.


### [Endpoint Instance](/nservicebus/endpoints/)

An Endpoint Instance is a run-time object that allows interaction with the bus. Endpoint Instances are able to send, receive and publish messages. It runs associated Message Handlers and Sagas to process incoming messages. An Endpoint Instance has a single Input Queue.


### [Hosting](/nservicebus/hosting)

The act of Hosting refers to running an Endpoint Instance in some process. This can be any .NET process such as a console application, a website, or a Windows service. Multiple Endpoint instances can be hosted in a single process.


### [Transport](/transports/)

The Transport is the mechanism that NServiceBus Endpoint Instances use to communicate with each other. NServiceBus supports many different transports in a pluggable manner. For Endpoint Instances to communicate they need to share a common Transport technology.


### Input Queue

Each endpoint instance is assigned a single Input Queue. This queue can be shared among instances of same Endpoint but never with other Endpoints.


### [Publish Subscribe](/nservicebus/messaging/publish-subscribe)

Publish Subscribe is the interaction of

 * Registering interest in being notified about an event (*subscriber*).
 * That event being delivered to the endpoint that registered itself (*publisher*).


### [Handler](/nservicebus/handlers/)

A Message Handler (sometimes referred as a "Handler") is a piece of code that processes a message of a given type. Message handlers are stateless.


### [Saga](/nservicebus/sagas/)

A Saga can be thought of as a long running Handler that handles multiple Messages and shared state. It is the NServiceBus equivalent of a [Process Manager](https://www.enterpriseintegrationpatterns.com/patterns/messaging/ProcessManager.html) pattern.


### [Timeout](/nservicebus/sagas/timeouts.md)

A Timeout is a message a Saga sends to its future self to indicate the fact that some action needs to be performed. A timeout can contain state which provides the context for that action.


### [Recoverability](/nservicebus/recoverability/)

NServiceBus has retry logic which surrounds all calls to user code. This allows failing business code to be retried in a sensible way in order to resolve any interim problems (such as a database server restart). Messages that fail all retries are send to a error queue for triage for either a retry or discarding.


### [Pipeline](/nservicebus/pipeline/)

The Pipeline refers to the series of actions taken when an incoming message is processed and an outgoing message is sent.


### [Behavior](/nservicebus/pipeline/manipulate-with-behaviors.md)

A Behavior is a single step in the Pipeline.


### [Message Property Encryption](/nservicebus/security/property-encryption.md)

NServiceBus has both built in encryption and extension points to create fully customized encryption models.


### [Auditing](/nservicebus/operations/auditing.md)

Allows the forwarding of every message received by an endpoint to a configured queue.


### [Serialization](/nservicebus/serialization/)

Serialization is the process of converting an in memory object representing a message into a stream of bytes in order to transmit it via the Transport. For Endpoint Instances to communicate they need to share a common serialization technology.


### [Persistence](/persistence/)

Several NServiceBus features rely on persistent storage to function. This includes Sagas, Timeouts, Gateway, Outbox and Subscriptions (with transports that do not support Pub/Sub natively).


### [Dependency injection](/nservicebus/dependency-injection)

NServiceBus relies heavily on Containers and Dependency Injection to manage services and state.


### [Assembly Scanning](/nservicebus/hosting/assembly-scanning.md)

NServiceBus leverages assembly scanning to implement several features.


### [Logging](/nservicebus/logging/)

NServiceBus has some sensible defaults for logging built in and, for more advanced scenarios, it supports routing log events to an external logging library.


### [Outbox](/nservicebus/outbox)

An alternative to Distributed Transactions to provide exactly-once message processing semantics when accessing user data store as part of message processing.


### [Distributor](/transports/msmq/distributor/)

A load balancing tool for message distribution.


### [Idempotence](https://en.wikipedia.org/wiki/Idempotence)

The ability to call the same message handler more than once without causing inconsistent business results.

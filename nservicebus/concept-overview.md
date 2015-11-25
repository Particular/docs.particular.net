---
title: Concepts Overview
summary: A high level feature and concept overview of NServiceBus
redirects:
- nservicebus/fluent-config-api-v3-v4
- nservicebus/fluent-config-api-v3-v4-intro
---

A high level overview of the concepts, features and vernacular of NServiceBus.


### [Message](/nservicebus/messaging/)

A Message is the unit of communication for NServiceBus. Types of messages:

 * [Command](/nservicebus/messaging/messages-events-commands.md): Used to request that an action should be taken.
 * [Event](/nservicebus/messaging/messages-events-commands.md): Used to communicate that some action has taken place.

Message type is usually defined via marker interfaces. 


### [Message Conventions](/nservicebus/messaging/messages-events-commands.md)

Message Conventions is a way of defining what classes are used for Messages, Events or Commands without the use of marker interfaces. An extended use of Message Conventions, that allow a message assembly with no NServiceBus references, is called [Unobtrusive Mode](/nservicebus/messaging/unobtrusive-mode.md).


### [Header](/nservicebus/messaging/headers.md)

Extra information about a message is communicated over the transport in a serialized collection of key value pairs in a similar way to how http headers are used.


### [Publish Subscribe](/nservicebus/messaging/publish-subscribe)

Publish Subscribe is the interaction of 

 * Registering interest in being notified about an Event.
 * That Event being delivered to the endpoint that registered

A "Subscriber" lets the "Publisher" know they're interested in an Event, and the "Publisher" stores the "Subscriber" address so that it knows where to send the Event. 


### [Handler](/nservicebus/handlers/)

A Handler is code that processes a message. They are stateless and constructed on a per-message basis. 


### [Saga](/nservicebus/sagas/)

A Saga can be thought of as a long running Handler that handles multiple Messages and shared state. It is the NServiceBus equivalent of a workflow.


### [Timeout](/nservicebus/sagas/#timeouts)

Used by Sagas to indicate that a give action, and associated state, should be performed at some point in the future.


### [Retries](/nservicebus/errors/automatic-retries.md)

NServiceBus has retry logic which surrounds all calls to user code. This allows failing business code to be retried in a sensible way.


### [Error Queue](/nservicebus/errors/)

Messages that fail all retries are send to a error queue for triage for either a retry or discarding.


### [Pipeline](/nservicebus/pipeline/)

The Pipeline refers to the series of actions taken when an incoming message is processed and an outgoing message is sent. 


### [Behavior](/nservicebus/pipeline/customizing.md)

A Behavior is a single step in the Pipeline. 


### [Encryption](/nservicebus/security/encryption.md)

NServiceBus has both built in encryption and extension points to create fully customized encryption models.


### [Transport](/nservicebus/transports/)

The Transport is the mechanism that NServiceBus Endpoints use to communicate with each other. NServiceBus supports many different transports in a pluggable manner. For Endpoints to communicate they need to share a common Transport technology.


### [Auditing](/nservicebus/operations/auditing.md)

Allows the forwarding of every message received by an endpoint to a configured queue.


### [Serialization](/nservicebus/serialization/)

Serialization is the process of converting an in memory .NET object (in our case a message) into a stream of bytes in order to transmit it via the Transport. For Endpoints to communicate they need to share a common serialization language.


### [Persistence](/nservicebus/persistence/)

Several NServiceBus features rely on persistence storage to function. This includes Sagas, Timeouts, Gateway, Outbox and Subscriptions.


### [Containers](/nservicebus/containers)

NServiceBus relies heavily on Containers and Dependency Injection to manage services and state.


### Endpoint

An Endpoint is the general term used to describe a collection of configuration and functionality (handlers and sagas). It is the "processing-unit" for NServiceBus in that it sends, receives and process messages. A Send-Only endpoint is an endpoint which has no handlers or sagas and has outgoing messages. Many conventions are based on the [Endpoint Name](/nservicebus/messaging/specify-input-queue-name.md)


### [Hosting](/nservicebus/hosting)

The act of "Hosting" refers to running an Endpoint in some other process. This can be any .NET process such as a console application, a website, or a Windows service. Multiple logical Endpoints can be hosted in a single process (custom AppDomains are required to achieve this in version 4 and below).


### [Assembly Scanning](/nservicebus/hosting/assembly-scanning.md)

NServiceBus leverages assembly scanning to implement several features. 


### [Logging](/nservicebus/logging/)

NServiceBus has some sensible defaults for logging built in and, for more advanced scenarios, it supports routing log events to an external logging library.


### [OutBox](/nservicebus/outbox)

An alternative to Distributed transactions to provide reliability in Message processing. 


### [Distributor](/nservicebus/scalability-and-ha/distributor/)

A load balancing tool for message distribution.

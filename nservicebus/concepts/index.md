---
title: Messaging concepts
summary: Overview of messaging concepts
reviewed: 2020-10-15
redirects:
- nservicebus/concept-overview
---

Messaging is typically used for communication across processes, also known as communication across components. It is typically used to communicate between components within a system, as opposed to integration-based communication which is often used to communicate outside of a single system.

### Communication styles

With messaging, by nature the communication is one-way and uses the [asynchronous communication pattern](https://en.wikipedia.org/wiki/Asynchronous_method_invocation). As a result a direct response, possibly with data as a return value, is not possible with messaging.

The asynchronous communication is not to be confused with asynchronous execution, where multiple tasks are executed in parallel using multiple threads for performance improvements. The async/await concepts in .NET are an example of asynchronous execution.

### Request/Reply pattern

A possibility to overcome the one-way nature of messaging is using the [request/reply pattern](/nservicebus/messaging/reply-to-a-message.md). The result is two separate messages without blocking calls that are waiting for a response. NServiceBus uses information stored in the headers of a message to extract where to route the reply message to.

### Publish/Subscribe pattern

Instead of sending a message to a single and specific receiver, with the [publish/subscribe pattern](/nservicebus/messaging/publish-subscribe) the sender is logically unaware of potential receivers. Receivers can subscribe to messages at runtime, having the benefit that the sender does not need to be changed whenever a new subscriber is introduced.

### Callback pattern

Occasionally a scenario exists where the sender of a message requires a blocking call and waits for a response. Usually this occurs in existing systems where messaging is introduced and the user-interface is designed to wait for a response. Instead of an immediate and partial (but large) rewrite of the user-interface, the [callback pattern](/nservicebus/messaging/callbacks.md) can be used to wait for a response.

### NServiceBus concepts

Even though [NServiceBus introduces its own concepts](/nservicebus/concepts/glossary.md), many of them already existed. A good source of information is Gregor Hohpe's [Enterprise Integration Patterns](https://www.enterpriseintegrationpatterns.com/) website and accompanying book.
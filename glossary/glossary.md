---
title: Glossary
summary: Definitions of terms applicable when using NServiceBus and/or the Particular Platform
tags:
- Glossary
---

# Glossary

### Message

A **Message** is the unit of communication for NServiceBus. There are two sub-types of messages

- [Messages, Events and Commands](https://docs.particular.net/nservicebus/messaging/messages-events-commands)
- [Message Conventions](https://docs.particular.net/nservicebus/messaging/conventions)

### Logical Endpoint
An Endpoint is a logical entity that communicates with other Endpoints via Messaging. Each Endpoint has an identifying name and contains a collection of Message Handlers and Sagas. An Endpoint can be deployed to a number of machines and environments. Each deployment of an endpoint is an instance. Each Endpoint Instance has an input queue which contains messages for the Endpoint Instance to process.

- [Message Routing](https://docs.particular.net/nservicebus/messaging/routing)
- [Endpoint Naming](https://docs.particular.net/nservicebus/endpoints/specify-endpoint-name)

### Endpoint Instance

And Endpoint Instance is a physical deployment of a Logical Endpoint. It is common for each Logical Endpoint to have a single Endpoint Instance. As Endpoints need to scale-out, additional Endpoint Instances can be added. This collection of Endpoint Instances still represents a single logical Endpoint.

- [Hosting NServiceBus](https://docs.particular.net/nservicebus/hosting/)
- [Scalability and High Availability](https://docs.particular.net/nservicebus/scalability-and-ha/)

### Consumer

Each endpoint instance can be called a consumer of messages. With scaled out consumers and a brokered transport, each consumer is competing to retrieve messages from the central (brokered) queue.

### Message Handler
A message handler is a piece of code that is executed by NServiceBus when a message arrives.   

In the light of microservices a message handler could be seen as a single microservice, it is responsible for a single business issue. Where it is often said that each single microservice should be deployed independently of others, with message handlers you have the option to bundle them before deployment, but also to separate them in production for scaling out endpoint instances or setting different SLAs.

- [Message Handlers](https://docs.particular.net/nservicebus/handlers/) 

### Saga

[tbd]

### Publish/Subscribe

[tbd]

### Outbox

[tbd]

-----

and the list goes on and on...

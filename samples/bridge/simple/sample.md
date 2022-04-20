---
title: Simple Bridge Usage
summary: How to use the bridge to connect endpoints on different transports
reviewed: 2022-04-15
component: Bridge
related:
 - nservicebus/bridge
---

This sample shows how to bridge two different transports.

For this sample to work without installing any prerequisites, all endpoints use the [LearningTransport](/transports/learning/). To demonstrate that it can still bridge different transports, the LearningTransport on one side of the bridge is configured to store messages in a different location on disk.

## Projects

There are two projects on the left side, using LearningTransport. With one project on the right side, also using LearningTransport but stored in a different location. The transport on any side can be replaced by any other supported transport, without any configuration changes on the other side, except for in the Bridge.

```mermaid
graph LR
    A[LeftSender] --> C[[BRIDGE]]
    B[LeftReceiver] --> C
    C --> D[RightReceiver]
    subgraph Transport A
      A
      B
    end
    subgraph Transport B
      D
    end
```

### Bridge

The project that references `NServiceBus.Transport.Bridge` and contains configuration to bridge the two transports.

### SharedMessages

The shared message contracts used by all endpoints. It also includes a helper class to find the storage location for the LearningTransport.

### LeftSender

* Allows a user to
  * Sends the `PlaceOrder` command message to `RightReceiver`.
  * Publish a `OrderReceived`  message.

* Receives and handles the `OrderResponse` message.

### LeftReceiver

- Receives and handles the `OrderReceived` event.

### RightReceiver

* Receives and handles the `PlaceOrder` command and replies with a `OrderResponse` message.
* Receives and handles the `OrderRecevied` event.

## Code walk-through

Any project in the solution contains regular NServiceBus code that will not be discussed.

### LearningTransport as alternative transport

As mentioned, this sample uses the LearningTransport on both sides of the bridge. To simulate two different transports, the location on disk for the LearningTransport on the right side of the Bridge needs to be changed to a different location on disk.

snippet: alternative-learning-transport 

### Bridge configuration

In the bridge, every endpoint that is available in the system needs to be configured. The following shows different variations of configuring the endpoints.

#### LeftSender

The `LeftSender` endpoint is an endpoint without subscriptions to any event and can be configured the easiest way.

snippet: endpoint-adding-simple

#### LeftReceiver

The `LeftReceiver` endpoint is a subscriber to `LeftSender`, but it does not need to cross the Bridge. Therefor there's no configuration needed at this moment to specify that a publisher needs to be registered.

#### RightReceiver

The `RightReceiver` endpoint is configured as a new endpoint and the event  `Messages.Events.OrderPlaced` is registered on the publisher `LeftReceiver` using the fully qualified name. This is how events are registered if a message assembly cannot be shared with the Bridge.

snippet: endpoint-adding-register-publisher-by-string

For events in assemblies that can actually be added, it is possible to register them by type, as [described in documentation](/nservicebus/bridge/configuration.md#registering-publishers), to prevent typing mistakes. Registering by type is possible in this sample, as the `Shared` assembly has no references to any version of NServiceBus and message types are registered using [conventions](/nservicebus/messaging/conventions.md). For demo purposes the name was added as a string.

#### Add endpoints to Bridge configuration

The last step is to register all the configured transports with the bridge itself

snippet: add-transports-to-bridge

## Migrating an endpoint

As an exercise, it could be possible to try and move the `LeftReceiver` to yet another transport. This is possible using the LearningTransport and using the code from `RightReceiver` to configure it to use yet another location.

The bridge would also need to be configured so that a new `BridgeTransport` is added with the already existing `leftReceiver` on the new `BridgeTransport`. The end result would look like this:

```mermaid
graph LR
    A[LeftSender] --> C[[BRIDGE]]
    B[LeftReceiver] --> C
    C --> D[RightReceiver]
    subgraph Transport A
      A
    end
    subgraph Transport B
      D
    end
    subgraph Transport C
      B
    end    
```

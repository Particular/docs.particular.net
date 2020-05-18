---
title: Data distribution
summary: Implementing data distribution on top of NServiceBus
component: Core
reviewed: 2020-05-18
tags:
- Routing
---

DANGER: Asynchronous messaging is not an optimal solution for data distribution scenarios. It is usually better to use a dedicated data distribution technology, such as a distributed cache or distributed configuration service. This sample is meant to demonstrate the flexibility of the NServiceBus routing engine.

The sample demonstrates how NServiceBus [routing](/nservicebus/messaging/routing.md) can be used to implement data distribution scenarios. When using the built-in [publish/subscribe](/nservicebus/messaging/publish-subscribe) pattern in NServiceBus, event messages are delivered to a single instance of any subscribed endpoint. In this sample, every event message is delivered to all instances of subscribed endpoints.


## Prerequisites

Make sure MSMQ is installed and configured as described in the [MSMQ transport - MSMQ configuration](/transports/msmq/#msmq-configuration) section.


## Running the project

 1. Start all projects by pressing <kbd>F5</kbd>.
 1. The text `Press <enter> to send a message` will appear in the Client.1 and Client.2 console windows.
 1. Press <kbd>enter</kbd> a few times in both console windows.


### Verifying that the sample works correctly

 1. Notice that for each order message sent from any client, there is a single confirmation `Order ABCD accepted` displayed in either the  Client.1 or Client.2 window. The `OrderAccepted` event is always processed by a single instance of the Client endpoint.
 1. Notice that for each order message, there is an `Invalidating cache` message displayed in both client consoles (data is being distributed to all instances).


## Code walk-through

This sample contains four projects, three of which are executables and one which is a shared library.


### Client

The Client application submits the orders for processing by the back-end systems by sending a `PlaceOrder` command. Client also holds a local in-memory order cache that must be invalidated when an order is accepted by the back-end.

The client application consists of two endpoints. The main endpoint is used to send the `PlaceOrder` commands and process `OrderAccepted` events.

snippet: MainConfig

The auxiliary endpoint is used for data distribution purposes. It reacts to `OrderAccepted` events and invalidates the cache. In order to ensure each scaled out instance of Client receives its own copy of the event, the logical name of the data distribution endpoint consists of the name of the main endpoint and a suffix which is specific to a given instance. The suffix can be set in the configuration for each deployment or can be obtained from an environment (e.g. Azure role instance ID).

snippet: DistributionEndpointName

NServiceBus uses assembly scanning to load user-provided components, such as message handlers. When co-hosting two endpoints in a single process, it is important to make sure NServiceBus loads the correct components for each endpoint. In this sample, the `DataDistribution` namespace is used to mark data distribution components

snippet: DistributionEndpointTypes

NOTE: In real-world scenarios NServiceBus endpoints are scaled out by deploying multiple physical instances of a single logical endpoint to multiple machines. For simplicity, the scale out in this sample is simulated by having two separate projects, Client and Client2.


### Server

The Server application processes the `PlaceOrder` commands and publishes `OrderAccepted` events.


### Shared project

The shared project contains definitions for messages.

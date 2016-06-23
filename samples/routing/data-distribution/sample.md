---
title: Data distribution
summary: Implementing data distribution on top of NServiceBus
component: Core
tags:
- Routing
- Data distribution
---

The sample demonstrates how NServiceBus [routing](/nservicebus/messaging/routing.md) can be used to implement data distribution scenarios. When using the built-in [Publish/Subscribe]((/nservicebus/messaging/publish-subscribe)) pattern in NServiceBus, event messages are delivered to a single instance of any subscribed endpoint. In this sample every event message is delivered to all instances of subscribed endpoints.


## Prerequisites

Make sure MSMQ is set up as described in the [MSMQ Transport - NServiceBus Configuration](/nservicebus/msmq/) section. 


## Running the project

 1. Start all the projects by hitting F5.
 1. The text `Press <enter> to send a message` should be displayed in the Client.1 and Client.2 console windows. 
 1. Press `<enter>` a couple of times in both Client windows.


### Verifying that the sample works correctly

 1. Notice that for each order message sent from any client there is a single confirmation `Order ABCD accepted` displayed in either Client.1 or Client.2 window. The `OrderAccepted` event is always processed by a single instance of the Client endpoint.
 1. Notice that for each order message there is `Invalidating cache` message displayed in both client consoles (data is being distributed to all instances).


## Code walk-through

This sample contains four projects:

 * Shared - A class library containing common routing code including the message definitions.
 * Client - A console application responsible for sending the initial `PlaceOrder` message.
 * Client2 - A console application identical to Client.
 * Server - A console application responsible for processing the `PlaceOrder` command.


### Client

The Client mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. The Client also holds a local in-memory order cache that needs to be invalidated when an order is accepted by the back-end.

The client application consists of two endpoints. The main endpoint is a standard NServiceBus endpoint. It is used to send the `PlaceOrder` commands and process `OrderAccepted` events.

snippet:MainConfig

The second endpoint is used for data distribution purposes. The logical name of the data distribution endpoint consists of the name of the main endpoint and a suffix which is specific to a given instance. Such suffix can be set in the configuration for each deployment or can be obtained from an environment (e.g. Azure role instance ID).

snippet:DistributionConfig

NServiceBus uses assembly scanning to load various user-provided components such as message handlers. When co-hosting two endpoints in a single process it is important to make sure NServiceBus loads correct components to correct endpoints. In this sample `DataDistribution` namespace is used to mark data distribution components

snippet:CacheInvalidationHandler

On the endpoint configuration level this namespace is used for filtering

snippet:FilterNamespace1

snippet:FilterNamespace2

In real-world scenarios NServiceBus is scaled out by deploying multiple instances of same application binaries to multiple machines (e.g. Client in this sample). For simplicity in this sample the scale-out is simulated by having two separate applications, Client and Client2.


### Server

The Server project mimics the back-end system where orders are accepted.


### Shared project

The shared project contains definitions for messages.
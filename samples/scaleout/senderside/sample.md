---
title: 'Scale Out: Competing Consumers And Sender-Side Distribution'
summary: Scale out existing message processing either by using underlying transport's competing consumers capability or by sender-side distribution
reviewed: 2016-03-17
component: Core
tags:
- Distributor
- Scalability
related:
- nservicebus/scalability-and-ha
- nservicebus/msmq/scalability-and-ha/sender-side-distribution
- samples/scaleout/distributor
---

NOTE: This sample applies only to version 6 and above.

Sometimes a single endpoint for handling messages is not enough so there is a need to scale out. The following sample demonstrates how to use NServiceBus to scale out existing message processing either by using competing consumers approach or by distributing messages by the sender.


## Code walk-through

There are several projects in the solution.


### Server1 and Server2

These two projects are identical and they are there to show the scale out in action from within Visual Studio. In a real-world scenario there would be a single Server project that is deployed to multiple paths and/or machines.


#### Instance ID

Following snippet instructs NServiceBus to get the instance ID from the `appSettings` configuration section.

snippet:Server-Set-InstanceId

snippet:Server-InstanceId

WARNING: Never hard-code instance IDs because they are mostly operations concern and operations people should be able to modify them without the need to recompile the source code.


#### Handling code

snippet:Server-Handler


### Messages

Contains message definitions shared by the server and the clients.


### Unaware client

This project contains code for a client which is not aware of specific scale out design of the server. The client specifies only the logical routing:

snippet:Logical-Routing


### Aware client

In addition to the logical routing, the scale out-aware client uses a [instance mapping file](/nservicebus/msmq/routing.md) to tell NServiceBus about the specific instances of the Server:

snippet:File-Based-Routing

snippet:Physical-Routes


## Running the code

Start the solution with all the console applications (`Server1`, `Server2`, `UnawareClient`, `AwareClient`) as startup applications.


### Competing consumers

Go to the `UnawareClient` console an press enter a few times. `Message received.` will printed out on `Server1` and `Server2` consoles. These messages seem to be routed randomly to either of the servers and there are cases when a number of consecutive messages end up in a single server (consequence of randomness). This is because the unaware client uses the queue shared by both servers and the message is processed by the first one that retrieves it from the underlying queue.

NOTE: The competing consumers scale out approach only works with all scaled out receivers running on the same machine.


### Round robin

Now go to the `AwareClient` console and, again, press enter a few times. `Message received.` messages show up alternately in `Server1` and `Server2` windows. This is because the client is responsible for the distribution and sends its messages to the configured instance-specific queues using [sender-side distribution](/nservicebus/msmq/scalability-and-ha/sender-side-distribution.md)

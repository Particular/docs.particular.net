---
title: Competing Consumers And Sender-Side Distribution
summary: Scale out existing message processing either by using underlying transport's competing consumers capability or by sender-side distribution
reviewed: 2017-10-16
component: Core
tags:
 - Scalability
related:
 - transports/msmq/scaling-out
 - transports/msmq/sender-side-distribution
 - samples/scaleout/distributor
---

NOTE: This sample applies only to version 6 and above.

Sometimes a single endpoint for handling messages is not enough so there is a need to scale out. The following sample demonstrates how to scale out existing message processing either by using competing consumers approach or by distributing messages by the sender.


## Code walk-through

There are several projects in the solution.


### Server1 and Server2

These two projects are identical and show the scale out in action from within Visual Studio. In a real-world scenario there would be a single server project that is deployed to multiple paths and/or machines.


#### Instance ID

Following snippet instructs NServiceBus to get the instance ID from the `appSettings` configuration section.

snippet: Server-Set-InstanceId

snippet: Server-InstanceId

WARNING: Never hard-code instance IDs because they are mostly operations concern and operations people should be able to modify them without the need to recompile the source code.


#### Handling code

snippet: Server-Handler


### Messages

Contains message definitions shared by the server and the clients.


### Unaware client

This project contains code for a client which is not aware of specific scale out design of the server. The client specifies only the logical routing:

snippet: Logical-Routing


### Aware client

In addition to the logical routing, the scale out-aware client uses an [instance mapping file](/transports/msmq/routing.md) to tell NServiceBus about the specific instances of the server. This is done as follows:

snippet: File-Based-Routing

snippet: Physical-Routes


## Running the code

Start the solution with all the console applications (`Server1`, `Server2`, `UnawareClient`, `AwareClient`) as startup applications.


### Competing consumers

Go to the `UnawareClient` console and press enter a few times. `Message received.` will printed out on the `Server1` and `Server2` consoles. These messages seem to be routed randomly to either of the servers and there are cases when a number of consecutive messages end up in a single server (consequence of randomness). This is because the unaware client uses the queue shared by both servers and the message is processed by the first one that retrieves it from the underlying queue.

WARNING: The competing consumers scale out approach only works with all scaled out receivers running on the same machine.


### Round robin

In the `AwareClient` console, press enter a few times to generate message load. This results in both the `Server1` and `Server2` consoles processing the generated message load in an alternating fashion. This is because the messages are sent in a round-robin fashion to `Server1` and `Server2` by the client using the [sender-side distribution feature](/transports/msmq/sender-side-distribution.md).

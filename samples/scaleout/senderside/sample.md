---
title: Sender-Side Distribution
summary: Scale out MSMQ message processing with sender-side distribution
reviewed: 2019-09-03
component: MsmqTransport
related:
 - transports/msmq/scaling-out
 - transports/msmq/sender-side-distribution
 - samples/scaleout/distributor-upgrade
---

NOTE: This sample is only relevant for the [MSMQ transport](/transports/msmq/). The other broker-based transports scale out using the [competing consumer pattern](/nservicebus/architecture/scaling.md#scaling-out-to-multiple-nodes-competing-consumers). The functionality in this sample is available for NServiceBus version 6 and above. 

Sometimes a single endpoint for handling messages is not enough, so there is a need to scale out. The following sample demonstrates how to scale out existing MSMQ message processing by distributing messages on the sender side.


## Code walk-through

There are several projects in the solution.


### Server1 and Server2

These two projects are identical and show the scale out in action from within Visual Studio. In a real-world scenario, there would be a single server project that is deployed to multiple paths and/or machines.


#### Instance ID

The following snippet instructs NServiceBus to get the instance ID from the `appSettings` configuration section:

snippet: Server-Set-InstanceId

snippet: Server-InstanceId

WARNING: Never hard-code instance IDs because they are mostly an operations concern, and operations staff should be able to modify them without the need to recompile the source code.


#### Handling code

snippet: Server-Handler


### Messages

Contains message definitions shared by the server and the client.


### Client

The client configures the logical routing:

snippet: Logical-Routing

In addition to the logical routing, the client also uses an [instance mapping file](/transports/msmq/routing.md) to tell NServiceBus about the specific instances of the server. This is done as follows:

snippet: File-Based-Routing

snippet: Physical-Routes


## Running the code

Start the solution with all the console applications (`Server1`, `Server2`, `Client`) as startup applications.

In the `Client` console, press enter a few times to generate message load. This results in both the `Server1` and `Server2` consoles processing the generated message load in an alternating fashion. This is because the messages are sent in a round-robin fashion to `Server1` and `Server2` by the client using the [sender-side distribution feature](/transports/msmq/sender-side-distribution.md).
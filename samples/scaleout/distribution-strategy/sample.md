---
title: Custom distribution strategy
summary: Use custom distribution strategy to influence how messages are sent to processor instances
component: Core
tags:
- Distributor
- Scalability
related:
- nservicebus/scalability-and-ha
- samples/scaleout/senderside
---

NOTE: This sample applies only to version 6 and above.

[The sender side distribution sample](/samples/scaleout/senderside) shows how clients can distribute messages among a number of server instance. Sometimes the built-in round-robin distribution strategy is not feasible though. This sample shows how to build a custom distribution strategy that takes *weight* (an approximation of processing power) into account when deciding where to route a given message.

## Code walk-through

There are several projects in the solution.


### Server1 and Server2

These two projects are identical and they are there to show the scale out in action from within the Visual Studio. In a real-world scenario there would be a single Server project that is deployed to multiple paths and/or machines.


#### Instance ID

Following snippet instructs NServiceBus to get the instance ID from the `appSettings` configuration section.

snippet:Server-Set-InstanceId

snippet:Server-InstanceId

WARNING: Never hard-code instance IDs because they are mostly operations concern and operations people should be able to modify them without the need to recompile the source code.


#### Handling code

snippet:Server-Handler


### Messages

Contains message definitions shared by the server and the clients.

### Client

The client uses a routing file to tell NServiceBus about the specific instances of the Server and their relative weights. In this example *instance2* is twice as likely to receive messages.

snippet:File-Based-Routing

snippet:Physical-Routes

Following code registers the custom distribution strategy

snippet:Logical-Routing

#### Weighted distribution

Because the distribution strategy is invoked for each outgoing message, it is important to keep it as fast as possible, sometimes trading off accuracy for performance.

With that advice in mind the distribution algorithm in this sample is not based on the random value from the sum of weights (which requires iterating over instance list twice). Rather than that, it uses the weight to compute the probability of rotating the currently selected instance. This causes bursts of messages being sent to each instance where the burst length is random but influenced by the weight of that instance. 

snippet:Should-Rotate


## Running the code

Start the solution with all the console applications (`Server1`, `Server2`, `Client`) as startup applications.

### Weighted round robin

Go to the `Client` console and press enter a few times. `Message received.` messages show up in both server console windows. Notice that more (roughly twice as many) messages appear in `Server2` window. 
---
title: Publish Subscribe
summary: Subscribers tell the publisher they&#39;re interested. Publishers store addresses for sending messages.
tags:
- Publish Subscribe
- Messaging Patterns
redirects:
- nservicebus/how-pub-sub-works
related:
- samples/pubsub
- samples/step-by-step
---

The primary concept is **subscribers let the publisher know they're interested, and the publisher stores their addresses so that it knows where to send which message**.


## Before we get started...

One of the common assumptions about pub/sub messaging is that it involves physical one-to-many communication. The only thing is, that at the physical level pub/sub isn't that interesting. It becomes valuable when publishing logical events from one logical area of responsibility to other logically interested parties.

NServiceBus' infrastructure pieces handle the physical distribution and one-to-many message dispatch many look for in pub/sub, but these are quite transparent to the programming model. Let's take a look at the overlay of logical pub/sub and physical distribution, one step at a time.

![logical pub/sub and physical distribution 1](nservicebus-pubsub-1.png)

The above diagram shows us one logical publisher P1, and two logical subscribers SA and SB. Each has a number of physical nodes (colored in blue) and some NServiceBus infrastructure (colored in orange). For now, we're going to assume that both SA and SB are already subscribed, each specifying the left port of its distributor as its public endpoint.


## What happens when we publish

When a node in the logical publisher P1 goes to publish a message, here's what happens:

![logical pub/sub and physical distribution 2](nservicebus-pubsub-2.png)

When requested by applicative logic to publish a message, the NServiceBus infrastructure contacts its configured subscriptions database, finds all the subscriber endpoints registered for the given message type, and dispatches a physical message to each one.

Since one-way messaging is used to dispatch physical messages, even if one of the subscriber endpoints is offline or otherwise unavailable, this does not cause the publishing thread to block. The message is stored in the sending machine's outgoing queue (for a configurable period of time), while the messaging infrastructure attempts to deliver the message to its destination.


## What the distributor does

All the Distributor does at this point is forward the message it receives to another node.

![logical pub/sub and physical distribution 3](nservicebus-pubsub-3.png)

You can think of the Distributor as something like a load balancer. It distributes the received messages to a number of other machines. This kind of physical one-to-many communication is needed for scaling out the number of machines running for a given subscriber, but doesn't actually entail any pub/sub. Each subscriber gets its own Distributor and each of them decides independently to which machine it passes its messages.

See [more information on the distributor](/nservicebus/scalability-and-ha/distributor/).


## The same for any publisher node

It doesn't matter which node in the publisher is publishing a message, the same process happens.

![logical pub/sub and physical distribution 4](nservicebus-pubsub-4.png)

What this means is that you can scale out the number of publishing nodes just by making use of a database for storing subscriptions, with no need for a distributor. When using the generic NServiceBus Host process you get this by default in its production profile.
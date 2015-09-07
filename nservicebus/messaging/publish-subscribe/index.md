---
title: Publish Subscribe
summary: Subscribers tell the publisher they&#39;re interested. Publishers store addresses for sending messages.
tags:
- Publish Subscribe
- Messaging Patterns
redirects:
- nservicebus/how-pub-sub-works
---

Now that you've seen publish/subscribe in action, let's take a look behind the curtains and see what's going on.

If you haven't seen the [publish/subscribe sample](/samples/pubsub/) yet take a minute to walk through or start by [creating a new project using NServiceBus](/samples/step-by-step/).

The main thing to understand is this: subscribers let the publisher know they're interested, and the publisher stores their addresses so that it knows where to send which message. It's fairly straight-forward, once you know how it all works.

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

## What is auto subscribe

The default mode auto subscribe. When a subscriber endpoint is started it determines to which events it needs to subscribe. It then sends sends subscribtion messages to the dependent publishers. The publishers update their subscription data.

Each time the subscriber is restarted it sends these subscription messages. When you monitor the queing infrastructure you see these subscription messages flowing.

## What happens when a subscriber stops

The default mode is auto subscribe. The subscriber endpoint will never unsubscribe. When the subscriber endpoint stops, it is still registered at the publisher to receive events. The publisher still sends events to the queue of the stopped subscriber. When the subscriber is started it will consume the messages from its queue. The subscriber will never loose an event.

## Can I manually subscribe/unsubscribe

If auto subscribe is unwanted then this can be disabled during initialization. How this is done varies per NServiceBus version. You can explicitly subscribe and unsubscribe via the bus instance. See [How to Publish/Subscribe to a Message](how-to-pub-sub.md) for an example.


## What happens when a subscriber uninstalls

When a subscriber is uninstalled then it will not unsubscribe at the publisher. The reason for doing this is not to loose events when you upgrade an endpoint by uninstalling the current version and then installing the new version.


## Manually removing subscription entries

Manually removing a subscription entry is sometimes needed when you want to decommission or migrate an endpoint that is subscribed to a publisher. 

There are different subscription storage providers but all store a list of subscription items where each item contain the following values:

- SubscriberEndpoint
- MessageType
- Version
- TypeName


Perform these tasks to manually remove subscription entries:

- Stop the publisher endpoint
- Open the subscription storage of the publisher
- Open each subscription item and delete it when its **SubscriberEndpoint** value matches
- Start the publisher endpoint

NOTE: When you start the subscriber endpoint then it will automatically subscribe again.


## How do I decommission a subscriber

Currently it is required to manually update the subscription storage of the publisher by deleting the subscriber endpoint specific entries first and then restarting the publisher. You can then remove the queue from the subscriber.


## Migrating an endpoint to a different machine

Moving an endpoint to a different machine is complex when you do not use a central queue infrastructure.

NOTE: Do NOT install the endpoint yet on the new machine as this will subscribe it to publishers and start receiving events.

1. Stop all endpoints that send messages to this endpoint including the publishers.
2. Let the endpoint process all messages in its queues.
3. Stop the endpoint
4. Install the endpoint on the new machine
5. Update the configuration files so that message are send to the queue on the new machine.
6. Update the subscription storage of the dependant publishers so that they will be sending messages to the new machine.
7. Start the migrated endpoint on the new machine
8. Start the endpoints

All message should now be send correctly except replies.

NOTE: If the endpoints receives messages from other endpoints via `Bus.Reply` then these messages are send to the previous queue. This is because the reply address is included in the message if that message was sent from the previous machine.

## What the distributor does

All the Distributor does at this point is forward the message it receives to another node.

![logical pub/sub and physical distribution 3](nservicebus-pubsub-3.png)

You can think of the Distributor as something like a load balancer. It distributes the received messages to a number of other machines. This kind of physical one-to-many communication is needed for scaling out the number of machines running for a given subscriber, but doesn't actually entail any pub/sub. Each subscriber gets its own Distributor and each of them decides independently to which machine it passes its messages.

See [more information on the distributor](/nservicebus/scalability-and-ha/distributor/).

## The same for any publisher node

It doesn't matter which node in the publisher is publishing a message, the same process happens.

![logical pub/sub and physical distribution 4](nservicebus-pubsub-4.png)

What this means is that you can scale out the number of publishing nodes just by making use of a database for storing subscriptions, with no need for a distributor. When using the generic NServiceBus Host process you get this by default in its production profile.


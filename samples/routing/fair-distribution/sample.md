---
title: Fair load distribution
summary: Implementing fair load distribution for heterogeneous scaled-out endpoints
component: Core
reviewed: 2016-06-21
tags:
- Routing
- Distribution
- DistributionStrategy
---


## Running the project

 1. Start all the projects by hitting F5.
 1. The text `Press <enter> to send a message` should be displayed in the Client's console window. 
 1. Hit `<enter>` several times to send some messages.


### Verifying that the sample works correctly

 1. Notice more messages are being send to Server.1 compared to Server.2
 1. Use a [MSMQ viewing tool](/nservicebus/msmq/viewing-message-content-in-msmq.md) to view the queue contents.
 1. Keep hitting `<enter>` and observer number of messages in Server.1 and Server.2 queues.
 1. Notice that although Server.2 processes messages 50% slower than Server.1, numbers of messages in both queues are almost equal.


## Code walk-through

This sample contains four projects:

 * Shared - A class library containing common routing code including the message definitions.
 * Client - A console application responsible for sending the initial `PlaceOrder` message.
 * Server - A console application responsible for processing the `PlaceOrder` command.
 * Server2 - A console application identical to Server.


### Client

The Client does not store any data. It mimics the front-end system where orders are submitted by the users and passed via the bus to the back-end. Client routing is configured to send `PlaceOrder` commands to two instances of `Server` endpoint:

snippet:Routing

Following code enables fair load distribution (as opposed to default round-robin algorithm):

snippet:FairDistributionClient


### Server

The Sales project mimics the back-end system where orders are accepted. Apart from the standard NServiceBus configuration it enables the flow control feature:

snippet:FairDistributionServer

Sales endpoint is scaled out via `Server` and `Server2` projects. In real-world scenario there would be a single project deployed to multiple virtual machines.


### Shared project

The shared project contains definitions for messages and the custom routing logic.


### Marking messages

All outgoing messages are marked with sequence numbers. Separate sequences are maintained for each downstream queue.

snippet:MarkMessages


### Acknowledging message delivery

Every N messages the downstream endpoint instance sends back ACK message containing the biggest sequence number it processed so far. ACKs are sent separately to each upstream endpoint instance.

snippet:ProcessMarkers 


### Processing acknowledgements 

When the ACK message is received, the upstream endpoint can calculate the number of messages that are currently in-flight between itself and the downstream endpoint.

snippet:ProcessACKs


### Smart routing

The calculated number of in-flight messages can be used to distribute messages in such a way that all instances of downstream scaled-out endpoint have similar number of messages in their input queues.

snippet:GetLeastBusy
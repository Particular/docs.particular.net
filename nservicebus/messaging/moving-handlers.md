---
title: Moving Handlers
summary: Guidelines for choosing a way how ot move handlers between the endpoints
reviewed: 2021-09-01
component: Core
isLearningPath: true
---

As the distributed systems evolve, sometimes there is a need to change which Endpoint handle which message type. During development phase such changes are relativelly straightforward, but when the system is in production additional consideration need to be given to the messages that are in-flight. 

## How to move Handlers in between Endpoints

To move a handler from SourceEndpoint to a DestinationEndpoint the following steps needs to be taken

 - Copy the handler into DestinationEndpoint and deploy that endpoints. Now both endpoints are handling the same message type.
 - Make sure that new Endpoint recieves the messages that it should be handling. From now on all newly send messages would only go to the DestinationEndpoint.
 - After ensuring that the SourceEndpoint does not have any remaining messages of the type that should be processed by DestinationEndpoint, delete the handler from SourceEndpoint and deploy it.

### How to ensure that the messages get to the DestinationEndpoint

This depends on if the message is a command or an event. 

When the handler process events, it is important for the DestinationEndpoint to subscribe to that event. That will ensure that this event is delievered to DestinationEndpoint. Followed by that SourceEndpoint should unsubscribe from that event to ensure that no new events are delievered to the SourceEndpoint queue.

When the handler process commands, then every piece of code that sends that command needs to be updated to send them to a queue of DestinationEndpoint. In the meantime the SourceEndpoint handler may be changed to [forward the messages to the new destination](/nservicebus/messaging/forwarding.md). 

### How to handle error messages that were discovered after removing the handler

When a set of messages are found that were send to a SourceEndpoint and a handler was already removed, a [retry redirect](/servicepulse/redirect.md) could be set up to redirect every failed message to a queue of a DestinationEndpoint. After retrieving those messages, the redirect retry should be removed. 

## Why to move handlers between endpoints

### Throughput limitations

It may happen that Endpoint, which handle many message types, can't keep up with the volume of messages that it gets. One of the solution would be to separate the handlers to divide the load into more Endopoints. 

### Different SLA'a

When Endpoint process many different message types, sometimes some of the messages should be processed faster than the others, or service level agreement for given message type could be different than the others. At those cases the easiest solution is to move processing of that message to a separate Endpoint in which case those message types don't have to compete with other message types in the queue. 



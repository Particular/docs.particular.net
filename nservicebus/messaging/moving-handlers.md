---
title: Moving Handlers
summary: Guidelines on how to move handlers between the endpoints
reviewed: 2021-09-01
component: Core
isLearningPath: true
---

As distributed systems evolve, the need may arise to change which endpoint handles a message. During the development phase, such changes are relatively straightforward, but when the system is in production additional considerations need to be given to messages that are in-flight. 

## How to move Handlers in between Endpoints

To move a handler from `SourceEndpoint` to a `DestinationEndpoint`, the following steps need to be taken:

 - Copy the handler into `DestinationEndpoint` and deploy that endpoint. Now both endpoints are handling the same message type.
 - Make sure that `DestinationEndpoint` receives the messages that it should be handling. From now on all newly sent messages should only go to the DestinationEndpoint.
 - After ensuring that the `SourceEndpoint` does not have any remaining messages of the type that should be processed by `DestinationEndpoint`, delete the handler from `SourceEndpoint` and deploy it.

### How to ensure that the messages get to the DestinationEndpoint

This depends on whether the message is a command or an event. 

When the handler process events, it is important for the `DestinationEndpoint` to subscribe to that event. That will ensure that this event is delivered to `DestinationEndpoint`. Followed by that, `SourceEndpoint` should unsubscribe from that event to ensure that no new events are delivered to the `SourceEndpoint` queue.

When the handler processes commands, every piece of code that sends that command needs to be updated to send them to a queue of `DestinationEndpoint`. In the meantime, the `SourceEndpoint`-handler may be changed to [forward the messages to the new destination](/nservicebus/messaging/forwarding.md). 

### How to handle error messages that were discovered after removing the handler

When a set of messages are found that were sent to a `SourceEndpoint` and a handler was already removed, a [retry redirect](/servicepulse/redirect.md) could be set up to redirect every failed message to a queue of a `DestinationEndpoint`. After retrieving those messages, the redirect retry should be removed. 

## Reasons to move handlers between endpoints

### Throughput limitations

It may happen that an endpoint that handles multiple message types, can't keep up with the volume of messages that it receives. One of the solutions would be to separate high throughput handlers into separate endpoints, to divide the load into more endpoints. 

### Different SLA's

When an endpoint processes multiple message types, sometimes some of the messages should be processed faster than others, or service level agreements for a given message type could differ from others. In those cases, the easiest solution is to move the processing of that message to a separate endpoint so that those message types don't have to compete with other message types in the queue. 



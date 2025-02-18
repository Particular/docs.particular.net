---
title: Moving Handlers
summary: Guidelines on how to move handlers between the endpoints
reviewed: 2024-08-05
component: Core
isLearningPath: true
---

As distributed systems evolve, the need may arise to change which endpoint handles a specific message type. During the development phase, such changes are relatively straightforward. But when the system is in production, additional considerations are necessary for in-flight messages (i.e., those that have been sent but have not yet been consumed).

## How to move handlers between endpoints

To move a handler from `SourceEndpoint` to a `DestinationEndpoint`, take the following steps:

- Copy the handler into `DestinationEndpoint` and deploy that endpoint. Now, both endpoints handle the same message type.
- Make sure that `DestinationEndpoint` receives the messages that it should be handling. From now on, all newly sent messages should only go to the `DestinationEndpoint`.
- After ensuring that the `SourceEndpoint` has no remaining messages of the type that should be processed by `DestinationEndpoint`, delete the handler from `SourceEndpoint` and deploy it.

### How to ensure that the messages get to the `DestinationEndpoint`

This depends on whether the message is a command or an event.

When the handler processes events, it is important for the `DestinationEndpoint` to subscribe to them. This ensures that the event is delivered to `DestinationEndpoint`. After that, `SourceEndpoint` should unsubscribe from the event to ensure no new events are delivered to the `SourceEndpoint` queue.

When the handler processes commands, every piece of code that sends the command must be updated to send it to a queue of `DestinationEndpoint` rather than `SourceEndpoint`. In the meantime, the `SourceEndpoint` handler may be changed to [forward the messages to the new destination](/nservicebus/messaging/forwarding.md).

### How to handle error messages that were discovered after removing the handler

When a set of messages is found that was sent to a `SourceEndpoint` and a handler was already removed, a [retry redirect](/servicepulse/redirect.md) can be set up to redirect every failed message to the `DestinationEndpoint` queue. After retrying those messages, the redirect retry should be removed.

## Reasons to move handlers between endpoints

### Throughput limitations

An endpoint that handles multiple message types may be unable to keep up with the volume of messages it receives. One way to mitigate this is to separate high-throughput handlers into separate endpoints, i.e., to divide the load into more endpoints.

### Different SLAs

When an endpoint processes multiple message types, some of the messages might need to be processed faster than others; that is, service level agreements for a given message type could differ from others. In those cases, a simple solution is to move the processing of that message to a separate endpoint so that those message types don't have to compete with other message types in the queue.

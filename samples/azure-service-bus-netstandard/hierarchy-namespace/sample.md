---
title: Azure Service Bus Hierarchy Namespace Sample
summary: Demonstrates how to use hierarchical namespaces with Azure Service Bus
reviewed: 2026-02-05
component: ASBS
related:
- transports/azure-service-bus/configuration
---

## Prerequisites

include: asb-connectionstring-xplat

## Code walk-through

This sample shows a usage of hierarchy namespace with Azure Service Bus. In the sample there are three endpoints:
- `SenderAndPublisher` - belonging to a hierarchy (`my-hierarchy`), sends messages and publishes an event,
- `Receiver1` - belonging to the same hierarchy, handles messages and subscribes to the event,
- `Receiver2` - outside of the hierarchy, handles messages and subscribes to the event.

There are two types of messages that `SenderAndPublisher` is going to send:
- `RegularMessage`,
- `ExcludedMessage`, that is excluded from the hierarchy with the following code:

snippet: excludedMessage

### Scenario 1 - regular message

`SenderAndPublisher` sends a `RegularMessage` to both receivers. It is sent successfully to `Receiver1`, because it belongs to the same hierarchy. It is not sent to `Receiver2`, because it is outside of the hierarchy. The following message is included in the exception:

`The messaging entity 'sb://xxx.servicebus.windows.net/my-hierarchy/Samples.ASBS.HierarchyNamespace.Receiver2' could not be found.`

### Scenario 2 - excluded message

`SenderAndPublisher` sends an `ExcludedMessage` to both receivers. It is sent successfully to `Receiver2`, because this type of message is excluded from the hierarchy and `Receiver2` is outside of the hierarchy. It is not sent to `Receiver1`, because it is in the hierarchy. The following message is included in the exception:

`The messaging entity 'sb://xxx.servicebus.windows.net/Samples.ASBS.HierarchyNamespace.Receiver1' could not be found.`

### Scenario 3 - event

`SenderAndPublisher` publshes a `SampleEvent`. Both receivers are subscribed to this type of event. Only `Receiver1` receives it though, because it is in the same hierarchy as `SenderAndPublisher`. Since `Receiver2` is outside of the hierarchy, that event is not delivered there.

 Note that there are no exceptions while publishing an event, even though it is not delivered to `Receiver2` that has a handler for this kind of events.

### Transport configuration

snippet: config

Two endpoints (`SenderAndPublisher` and `Receiver1` are configured to be included in the hierarchy `my-hierarchy`). `Receiver2` is not included in the hierarchy.

snippet: namespaceOptions

This is necessary to include an endpoint in a hierarchy.

## Viewing messages in-flight

The following queues and topics for the endpoints can be seen in the Azure Portal or a third-party tool:

- `my-hierarchy/samples.asbs.hierarchynamespaceescape.senderandpublisher`
- `my-hierarchy/samples.asbs.hierarchynamespaceescape.receiver1`
- `samples.asbs.hierarchynamespaceescape.receiver2`
- `my-hierarchy/shared.sampleevent`
- `shared.sampleevent`
- `error`

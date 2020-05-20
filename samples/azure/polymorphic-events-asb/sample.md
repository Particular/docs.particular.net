---
title: Polymorphic events with the Azure Service Bus transport
component: ASB
reviewed: 2020-05-20
related:
 - transports/azure-service-bus
 - nservicebus/messaging/publish-subscribe
 - nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed
 - samples/azure/azure-service-bus
redirects:
 - samples/azure/polyorphic-events-asb
---

include: legacy-asb-warning

Note: For true polymorphic events support, use the `ForwardingTopology`. This sample should be used only if `EndpointOrientedTopology` is required.


## Prerequisites

include: asb-connectionstring


include: asb-transport


## Code walk-through

This sample has two endpoints:

 * `Publisher` publishes `BaseEvent` and `DerivedEvent` events.
 * `Subscriber` subscribes to and handles `BaseEvent` and `DerivedEvent` events.

`DerivedEvent` derives from `BaseEvent`. The difference between the two events is the `Data` property provided with `DerivedEvent`.

snippet: BaseEvent

snippet: DerivedEvent


## Publisher

The `Publisher` will publish an event of type `BaseEvent` or `DerivedEvent` based on the input it receives from the console.


## Subscriber

By default, all events handled in `Subscriber` will be auto-subscribed. Default topology subscription behavior will create two subscriptions, one for each event.

![](images/subscriptions.png)


### Auto-subscription behavior

Normally, this would be fine. Not so with ASB transport and polymorphic events. Each subscription is filtering messages based on the `NServiceBus.EnclosedMessageTypes` header. When an event of `BaseType` is published, it's going only into `Samples.ASB.Polymorphic.Subscriber.BaseEvent` subscription:

![](images/baseevent.published.png)

NOTE: In version 7 and above of the transport, polymorphic events are supported with auto-subscription turned on when `ForwardingTopology` is used.

But whenever `DerivedEvent` event is published, both `Samples.ASB.Polymorphic.Subscriber.BaseEvent` and `Samples.ASB.Polymorphic.Subscriber.DerivedEvent` subscriptions get a copy of that message:

![](images/derivedevent.published.png)

Since `DerivedEvent` implements `BaseEvent`, it's `NServiceBus.EnclosedMessageTypes` header will contain both types:

```
Events.DerivedEvent, Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;
Events.BaseEvent, Shared, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
```

Both filters will pick the `DerivedEvent` message, causing duplicate delivery to the `Subscriber`. The NServiceBus `Subscriber` endpoint will invoke handlers for each type that message implements. The end result will be multiple invocations for the same message.

snippet: PublisherOutput

snippet: SubscriberOutput


### How to restrict the message handlers

To address this in general and allow proper handling of polymorphic events, `Subscriber` has do the following:

 1. Disable automatic subscription.
 1. Subscribe explicitly to the base events only of polymorphic events.
 1. Subscribe explicitly to the non-polymorphic events it's interested in.

snippet: DisableAutoSubscripton

When an event is a polymorphic event, such as `DerivedEvent`, the endpoint will subscribe to the **base event** only.

snippet: ControledSubscriptions

For this sample, configuring `Subscriber` as described above will create the topology that only has the `BaseEvent` subscription serving as "catch-all".

![](images/single.subscription.png)

The sample now adheres to the expected polymorphic message handling

snippet: PublisherOutput-from-sample

snippet: SubscriberOutput-from-sample

partial: RegisterPublisherNames

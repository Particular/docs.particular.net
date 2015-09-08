---
title: Controlling what is subscribed
summary: When applying the publish-subscribe pattern there are several ways to control what messages are subscribed to
tags: []
---


## Automatic subscriptions

NServiceBus will automatically setup subscriptions for you. Messages matching the following criteria will be auto subscribed at startup.

 1. Defined as a event either using `IEvent` or by the `.DefiningEventsAs` convention.
 1. At least one [message handler and/or saga](/nservicebus/handlers/) exists for the given message
 1. Has routing specified. Note that this only applies to transports that don't support publish-subscribe natively. 


### Routing configuration needed

If your selected transport doesn't natively supports publish-subscribe you need to specify the address of the publisher for each event. This is done by [specifying a message owner in the message endpoint mappings](/nservicebus/messaging/message-owner.md). For example:

<!-- import endpoint-mapping-appconfig -->


### Exclude sagas from auto subscribe

In version 3 and lower events that are only handled by sagas are not subscribed to by default. In version 4 and higher sagas are treated the same as handlers and will cause an endpoint to subscribe to a given event. You can opt-in to the old exclude saga event handling behavior using:

<!-- import DoNotAutoSubscribeSagas -->


### Auto subscribe to plain messages

Before version 4 all messages not defined as a command using `ICommand` or the `.DefiningCommandsAs` convention where automatically subscribed. You can opt-in to the old behavior using:

<!-- import AutoSubscribePlainMessages -->


## Disabling auto-subscription

You can also choose to **not** have the infrastructure automatically subscribe using the configuration API

<!-- import DisableAutoSubscribe -->


## How to manually subscribe to a message?

To manually subscribe and unsubscribe from a message:

<!-- import ExplicitSubscribe -->
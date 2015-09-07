---
title: How to Publish/Subscribe to a Message
summary: Publish and subscribe to messages using NServiceBus, automatically and manually.
tags: []
redirects:
- nservicebus/how-to-pub-sub-with-nservicebus
---

## How to publish a message?

<!-- import InstancePublish -->

If you are using interfaces to define your event contracts you need to set the message properties by passing in a lambda. NServiceBus will then generate a proxy and set those properties. 

<!-- import InterfacePublish -->


## Automatic subscriptions

NServiceBus will automatically setup subscriptions for you. Messages matching the following criteria will be auto subscribed at startup.

1. Defined as a event either using `IEvent` or by the `.DefiningEventsAs` convention.
2. At least one [message handler and/or saga](/nservicebus/handlers/) exists for the given message
3. Has routing specified. Note that this only applies to transports that don't support Pub/Sub natively. Examples are Msmq, Sqlserver and Azure Storage Queues. See below for more details.


### Routing configuration needed

If your selected transport doesn't natively supports pub/sub you need to specify the address of the publisher for each event. This is done using the message endpoint mappings as shown below.

<!-- import endpoint-mapping-appconfig -->


#### Exclude sagas from auto subscribe

Before Version 4 events that where only handled by sagas where not subscribed to by default. You can opt-in to the old behavior using:

<!-- import DoNotAutoSubscribeSagas -->


#### Auto subscribe to plain messages

Before Version 4 all messages not defined as a command using `ICommand` or the `.DefiningCommandsAs` convention where automatically subscribed. You can opt-in to the old behavior using:

<!-- import AutoSubscribePlainMessages -->


### Disabling auto-subscription

You can also choose to **not** have the infrastructure automatically subscribe using the configuration API

<!-- import DisableAutoSubscribe -->


## How to manually subscribe to a message?

To manually subscribe and unsubscribe from a message:

<!-- import ExplicitSubscribe -->

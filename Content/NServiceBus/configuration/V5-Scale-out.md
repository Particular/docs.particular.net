---
title: Configuration API Scale out settings for broker scenarios in V5
summary: Configuration API Scale out settings for broker scenarios in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

When the endpoint transport is a broker based transport and we want to install more then one instance of the same endpoint we can control the scale out behavior of the endpoint via the `ScaleOut` method of the `BusConfiguration` class:

* `UseUniqueBrokerQueuePerMachine`: Instructs the broker based transports to use a separate queue per endpoint when running on multiple machines. This allows clients to make use of callbacks. This setting is the default.
* `UseSingleBrokerQueue`: Instructs the broker based transports to use a single queue for the endpoint regardless of which machine its running on. This is suitable for backend processing endpoints and is the default for the As_aServer role.  Clients that needs to make use of callbacks needs to make sure that this setting is off, via the `UseUniqueBrokerQueuePerMachine` method, since they need to have a unique input queue per machine in order to not miss any of the callbacks.
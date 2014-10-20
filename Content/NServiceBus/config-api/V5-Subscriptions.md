---
title: Configuration API Subscriptions in V5
summary: Configuration API Subscriptions
 in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

Using the `AutoSubscribe` methos it is possible to control some options of the message subscriptions engine:

* `DoNotRequireExplicitRouting`: Allows another endpoint to subscribe to messages addressed to the current endpoint;
* `DoNotAutoSubscribeSagas`: Turns off auto subscriptions for sagas. Sagas where not auto subscribed by default before V4;
* `AutoSubscribePlainMessages`: Turns on auto-subscriptions for messages not marked as commands. This was the default before V4;
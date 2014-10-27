---
title: Configuration API Message Handlers Order in V5
summary: Configuration API Message Handlers Order in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

If a single endpoint contains multiple handlers that are registered to handle the same message type, or a base classe of the incoming message type, whether required it is possible to specify the order in which handlers needs to be invoked each time a specific message is received:

* `LoadMessageHandlers`: allow to specify the order in which handlers of a given message should be invoked;
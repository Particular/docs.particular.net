---
title: Delayed Delivery
summary: Describes the native delayed delivery implementation in the RabbitMQ transport
component: Rabbit
versions: '[4,]'
---

Starting with Version 4.TBD, the transport no longer relies on the [timeout manager](/nservicebus/messaging/timeout-manager.md) to implement [delayed delivery](/nservicebus/messaging/delayed-delivery.md). The transport now creates an infrastructure inside the broker to natively handle delaying messages.


## How it works


## Settings


### Disable the timeout manager


### All endpoints support native delays

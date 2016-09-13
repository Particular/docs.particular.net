---
title: Callback support
reviewed: 2016-08-30
component: Rabbit
reviewed: 2016-08-30
versions: '[2,]'
related:
 - nservicebus/messaging/callbacks
---

When scaling out an endpoint using the RabbitMQ transport, any of the endpoint instances can consume messages from the same shared broker queue. However, this behavior can cause problems when dealing with callback messages because the reply message for the callback needs to go to the specific instance that requested the callback.


partial: queue
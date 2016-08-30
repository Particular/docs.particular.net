---
title: RabbitMQ Transport Callback support
reviewed: 2016-08-30
component: Rabbit
reviewed: 2016-08-30
---

When scaling out an endpoint using the RabbitMQ transport, any of the endpoint instances can consume messages from the same shared broker queue. However, this behavior can cause problems when dealing with callback messages because the reply message for the callback needs to go to the specific instance that requested the callback.


partial: queue


## CallbackReceiverMaxConcurrency

By default, 1 dedicated thread is used for the callback receiver queue. To add more threads, due to a high rate of callbacks, use the following:

snippet:rabbitmq-config-callbackreceiver-thread-count
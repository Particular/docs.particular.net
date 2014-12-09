---
title: RabbitMQ configuration settings 
summary: The various ways to customize the RabbitMQ transport
tags:
- RabbitMQ
- Transports
---

## Configuring RabbitMQ transport to be used 

To make NServiceBus use RabbitMQ as the underlying transport you need to add this to your configuration:

<!-- import rabbitmq-config-basic -->

In order to work the transport needs to connect the the RabbitMQ broker. By default the transport will look for a connection string called `NServiceBus/Persistence` in your app.config.

A typical connection string would look like this:

<!-- import rabbitmqconnectionstring -->

In the above sample we tell the transport to connect to the RabbitMQ broker running at the machine `broker1`.

Below is the full list of connection string options. Note that you needs to separate them with a `;`.

* `Port`: The port where the broker listens. Defaults to `5672`
* `VirtualHost`: The [virtual host](https://www.rabbitmq.com/access-control.html) to use. Defaults to `/`
* `UserName`: The username when connecting. Defaults to `guest`
* `Password`: The password when connecting. Defaults to `guest`
* `RequestedHeartbeat`: The interval for the heartbeats between the client and the server. Defaults to `5` seconds
* `PrefetchCount`: The number of messages to [prefetch](http://www.rabbitmq.com/consumer-prefetch.html) when consuming messages from the broker. Defaults to `1`
* `UsePublisherConfirms`: Controls if [publisher confirms](https://www.rabbitmq.com/confirms.html) should be used. Defaults to `true`
* `MaxWaitTimeForConfirms`: How long the client should wait for publisher confirms if enabled. Defaults to `30` seconds.

* `RetryDelay`: The time to wait before trying to reconnect to the broker if connection is lost. Defaults to `10` seconds

If you prefer to use a custom name for you connection string use:

<!-- import rabbitmq-config-connectionstringname -->

or if you want to specify the connection string in code:

<!-- import rabbitmq-config-connectionstring-in-code -->

### Callback support

RabbitMQ is a broker which means that you scale out by adding more endpoints feeding of the same broker queue. This usually works fine if no state is shared between the different instances. This is not the case for callbacks since they do rely on local state kept in memory. In order to seamlessly support this scenario out of the box the RabbitMQ transport has the concept of a callback receiver. Essentially this is a separate queue named `{endpointname}.{machinename}` to which all callbacks are routed. This means that callbacks are handled by the same instance that requested them. This behaviour is on by default and if you're not using callbacks you can disable it using the following configuration:

<!-- import rabbitmq-config-disablecallbackreceiver -->

This means that the queue will not be created and no extra threads will be used to fetch messages from that queue. 

By default 1 dedicated thread is used for the callbacks but if you want to add more threads due to a high rate of callbacks being used you can control the threads count using:

<!-- import rabbitmq-config-callbackreceiver-thread-count -->

### Getting full control over the broker connection

The default connection manager that comes with the transport is usually good enough for most users. But if you want full control over how the connection(s) with the broker is managed you can implement you own connection manager. To do this you need to create your own class inheriting from `IManageRabbitMqConnections`. This requires you to provide a connection for:

1. Administrative actions like creating queues and exchanges
2. Publishing messages to the broker
3. Consuming messages from the broker

In order for the transport to use you new connection manager you need to register it as shown below:

<!-- import rabbitmq-config-useconnectionmanager -->




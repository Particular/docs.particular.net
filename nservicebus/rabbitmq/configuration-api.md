---
title: RabbitMQ Transport configuration settings
summary: The various ways to customize the RabbitMQ transport
reviewed: 2016-04-20
tags:
- RabbitMQ
- Transports
- Transactions
---

## Configuring RabbitMQ transport to be used

To make NServiceBus use RabbitMQ as the underlying transport add this to the configuration:

snippet:rabbitmq-config-basic

In order to work the transport needs to connect the the RabbitMQ broker. By default the transport will look for a connection string called `NServiceBus/Transport` in the `app.config`.

A typical connection string would look like this:

snippet:rabbitmqconnectionstring

In the above sample the transport is configured to connect to the RabbitMQ broker running at the machine `broker1`.

Below is the full list of connection string options. Note that they are separated by a `;`.

 * `Port`: The port where the broker listens. Defaults to `5672`
 * `VirtualHost`: The [virtual host](https://www.rabbitmq.com/access-control.html) to use. Defaults to `/`
 * `UserName`: The username when connecting. Defaults to `guest`
 * `Password`: The password when connecting. Defaults to `guest`
 * `RequestedHeartbeat`: The interval for the heartbeats between the client and the server. Defaults to `5` seconds
 * `DequeueTimeout` The time period allowed for the dequeue strategy to dequeue a message. Defaults to `1` second
 * `PrefetchCount`: The number of messages to [prefetch](http://www.rabbitmq.com/consumer-prefetch.html) when consuming messages from the broker. Defaults to the number of configured threads for the transport(as of v2.1)
 * `UsePublisherConfirms`: Controls if [publisher confirms](https://www.rabbitmq.com/confirms.html) should be used. Defaults to `true`
 * `MaxWaitTimeForConfirms`: How long the client should wait for publisher confirms if enabled. Defaults to `30` seconds.
 * `RetryDelay`: The time to wait before trying to reconnect to the broker if connection is lost. Defaults to `10` seconds

To use a custom name for the connection string use:

snippet:rabbitmq-config-connectionstringname

To specify the connection string in code:

snippet:rabbitmq-config-connectionstring-in-code

For debugging purposes, increase the `RequestedHeartbeat` and `DequeueTimeout` like this:

snippet:rabbitmqconnectionstring-debug


### Callback support

RabbitMQ is a broker which means that scale out is done by adding more endpoints feeding of the same broker queue. This usually works fine if no state is shared between the different instances. This is not the case for callbacks since they do rely on local state kept in memory. In order to seamlessly support this scenario out of the box the RabbitMQ transport has the concept of a callback receiver. Essentially this is a separate queue named `{endpointname}.{machinename}` to which all callbacks are routed. This means that callbacks are handled by the same instance that requested them. This behavior is on by default. If not using callbacks it can disable it using the following configuration:

snippet:rabbitmq-config-disablecallbackreceiver

This means that the queue will not be created and no extra threads will be used to fetch messages from that queue.

By default 1 dedicated thread is used for the callbacks. To add more threads, due to a high rate of callbacks, use the following:

snippet:rabbitmq-config-callbackreceiver-thread-count


### Controlling the message ID strategy

By default NServiceBus uses the `message-id` property of the AMQP standard to relay the message id. If this header isn't set the transport will throw an exception since NServiceBus needs a message ID in order to perform retries, de-duplication etc. in a safe way. In integration scenarios where the sender is not controlled consider using a custom scheme that extracts the message ID from e.g.a custom header or some data contained in the actual message body. In these cases in a custom strategy by calling:

snippet:rabbitmq-config-custom-id-strategy

WARNING: It is extremely important to use a uniquely identifying property of the message in a custom message ID strategy. If the value for a message were to change (for example, if attempting to use `Guid.NewGuid().ToString()`) then message retries would break, as the infrastructure would be unable to determine that it was processing the same message repeatedly.


### Getting full control over the broker connection

The default connection manager that comes with the transport is usually good enough for most users. To control how the connection(s) with the broker is managed implement a custom connection manager by inheriting from `IManageRabbitMqConnections`. This requires that connections be provided for:

 1. Administrative actions like creating queues and exchanges
 2. Publishing messages to the broker
 3. Consuming messages from the broker

In order for the transport to use the above register it as shown below:

snippet:rabbitmq-config-useconnectionmanager


### Controlling behavior when broker connection is lost

By the default the RabbitMQ transport will trigger the on critical error action when it continuously fails to connect to the the broker for 2 minutes. This can now be customized using the following configuration setting: (values must be parsable to `System.TimeSpan`)

snippet:rabbitmq-custom-breaker-settings


### Changing routing topology

By default the RabbitMQ transport create separate exchanges for each message type, including inherited types, being published in the system. This means that polymorphic routing and multiple inheritance for events is supported since each subscriber will bind its input queue to the relevant exchange based on the event types that it has handlers for.

For less complex scenarios use the `DirectRoutingTopology` that routes all events through a single exchange, `amq.topic` by default. The events will be published using a routing key based on the event type and subscribers will use that key to filter their subscriptions.

To enable direct routing use the following configuration:

snippet:rabbitmq-config-usedirectroutingtopology

You can adjust the conventions for exchange name and routing key by using the overload:

snippet:rabbitmq-config-usedirectroutingtopologywithcustomconventions

If the routing topologies mentioned above isn't flexible enough then take full control over how routing is done by implementing a custom routing topology. This is done by:

 1. Define the topology by creating a class implementing `IRoutingTopology`
 1. Register it with the transport calling `.UseRoutingTopology` as shown below

snippet:rabbitmq-config-useroutingtopology

## Transactions and delivery guarantees

### Versions 4 and above

The RabbitMQ transport supports the following [Transport Transaction Modes](/nservicebus/messaging/transactions.md):

* Transport transaction - Receive Only
* Unreliable (Transactions Disabled)

### Transport transaction - Receive Only

When running in `ReceiveOnly` mode, the RabbitMQ transport consumes messages from the broker in manual acknowledgment mode. After a message is successfully processed, it is acknowledged via the AMQP [basic.ack](http://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method, which lets the broker know that the message can be removed from the queue. If a message is not successfully processed and needs to be retried, it is requeued via the AMQP [basic.reject](http://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.reject) method.

WARNING: If the connection to the broker is lost for any reason before a message can be acknowledged, even if the message was successfully processed, the message will automatically be requeued by the broker. This will result in the endpoint processing the same message multiple times.

### Unreliable (Transactions Disabled)

When running in `None` mode, the RabbitMQ transport consumes messages from the broker in manual acknowledgment mode. Regardless of whether a message is successfully processed or not, it is acknowledged via the AMQP [basic.ack](http://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method after the processing attempt. This means that a message will be attempted once, and moved to the error queue if it fails.

WARNING: Since manual acknowledgment mode is being used, if the connection to the broker is lost for any reason before a message can be acknowledged, the message will automatically be requeued by the broker. If this occurs, the message will be retried by the endpoint, despite the transaction mode setting.

---
title: RabbitMQ Transport configuration settings
summary: The various ways to customize the RabbitMQ transport.
reviewed: 2016-05-12
tags:
- RabbitMQ
- Transports
- Transactions
---


## Configuring the endpoint to use the RabbitMQ transport

To use RabbitMQ as the underlying transport:

snippet:rabbitmq-config-basic


## RabbitMQ connection string

The RabbitMQ transport requires a connection string to connect to the RabbitMQ broker. This connection string can be specified via code or via `app.config`.


### Specifying the connection string via code

To specify the connection string in code:

snippet:rabbitmq-config-connectionstring-in-code


### Specifying the connection string via app.config

By default, the transport will look for a connection string called `NServiceBus/Transport` in `app.config`:

snippet:rabbitmq-connectionstring

To use a custom name for the connection string:

snippet:rabbitmq-config-connectionstringname


### Connection string options

Below is the list of connection string options. When constructing a connection string, these options should be separated by a semicolon.


#### Host

The host name of the broker.

NOTE: This value is required.


#### Port

The port where the broker listens.

Default: `5671` if the `UseTls` setting is set to `true`, otherwise the default value is `5672`


#### VirtualHost

The [virtual host](https://www.rabbitmq.com/access-control.html) to use.

Default: `/`


#### UserName

The user name to use to connect to the broker.

Default: `guest`


#### Password

The password to use to connect to the broker.

Default: `guest`


#### RequestedHeartbeat

The interval for the heartbeats between the client and the server.

Default: `5` seconds


#### DequeueTimeout

The time period allowed for the dequeue strategy to dequeue a message.

Default: `1` second

Versions: 3 and below.


#### PrefetchCount

The number of messages to [prefetch](http://www.rabbitmq.com/consumer-prefetch.html) when consuming messages from the broker.

Default: The number of configured threads for the transport (as of Version 2.1)

Versions: 3 and below. In Versions 4 and above, `PrefetchCount` is controlled through the `EndpointConfiguration.LimitMessageProcessingConcurrencyTo` setting.


#### UsePublisherConfirms

Controls if [publisher confirms](https://www.rabbitmq.com/confirms.html) should be used.

Default: `true`


#### MaxWaitTimeForConfirms

How long the client should wait for publisher confirms, if enabled.

Default: `30` seconds.

Versions: 3 and below.


#### RetryDelay

The time to wait before trying to reconnect to the broker if the connection is lost.

Default: `10` seconds


#### UseTls

Indicates if the connection to the broker should be secured with [TLS](/nservicebus/rabbitmq/configuration-api.md#specifying-the-connection-string-transport-layer-security-support).

Default: `false`

Versions: 3.2 and above


#### CertPath

The file path to the client authentication certificate when using [TLS](/nservicebus/rabbitmq/configuration-api.md#specifying-the-connection-string-transport-layer-security-support)

Versions: 3.2 and above


#### CertPassphrase

The password for the client authentication certificate specified in `CertPath`

Versions: 3.2 and above

NOTE: For debugging purposes, it can be helpful to increase the `RequestedHeartbeat` and `DequeueTimeout` settings as shown below:

snippet:rabbitmq-connectionstring-debug

Increasing these settings can help prevent the connection to the broker from timing out while the code is paused from hitting a breakpoint.


## Callback support

When scaling out an endpoint using the RabbitMQ transport, any of the endpoint instances can consume messages from the same shared broker queue. However, this behavior can cause problems when dealing with callback messages because the reply message for the callback needs to go to the specific instance that requested the callback.


### Versions 4 and above

In Versions 4 and above, callbacks are no longer directly managed by the RabbitMQ transport and are not enabled by default. To enable them, follow the steps outlined in the [Callbacks documentation](/nservicebus/messaging/handling-responses-on-the-client-side.md#message-routing-nservicebus-callbacks-version-1-and-above).


### Versions 3 and below

In Versions 3 and below, callbacks are enabled by default, and the transport will create a separate callback receiver queue, named `{endpointname}.{machinename}`, to which all callbacks are routed. If callbacks are not being used, the callback receiver can be disabled using the following setting:

snippet:rabbitmq-config-disable-callback-receiver

This means that the queue will not be created and no extra threads will be used to fetch messages from that queue.

By default, 1 dedicated thread is used for the callbacks. To add more threads, due to a high rate of callbacks, use the following:

snippet:rabbitmq-config-callbackreceiver-thread-count


## Transport Layer Security support

In Versions 3.2 and above, the RabbitMQ transport supports creating secure connections to the broker using Transport Layer Security (TLS). For information on how to configure TLS on the RabbitMQ broker, refer to the [RabbitMQ documentation](http://www.rabbitmq.com/ssl.html). To enable TLS support, set the `UseTls` setting to `true` in the connection string. If the RabbitMQ broker has been configured to require client authentication, a client certificate can be specified in the `CertPath` setting. If that certificate requires a password, it can be specified in the `CertPassphrase` setting.

An example connection string using these settings:

snippet:rabbitmq-connection-tls

NOTE: The RabbitMQ transport requires TLS 1.2 to establish a secure connection, so the broker must have TLS 1.2 enabled.


## Controlling the message ID strategy

By default, NServiceBus uses the `message-id` property of the AMQP standard to relay the message ID. If this header isn't set, the transport will throw an exception because NServiceBus needs a message ID to perform retries, de-duplication, etc., in a safe way. For integration scenarios where the sender is not controlled, consider using a custom scheme that extracts a message ID from a custom header or some data contained in the actual message body. This custom strategy can be configured by calling:

snippet:rabbitmq-config-custom-id-strategy

WARNING: It is extremely important to use a uniquely identifying property of the message in a custom message ID strategy. If the value for a message were to change (for example, if attempting to use `Guid.NewGuid().ToString()`) then message retries would break, as the infrastructure would be unable to determine that it was processing the same message repeatedly.


## Providing a custom connection manager


### Versions 4 and above

In Versions 4 and above, the ability to provide a custom connection manager via the `IManageRabbitMqConnections` interface has been removed.


### Versions 3 and below

The default connection manager that comes with the transport is usually good enough for most users. To control how the connections with the broker are managed, implement a custom connection manager by inheriting from `IManageRabbitMqConnections`. This requires that connections be provided for:

 1. Administrative actions like creating queues and exchanges.
 1. Publishing messages to the broker.
 1. Consuming messages from the broker.

In order for the transport to use the above, register it as shown below:

snippet:rabbitmq-config-useconnectionmanager


## Controlling behavior when the broker connection is lost

The RabbitMQ transport monitors the connection to the broker and will trigger the critical error action if the connection fails and stays disconnected for the configured amount of time.


### TimeToWaitBeforeTriggering

Controls the amount of time the transport waits after a failure is detected before triggering the critical error action.

Type: `System.TimeSpan`

Default: `00:02:00` (2 minutes)

snippet:rabbitmq-custom-breaker-settings-time-to-wait-before-triggering-code
snippet:rabbitmq-custom-breaker-settings-time-to-wait-before-triggering-xml


### DelayAfterFailure

Controls the amount of time the transport waits after a failure is detected before trying to poll for incoming messages again.

Type: `System.TimeSpan`

Default: `00:00:05` (5 seconds)

snippet:rabbitmq-custom-breaker-settings-delay-after-failure

NOTE: This setting has been removed in Versions 4 and above because the transport no longer needs to poll for incoming messages.


## Routing topology

The RabbitMQ transport has the concept of a routing topology, which controls how it creates exchanges, queues, and the bindings between them in the RabbitMQ broker. The routing topology also controls how the transport uses the exchanges it creates to send and publish messages.

### Conventional Routing Topology

By default, the RabbitMQ transport uses the `ConventionalRoutingTopology`, which creates separate [fanout exchanges](https://www.rabbitmq.com/tutorials/amqp-concepts.html) for each message type, including inherited types, being published in the system. This means that polymorphic routing and multiple inheritance for events is supported since each subscriber will bind its input queue to the relevant exchanges based on the event types that it has handlers for.

WARNING: The RabbitMQ transport doesn't automatically modify or delete existing bindings. Because of this, when modifying the message class hierarchy, the existing bindings for the previous class hierarchy will still exist and should be deleted manually.


### Direct Routing Topology

The `DirectRoutingTopology` is another provided routing topology that routes all events through a single exchange, `amq.topic` by default. Events are published using a routing key based on the event type, and subscribers will use that key to filter their subscriptions.

To enable the direct routing topology, use the following configuration:

snippet:rabbitmq-config-usedirectroutingtopology

Adjust the conventions for exchange name and routing key by using the overload:

snippet:rabbitmq-config-usedirectroutingtopologywithcustomconventions


### Custom Routing Topology

If the routing topologies mentioned above aren't flexible enough, then take full control over how routing is done by implementing a custom routing topology. This is done by:

 1. Define the topology by creating a class implementing `IRoutingTopology`.
 1. Register it with the transport calling `UseRoutingTopology` as shown below.

snippet:rabbitmq-config-useroutingtopology


## Transactions and delivery guarantees


### Versions 4 and above

The RabbitMQ transport supports the following [Transport Transaction Modes](/nservicebus/transports/transactions.md):

 * Transport transaction - Receive Only
 * Unreliable (Transactions Disabled)


#### Transport transaction - Receive Only

When running in `ReceiveOnly` mode, the RabbitMQ transport consumes messages from the broker in manual acknowledgment mode. After a message is successfully processed, it is acknowledged via the AMQP [basic.ack](http://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method, which lets the broker know that the message can be removed from the queue. If a message is not successfully processed and needs to be retried, it is re-queued via the AMQP [basic.reject](http://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.reject) method.

WARNING: If the connection to the broker is lost for any reason before a message can be acknowledged, even if the message was successfully processed, the message will automatically be re-queued by the broker. This will result in the endpoint processing the same message multiple times.


#### Unreliable (Transactions Disabled)

When running in `None` mode, the RabbitMQ transport consumes messages from the broker in manual acknowledgment mode. Regardless of whether a message is successfully processed or not, it is acknowledged via the AMQP [basic.ack](http://www.rabbitmq.com/amqp-0-9-1-quickref.html#basic.ack) method after the processing attempt. This means that a message will be attempted once, and moved to the error queue if it fails.

WARNING: Since manual acknowledgment mode is being used, if the connection to the broker is lost for any reason before a message can be acknowledged, the message will automatically be re-queued by the broker. If this occurs, the message will be retried by the endpoint, despite the transaction mode setting.
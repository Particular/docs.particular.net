---
title: Connection settings
summary: The various ways to customize the RabbitMQ transport.
reviewed: 2026-11-04
component: Rabbit
redirects:
 - nservicebus/rabbitmq/connection-strings
 - nservicebus/rabbitmq/connection-settings
---

The RabbitMQ transport requires a connection string to connect to the RabbitMQ broker, and there are two different styles to choose from. It can accept the standard [amqp URI](https://www.rabbitmq.com/uri-spec.html) connection strings, or a custom format documented below.


### Specifying the connection string via code

To specify the connection string in code:

snippet: rabbitmq-config-connectionstring-in-code


## Connection string options

Below is the list of connection string options. When constructing a connection string, these options should be separated by a semicolon.


### Host

The host name of the broker.

> [!NOTE]
> The host name is required.

### Port

The port where the broker listens.

Default: `5671` if the `UseTls` setting is set to `true`, otherwise the default value is `5672`

### VirtualHost

The [virtual host](https://www.rabbitmq.com/vhosts.html) to use.

Default: `/`

### UserName

The user name to use to connect to the broker.

Default: `guest`

### Password

The password to use to connect to the broker.

Default: `guest`

### UseTls

Indicates if the connection to the broker should be secured with [TLS](#transport-layer-security-support).

Default: `false`

partial: options

## Transport Layer Security support

Secure connections to the broker using [Transport Layer Security (TLS)](https://www.rabbitmq.com/ssl.html) are supported. To enable TLS support, set the `UseTls` setting to `true` in the connection string:

snippet: rabbitmq-connection-tls

#if-version [, 8)

> [!WARNING]
> TLS 1.2 must be enabled on the broker to establish a secure connection.

#end-if

### Peer verification

If the broker has been configured to verify peer certificates, then a client certificate must be provided:

snippet: rabbitmq-client-certificate-file

This can also be done by passing a certificate in directly:

snippet: rabbitmq-client-certificate

partial: connection-string-cert

### Remote certificate validation

By default, the RabbitMQ client will refuse to connect to the broker if the remote server certificate is invalid. This validation can be disabled with the following setting:

snippet: rabbitmq-disable-remote-certificate-validation

### Certificate authentication

By default, the broker requires a username and password to authenticate the client, but it can be configured to use client certificate data instead. To do this, the client needs to be configured to use the external authentication mechanism:

snippet: rabbitmq-external-auth-mechanism

partial: external-auth

partial: auth-mechanisms

partial: add-cluster-node

partial: management-api-configuration

partial: delivery-limit-validation

## Broker permissions

The transport uses the credentials from the connection string for all AMQP operations. RabbitMQ grants [configure, write, and read permissions](https://www.rabbitmq.com/docs/access-control) per virtual host, each as a regular expression that matches queue and exchange names. Configure permission covers declaring a queue or exchange. Write permission covers publishing to an exchange and being the destination of a binding. Read permission covers consuming from a queue and being the source of a binding.

When [installers](/nservicebus/operations/installers.md) are enabled, the transport also creates the endpoint's queues, exchanges, bindings, and delayed delivery infrastructure, so the credentials also need permission to create them. The tables below list the permissions an endpoint needs at runtime, after that infrastructure exists.

### Sending

|Operation |Conventional topology |Direct topology
|:--- |:--- |:---
|Sending messages |Write on the destination endpoint's exchange |Write on the default exchange (`amq.default` for permission checks)
|Sending delayed messages |Write on the destination endpoint's exchange |Write on the destination queue

These requirements also apply when sending to the error and audit queues.

### Receiving

The endpoint needs read permission on the queues it receives from, regardless of the routing topology.

### Delayed delivery

The endpoint needs these permissions to send delayed messages, including delayed retries, regardless of the routing topology:

#if-version [,7)

|Resource |Permissions
|:--- |:---
|The `nsb.delay-level-00` to `nsb.delay-level-27` exchanges |Write
|The `nsb.delay-delivery` exchange |Read

#end-if

#if-version [7,)

|Resource |Permissions
|:--- |:---
|The `nsb.v2.delay-level-00` to `nsb.v2.delay-level-27` exchanges |Write
|The `nsb.v2.delay-delivery` exchange |Read

#end-if

The endpoint also needs the destination permissions listed under [Sending](#broker-permissions-sending).

### Conventional routing topology

With the [conventional routing topology](routing-topology.md#conventional-routing-topology), an endpoint needs:

|Resource |Permissions |Used for
|:--- |:--- |:---
|Event exchanges, named `Namespace:TypeName`, for every event type the endpoint publishes or subscribes to, and for each base class and interface of that type |Configure, write, read |Publishing and subscribing
|The endpoint's own exchange |Write |Subscribing and unsubscribing

Unsubscribing removes the binding between the event exchange and the endpoint's exchange. RabbitMQ requires read permission on the source (the event exchange) and write permission on the destination (the endpoint's exchange) to remove this binding. Configure permission is not required.

> [!WARNING]
> When the endpoint publishes or subscribes to an event type for the first time in a process, the transport declares the exchanges for the type, its base classes, and its interfaces, and binds them together. RabbitMQ checks configure permission on every declaration, including a declaration of an exchange that already exists with the same arguments. The credentials therefore need configure permission on event exchanges even when every exchange already exists. Disabling installers does not remove this requirement.

### Direct routing topology

With the [direct routing topology](routing-topology.md#direct-routing-topology), an endpoint needs:

|Resource |Permissions |Used for
|:--- |:--- |:---
|The topic exchange, `amq.topic` by default |Write |Publishing
|The topic exchange |Read |Subscribing and unsubscribing
|A topic exchange other than `amq.topic` |Configure |Subscribing
|Queues the endpoint receives from |Write |Subscribing and unsubscribing

> [!WARNING]
> Subscribing requires configure permission on a custom topic exchange, even when it already exists and installers are disabled. Publishing does not require configure permission, nor does subscribing with the default `amq.topic` exchange.

## Controlling the prefetch count

When consuming messages from the broker, throughput can be improved by having the consumer [prefetch](https://www.rabbitmq.com/consumer-prefetch.html) additional messages.
The prefetch count is calculated by multiplying the [maximum concurrency](/nservicebus/operations/tuning.md) by the prefetch multiplier. The default value of the multiplier is 3, but it can be changed as follows:

snippet: rabbitmq-config-prefetch-multiplier

Alternatively, the whole calculation can be overridden by setting the prefetch count directly as follows:

snippet: rabbitmq-config-prefetch-count

> [!NOTE]
> If the configured value is less than the maximum concurrency, the prefetch count will be set to the maximum concurrency value instead.

## Controlling behavior when the broker connection is lost

The RabbitMQ transport monitors the connection to the broker and will trigger [the critical error action](/nservicebus/hosting/critical-errors.md) if the connection fails and stays disconnected for the configured amount of time.

### Heartbeat interval

Controls how frequently AMQP heartbeat messages are sent between the endpoint and the broker.

Type: `System.TimeSpan`

Default: `00:01:00` (1 minute)

snippet: change-heartbeat-interval

### Network recovery interval

Controls the time to wait between attempts to reconnect to the broker if the connection is lost.

Type: `System.TimeSpan`

Default: `00:00:10` (10 seconds)

snippet: change-network-recovery-interval

### TimeToWaitBeforeTriggering

Controls the amount of time the transport waits after a failure is detected before triggering the critical error action.

Type: `System.TimeSpan`

Default: `00:02:00` (2 minutes)

snippet: rabbitmq-custom-breaker-settings-time-to-wait-before-triggering-code

## Debugging recommendations

It can be helpful to increase the heartbeat interval to avoid connection timeouts while debugging:

snippet: rabbitmq-debug-api

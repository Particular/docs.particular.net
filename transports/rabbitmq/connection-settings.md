---
title: Connection settings
summary: The various ways to customize the RabbitMQ transport.
reviewed: 2025-04-08
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

### Client authentication

If the broker has been configured to require client authentication, a client certificate must be specified:

snippet: rabbitmq-client-certificate-file

This can also be done by passing a certificate in directly:

snippet: rabbitmq-client-certificate

partial: connection-string-cert

### Remote certificate validation

By default, the RabbitMQ client will refuse to connect to the broker if the remote server certificate is invalid. This validation can be disabled with the following setting:

snippet: rabbitmq-disable-remote-certificate-validation

### External authentication

By default, the broker requires a username and password to authenticate the client, but it can be configured to use other external authentication mechanisms. If the broker requires an external authentication mechanism, the client can be configured to use it with the following setting:

snippet: rabbitmq-external-auth-mechanism

partial: add-cluster-node

partial: management-api-configuration

partial: delivery-limit-validation

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

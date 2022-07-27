---
title: Connection settings
summary: The various ways to customize the RabbitMQ transport.
reviewed: 2020-04-15
component: Rabbit
redirects:
 - nservicebus/rabbitmq/connection-strings
 - nservicebus/rabbitmq/connection-settings
---

The RabbitMQ transport requires a connection string to connect to the RabbitMQ broker, and there are two different styles to choose from. It can accept the standard [AMQP URI](https://www.rabbitmq.com/uri-spec.html) connection strings, and it also can use a custom format that is documented below.


### Specifying the connection string via code

To specify the connection string in code:

snippet: rabbitmq-config-connectionstring-in-code


## Connection string options

Below is the list of connection string options. When constructing a connection string, these options should be separated by a semicolon.


### Host

The host name of the broker.

NOTE: This Host value is required.

By default, the [guest user can only connect via localhost](https://www.rabbitmq.com/access-control.html). If connecting to a remote host, a user name and password must be provided.

```xml
<connectionStrings>
   <add name="NServiceBus/Transport"
        connectionString="host=myremoteserver;
                          username=myusername;
                          password=mypassword"/>
</connectionStrings>
```


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


### RequestedHeartbeat

The interval for heartbeats between the endpoint and the broker.

Default: `60` seconds


### RetryDelay

The time to wait before trying to reconnect to the broker if the connection is lost.

Type: `System.TimeSpan`

Default: `00:00:10` (10 seconds)

### UseTls

Indicates if the connection to the broker should be secured with [TLS](#transport-layer-security-support).

Default: `false`


### CertPath

The file path to the client authentication certificate when using [TLS](#transport-layer-security-support).


### CertPassphrase

The password for the client authentication certificate specified in `CertPath`


## Transport Layer Security support

Secure connections to the broker using [Transport Layer Security (TLS)](https://www.rabbitmq.com/ssl.html) are supported. To enable TLS support, set the `UseTls` setting to `true` in the connection string:

snippet: rabbitmq-connection-tls

WARNING: TLS 1.2 must be enabled on the broker to establish a secure connection.


### Client authentication

If the broker has been configured to require client authentication, a client certificate must be specified in the `CertPath` setting. If that certificate requires a password, it must be specified in the `CertPassphrase` setting.

Here is a sample connection string using these settings:

snippet: rabbitmq-connection-client-auth

Client certificates can also be specified via code instead of using the connection string:

snippet: rabbitmq-client-certificate-file

This can also be done by passing a certificate in directly:

snippet: rabbitmq-client-certificate

NOTE: If a certificate is specified via either code API, the `CertPath` and `CertPassphrase` connection string settings are ignored.


### Remote certificate validation 

By default, the RabbitMQ client will refuse to connect to the broker if the remote server certificate is invalid. This validation can be disabled by using the following setting:

snippet: rabbitmq-disable-remote-certificate-validation


### External authentication

By default, the broker requires a username and password to authenticate the client, but it can be configured to use other external authentication mechanisms. If the broker requires an external authentication mechanism, the client can be configured to use it with the following setting:

snippet: rabbitmq-external-auth-mechanism

partial: add-cluster-node

## Controlling the prefetch count

When consuming messages from the broker, throughput can be improved by having the consumer [prefetch](https://www.rabbitmq.com/consumer-prefetch.html) additional messages.
The prefetch count is calculated by multiplying the [maximum concurrency](/nservicebus/operations/tuning.md#tuning-concurrency) by the prefetch multiplier. The default value of the multiplier is 3, but it can be changed by using the following:

snippet: rabbitmq-config-prefetch-multiplier

Alternatively, the whole calculation can be overridden by setting the prefetch count directly using the following:

snippet: rabbitmq-config-prefetch-count

NOTE: If the configured value is less than the maximum concurrency, the prefetch count will be set to the maximum concurrency value instead.


## Controlling behavior when the broker connection is lost

The RabbitMQ transport monitors the connection to the broker and will trigger the critical error action if the connection fails and stays disconnected for the configured amount of time.

### Heartbeat interval

Controls how frequently AMQP heartbeat messages will be sent between the endpoint and the broker.

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

For debugging purposes, it can be helpful to increase the heartbeat interval via the connection string:

snippet: rabbitmq-connectionstring-debug

Or via the API:

snippet: rabbitmq-debug-api

Increasing this setting can help to avoid connection timeouts while debugging.


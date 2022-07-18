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


partial: appconfig


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


partial: DequeueTimeout


partial: PrefetchCount


partial: publisher-confirms


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


partial: tls-details


partial: connection-manager


partial: prefetch-control


partial: publisher-confirms-setting


## Controlling behavior when the broker connection is lost

The RabbitMQ transport monitors the connection to the broker and will trigger the critical error action if the connection fails and stays disconnected for the configured amount of time.

### Heartbeat interval

Controls how frequently AMQP heartbeat messages will be sent between the endpoint and the broker.

Type: `System.TimeSpan`

Default: `00:01:00` (1 minute)

snippet: change-heartbeat-interval


## Network recovery interval

Controls the time to wait between attempts to reconnect to the broker if the connection is lost.

Type: `System.TimeSpan`

Default: `00:00:10` (10 seconds)

snippet: change-network-recovery-interval


### TimeToWaitBeforeTriggering

Controls the amount of time the transport waits after a failure is detected before triggering the critical error action.

Type: `System.TimeSpan`

Default: `00:02:00` (2 minutes)


snippet: rabbitmq-custom-breaker-settings-time-to-wait-before-triggering-code


partial: delayafterfailure


## Debugging recommendations

For debugging purposes, it can be helpful to increase the heartbeat interval via the connection string:

snippet: rabbitmq-connectionstring-debug

Or via the API:

snippet: rabbitmq-debug-api

Increasing this setting can help to avoid connection timeouts while debugging.


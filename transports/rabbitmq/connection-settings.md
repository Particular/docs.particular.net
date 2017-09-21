---
title: Connection settings
summary: The various ways to customize the RabbitMQ transport.
reviewed: 2017-01-13
component: Rabbit
redirects:
 - nservicebus/rabbitmq/connection-strings
 - nservicebus/rabbitmq/connection-settings
---


RabbitMQ uses the [AMQP URI Specification](https://www.rabbitmq.com/uri-spec.html). The RabbitMQ transport requires a connection string to connect to the RabbitMQ broker.


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

The [virtual host](https://www.rabbitmq.com/access-control.html) to use.

Default: `/`


### UserName

The user name to use to connect to the broker.

Default: `guest`


### Password

The password to use to connect to the broker.

Default: `guest`


### RequestedHeartbeat

The interval for the heartbeats between the client and the server.

Default: `5` seconds


partial: DequeueTimeout


partial: PrefetchCount


partial: publisher-confirms


### RetryDelay

The time to wait before trying to reconnect to the broker if the connection is lost.

Default: `10` seconds


partial: tls-settings


partial: debugging-note


partial: tls-details


partial: connection-manager


partial: prefetch-control


partial: publisher-confirms-setting


## Controlling behavior when the broker connection is lost

The RabbitMQ transport monitors the connection to the broker and will trigger the critical error action if the connection fails and stays disconnected for the configured amount of time.


### TimeToWaitBeforeTriggering

Controls the amount of time the transport waits after a failure is detected before triggering the critical error action.

Type: `System.TimeSpan`

Default: `00:02:00` (2 minutes)

partial: timetowaitbeforetriggering


partial: delayafterfailure
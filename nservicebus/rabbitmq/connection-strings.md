---
title: RabbitMQ Transport connection strings
summary: Detailed connection string information for RabbitMQ.
reviewed: 2016-04-29
tags:
- Connection strings
- Transports
- RabbitMQ
---

RabbitMQ uses the [AMQP URI Specification](https://www.rabbitmq.com/uri-spec.html). Refer to the [configuration documentation](/nservicebus/rabbitmq/configuration-api.md#configuring-rabbitmq-transport-to-be-used) to see the possible configuration parameters.

```xml
<connectionStrings>
   <!-- RabbitMQ -->
   <add name="NServiceBus/Transport"
        connectionString="host=localhost"/>
</connectionStrings>
```

### Remote Host

By default, the [guest user can only connect via localhost](https://www.rabbitmq.com/access-control.html). If connecting to a remote host, a user name and password must be provided.

```xml
<connectionStrings>
   <!-- RabbitMQ -->
   <add name="NServiceBus/Transport"
        connectionString="host=myremoteserver;
                          username=myusername;
                          password=mypassword"/>
</connectionStrings>
```

---
title: Simple RabbitMQ usage
reviewed: 2016-03-21
component: Rabbit
tags:
- RabbitMQ
related:
- nservicebus/rabbitmq
---


## Code walk-through

This sample shows a basic usage of RabbitMQ as a transport for NServiceBus.


### Enable RabbitMQ

snippet:ConfigureRabbit


### Configuring RabbitMQ

Since the above connection string does not define a username NServiceBus will default to

```no-highlight
username: guest
password: guest
```

For more information on the RabbitMQ connection string see [Configuring RabbitMQ transport to be used](/nservicebus/rabbitmq/configuration-api.md).
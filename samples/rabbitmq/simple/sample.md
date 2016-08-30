---
title: Simple RabbitMQ usage
reviewed: 2016-03-21
component: Rabbit
tags:
- RabbitMQ
related:
- nservicebus/rabbitmq
- nservicebus/rabbitmq/connection-settings
---


## Code walk-through

This sample shows a basic usage of RabbitMQ as a transport for NServiceBus.


### Enable RabbitMQ

snippet:ConfigureRabbit


### Configuring RabbitMQ

Since the above connection string does not define a username it will default to

```no-highlight
username: guest
password: guest
```
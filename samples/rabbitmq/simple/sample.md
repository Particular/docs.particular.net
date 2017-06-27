---
title: Simple RabbitMQ Transport usage
reviewed: 2016-10-18
component: Rabbit
related:
- transports/rabbitmq
- transports/rabbitmq/connection-settings
---


## Prerequisites

Ensure an instance of RabbitMQ is running and accessible.


## Code walk-through

This sample shows basic usage of RabbitMQ as a transport for NServiceBus. The application sends an empty message to itself, via the RabbitMQ broker, and writes to the console when the message is received.


### Configuration

snippet: ConfigureRabbit

The username and password can also be configured via the connection string. If these are not present, the connection string effectively defaults to `host=localhost;username=guest;password=guest`.

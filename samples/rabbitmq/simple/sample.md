---
title: Simple RabbitMQ Transport Usage
reviewed: 2020-04-07
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

The username and password can be configured via the connection string. If these are not present, the connection string defaults to `host=localhost;username=guest;password=guest`.

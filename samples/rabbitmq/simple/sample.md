---
title: Simple RabbitMQ Transport Usage
reviewed: 2020-09-29
component: Rabbit
related:
- transports/rabbitmq
- transports/rabbitmq/connection-settings
---


## Prerequisites

Ensure an instance of RabbitMQ is running and accessible.


## Code walk-through

This sample shows basic usage of RabbitMQ as a transport for NServiceBus to connect two endpoints. The sender either sends a command or publishes an event to a receiver endpoint, via the RabbitMQ broker, and writes to the console when the message is received.


### Configuration

snippet: ConfigureRabbit

The username and password can be configured via the connection string. If these are not present, the connection string defaults to `host=localhost;username=guest;password=guest`.

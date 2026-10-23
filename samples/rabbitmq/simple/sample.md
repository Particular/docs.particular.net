---
title: Simple RabbitMQ Transport Usage
reviewed: 2026-10-23
component: Rabbit
related:
- transports/rabbitmq
- transports/rabbitmq/connection-settings
---


## Prerequisites

Ensure an instance of RabbitMQ is running and accessible.

For local development, consider running the [Docker container](https://hub.docker.com/_/rabbitmq#how-to-use-this-image)


## Code walk-through

This sample shows basic usage of RabbitMQ as a transport for NServiceBus to connect two endpoints. The sender sends a command or publishes an event to a receiver endpoint using the RabbitMQ broker. The receiver writes to the console when the message is received.


### Configuration

snippet: ConfigureRabbit

The username and password can be configured in the connection string. If these are not present, the connection string defaults to `host=localhost;username=guest;password=guest`.

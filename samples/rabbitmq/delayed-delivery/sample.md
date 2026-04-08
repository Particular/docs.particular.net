---
title: RabbitMQ Delayed Delivery
reviewed: 2026-04-08
component: Rabbit
related:
- transports/rabbitmq
- transports/rabbitmq/delayed-delivery
- transports/rabbitmq/operations-scripting
---


## Prerequisites

Ensure an instance of RabbitMQ is running and accessible.


## Code walk-through

This sample demonstrates delayed delivery with the RabbitMQ transport. The sender sends commands with different delays, causing messages to be placed into different levels of the v2 delay infrastructure. The receiver logs each command when it is delivered.


### Configuration

snippet: ConfigureRabbit


### Sending delayed messages

snippet: SendDelayedMessages

Each delay value exercises different levels of the [v2 delay infrastructure](/transports/rabbitmq/delayed-delivery.md). The delays are chosen to spread messages across distinct delay levels:

| Delay | Starting delay level |
|---|---|
| 5 seconds | 2 |
| 60 seconds | 5 |
| 360 seconds | 8 |
| 86400 seconds (1 day) | 16 |

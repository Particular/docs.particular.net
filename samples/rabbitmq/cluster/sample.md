---
title: Clustered RabbitMQ Transport Usage
reviewed: 2021-06-04
component: Rabbit
related:
- transports/rabbitmq
- transports/rabbitmq/connection-settings
---


## Prerequisites

Ensure a RabbitMQ cluster is running is running and accessible. Use the following docker commands to set one up if needed:

```
docker network create --driver bridge network1

docker run -d --network network1 --hostname rabbit1 --name rabbit1 -p 5672:5672 -p 15672:15672 -e RABBITMQ_ERLANG_COOKIE='asdfasdf' rabbitmq:3-management
docker run -d --network network1 --hostname rabbit2 --name rabbit2 -p 5673:5672 -p 15673:15672 -e RABBITMQ_ERLANG_COOKIE='asdfasdf' rabbitmq:3-management
docker run -d --network network1 --hostname rabbit3 --name rabbit3 -p 5674:5672 -p 15674:15672 -e RABBITMQ_ERLANG_COOKIE='asdfasdf' rabbitmq:3-management

docker exec rabbit2 rabbitmqctl stop_app
docker exec rabbit2 rabbitmqctl join_cluster rabbit@rabbit1
docker exec rabbit2 rabbitmqctl start_app

docker exec rabbit3 rabbitmqctl stop_app
docker exec rabbit3 rabbitmqctl join_cluster rabbit@rabbit1
docker exec rabbit3 rabbitmqctl start_app

```

## Code walk-through

This sample shows how to use a clustered RabbitMQ as a transport for NServiceBus to connect two endpoints. The sender either sends a command or publishes an event to a receiver endpoint, via the RabbitMQ broker, and writes to the console when the message is received.

### Configuration

snippet: ConfigureClusteredRabbit

The username and password can be configured via the connection string. If these are not present, the connection string defaults to `host=localhost;username=guest;password=guest`.

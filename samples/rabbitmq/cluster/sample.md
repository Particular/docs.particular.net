---
title: Clustered RabbitMQ transport usage
summary: A sample showing how to use the RabbitMQ transport in a RabbitMQ cluster
reviewed: 2021-06-04
component: Rabbit
related:
- transports/rabbitmq
- transports/rabbitmq/connection-settings
---


## Prerequisites

Ensure a RabbitMQ cluster is running is running and accessible. The following docker commands will set one up if needed:

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

snippet: cluster-configuration

The username and password can be configured via the connection string. If these are not present, the connection string defaults to `host=localhost;username=guest;password=guest`.

Note that [delayed delivery](/nservicebus/messaging/delayed-delivery.md) isn't supported for clusters and the sample therefore disables them. This means that [saga timeouts](/nservicebus/sagas/timeouts.md) can't be used and [delayed retries](/nservicebus/recoverability#delayed-retries) must be disabled as shown below:

snippet: cluster-disable-retries

### Adding nodes

RabbitMQ allows additional nodes to be added to avoid having to use a load balancer in front of the cluster. Adding those nodes will make the connection manager use them in a round robin fashion and automatically re-connect to healthy nodes should the connection be lost.

RabbitMQ defaults to port `5672`; the extra nodes (`rabbit2` and `rabbit3`) are added as shown below:

snippet: cluster-add-nodes

With this setup the connection manager will initially connect to `rabbit1` but try `rabbit2` or `rabbit3` should `rabbit1` become unavailable.

To test this, shutdown node `rabbit2`:

`docker exec rabbit1 rabbitmqctl stop_app`

Notice that a `WARN` message is logged and that the endpoint is reconnected to the cluster.

---
title: Hosting endpoints in Docker Linux containers
summary: Hosting multiple endpoints in several Docker Linux containers managed by Docker Compose
reviewed: 2019-12-16
component: Core
related:
- nservicebus/hosting/docker-host
- nservicebus/hosting
---

This sample demonstrates how to use Docker Linux containers to host NServiceBus endpoints communicating over the [RabbitMQ transport](/transports/rabbitmq/).

downloadbutton

## Prerequisites

This sample requires that the following tools are installed:

 * [.NET Core 3.1 SDK](https://www.microsoft.com/net/download/core)
 * [Docker Community Edition](https://www.docker.com/community-edition) or higher
 * If using Windows, [configure Docker to use Linux containers](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers) to support the Linux-based RabbitMQ container

## Running the sample

Running the sample involves building the container images and starting the multi-container application.

### Building container images

Building the container images using the following command will `dotnet publish` (which includes `dotnet restore` and `dotnet build`) the endpoints in addition to building the container images for both the `Sender` and the `Receiver`:

```bash
$ docker-compose build
```

### Starting containers

When the container images are ready, the containers can be started:

```bash
$ docker-compose up -d
```

## Observing containers

Both containers log to the console. These logs can be inspected:

```bash
$ docker-compose logs sender
$ docker-compose logs receiver
```

### Stopping and removing containers

The containers can be stopped and removed:

```bash
$ docker-compose down
```

## Code walk-through

This sample consists of `Sender` and `Receiver` endpoints exchanging messages using the [RabbitMQ transport](/transports/rabbitmq/). Each of these three components runs in a separate Docker Linux container.

### Endpoint Docker image

Each endpoint is a container built on top of the official `mcr.microsoft.com/dotnet/core/runtime:3.1` image from [Docker Hub](https://hub.docker.com/). The container image builds and publishes the endpoint binaries and then uses those artifacts to build the final container image:

snippet: receiver

### Multi-container application

Endpoint container images for the `Sender` and the `Receiver` are combined with an official [RabbitMQ image](https://hub.docker.com/_/rabbitmq/) to create a multi-container application using [Docker Compose](https://docs.docker.com/compose/):

snippet: compose

### Transport configuration

Endpoints configure the RabbitMQ transport to use the broker instance running in the `rabbitmq` container:

snippet: TransportConfiguration

### Waiting for RabbitMQ broker to become available

Both endpoints block startup until the broker becomes available using the shared `ProceedIfRabbitMqIsAlive` hosted service.

See the [docker documentation for other options to control startup order](https://docs.docker.com/compose/startup-order/).

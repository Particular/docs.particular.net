---
title: Hosting endpoints in Docker Linux containers
summary: Hosting multiple endpoints in several Docker Linux containers managed by Docker Compose
reviewed: 2022-06-29
component: Core
related:
- nservicebus/hosting/docker-host
- nservicebus/hosting
redirects:
- samples/hosting/kubernetes-simple
---

This sample demonstrates how to use Docker Linux containers to host NServiceBus endpoints communicating over the [RabbitMQ transport](/transports/rabbitmq/).

downloadbutton

## Prerequisites

This sample requires that the following tools are installed:

* [.NET Core 7.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
* [Docker Personal](https://www.docker.com/products/personal/) or higher
* If using Windows, [configure Docker to use Linux containers](https://docs.docker.com/desktop/faqs/windowsfaqs/#how-do-i-switch-between-windows-and-linux-containers) to support the Linux-based RabbitMQ container

## Running the sample

Running the sample involves building the container images and starting the multi-container application.

partial: building-containers

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

partial: endpoint-docker-image

### Multi-container application

Endpoint container images for the `Sender` and the `Receiver` are combined with an official [RabbitMQ image](https://hub.docker.com/_/rabbitmq/) to create a multi-container application using [Docker Compose](https://docs.docker.com/compose/):

snippet: compose

### Transport configuration

Endpoints configure the RabbitMQ transport to use the broker instance running in the `rabbitmq` container:

snippet: TransportConfiguration

### Waiting for RabbitMQ broker to become available

Both endpoints block startup until the broker becomes available using the shared `ProceedIfRabbitMqIsAlive` hosted service.

See the [docker documentation for other options to control startup order](https://docs.docker.com/compose/startup-order/).

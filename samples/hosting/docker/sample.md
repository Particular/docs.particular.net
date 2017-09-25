---
title: Hosting endpoints in Docker Linux containers
summary: Hosting multiple endpoints in several Docker Linux containers managed by Docker Compose
reviewed: 2017-09-25
component: Core
tags:
- Hosting
related:
- nservicebus/hosting
---

This sample demonstrates how to use Docker Linux containers to host NServiceBus endpoints communicating over the RabbitMQ transport.

## Prerequisites

This sample requires that the following tools are installed:

  - .NET Core 2.0 SDK (https://www.microsoft.com/net/download/core)
  - Docker Community Edition (https://www.docker.com/community-edition) or higher

NOTE: Container that runs the RabbitMQ broker is a Linux container and [Docker for Windows also needs to be configured to use Linux containers](https://docs.docker.com/docker-for-windows/#switch-between-windows-and-linux-containers).

## Running the sample

The sample requires .NET Core CLI tools and Docker CLI tools to build and run the code.

### Building and publishing binaries

First step is to build the binaries using the .NET Core command line tools:

```
dotnet build
```

When the binaries have been compiled, the next step is preparing them for deployment into the container:

```
dotnet publish
```

### Building container images

When the binaries are ready for deployment, the next step is to build container images for both the `Sender` and the `Receiver`:

```
docker-compose build
```

### Starting containers

When the container images are ready, the containers can be started:

```
docker-compose up -d
```

## Observing containers

Both containers log to the console. These logs can be inspected:

```
docker-compose logs sender
docker-compose logs receiver
```

### Stopping and removing containers

The containers can be stopped and removed:

```
docker-compose down
```

## Code walk-through

This sample consists of `Sender` and `Publisher` endpoints exchanging messages using the [RabbitMQ transport](/transports/rabbitmq). Each of these three components runs in a separate Docker Linux container.

### Endpoint Docker image

Each endpoint is a container built on top of official `microsoft/dotnet:2.0-runtime` image from [Docker Hub](https://hub.docker.com/). The container image is built using endpoint binaries from the `bin/Debug/netcoreapp2.0/publish` folder:

snippet:Dockerfile

NOTE: Run `dotnet build` and `dotnet publish` commands to generate endpoint binaries before creating the image.

### Multi-container application

Endpoint container images for the `Sender` and the `Receiver` are combined with an official [RabbitMQ image](https://hub.docker.com/_/rabbitmq/) to create a multi-container application using [Docker Compose](https://docs.docker.com/compose/):

snippet:DockerCompose

### Transport configuration

Endpoints configure the RabbitMQ transport to use the broker instance running in the `rabbitmq` container:

snippet:TransportConfiguration

### Waiting for RabbitMQ broker to become available

Docker Compose manages the dependencies between the containers and starts the `rabbitmq` container first but there is still a delay between when the RabbitMQ container starts and when the broker starts to accept client connections. Endpoints must wait for the broker to become available before starting the endpoint:

snippet:WaitForRabbitBeforeStart
snippet:WaitForRabbit

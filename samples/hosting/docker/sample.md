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

This sample demonstrates how to use Docker Linux containers to host NServiceBus endpoints communicating over the [RabbitMQ transport](/transports/rabbitmq/). While this sample uses [Docker Compose](https://docs.docker.com/compose/) to demonstrate how to orchestrate a multi-container application, the containers are compatible withe other orchestration technologies, for example [Kubernetes](https://kubernetes.io/docs/home/).

## Running the sample

Running the sample involves building the container images and starting the multi-container application.

### Building container images

Build the container images by using the following commands:

```
dotnet publish Sender --os linux --arch x64 /t:PublishContainer
dotnet publish Receiver --os linux --arch x64 /t:PublishContainer
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

This sample consists of `Sender` and `Receiver` endpoints exchanging messages using the [RabbitMQ transport](/transports/rabbitmq/). Each of these three components runs in a separate Docker Linux container.

### Endpoint containers

The endpoints use the [.NET SDK Container Building Tools](https://github.com/dotnet/sdk-container-builds) to enable the creation of containers via the `dotnet publish` command. See the [Microsoft tutorial](https://learn.microsoft.com/en-us/dotnet/core/docker/publish-as-container?pivots=dotnet-8-0) and [customization documentation](https://github.com/dotnet/sdk-container-builds/blob/main/docs/ContainerCustomization.md) for more details.

### Orchestration

Endpoint container images for the `Sender` and the `Receiver` are combined with an official [RabbitMQ image](https://hub.docker.com/_/rabbitmq/) to create a multi-container application using [Docker Compose](https://docs.docker.com/compose/):

snippet: compose

### Transport configuration

Endpoints configure the RabbitMQ transport to use the broker instance running in the `rabbitmq` container:

snippet: TransportConfiguration

### Waiting for RabbitMQ broker to become available

Both endpoints block startup until the broker becomes available using the shared `ProceedIfBrokerIsAlive` class.

See the [docker documentation for other options to control startup order](https://docs.docker.com/compose/startup-order/).

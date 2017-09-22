---
title: Hosting endpoints in Docker containers
summary: Hosting multiple endpoints in several Docker containers managed by Docker Compose
reviewed: 2017-09-22
component: Core
tags:
- Hosting
- Docker
related:
- nservicebus/hosting
---

## Prerequisites

This sample requires that the following tools are installed:

  - .NET Core 2.0 SDK (link TBD)
  - Docker (link TBD)

## Building and publishing binaries

First step is to build the binaries using the .NET Core command line tools:

```
dotnet build
```

When the binaries have been compiled, the next step is preparing them for deployment into the container:

```
dotnet publish
```

## Building container images

When the binaries are ready for deployment, the next step is to build container images for both the `Sender` and the `Receiver`:

```
docker-compose build
```

## Running containers

When the container images are ready, the containers can be started:

```
docker-compose build
```

## Observing containers

Both containers log to the console. These logs can be inspected:

```
docker-compose logs --tail=10 sender
docker-compose logs --tail=10 receiver
```

## Stopping and removing containers

The containers can be stopped and removed

```
docker-compose down
```

## Code walk-through

TBD

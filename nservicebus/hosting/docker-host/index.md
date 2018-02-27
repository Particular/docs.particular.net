---
title: Docker Container Host
summary: Take advantage of process isolation by hosting endpoints in Docker containers
tags:
 - Hosting
 - Docker
 - Licensing
related:
 - nservicebus/dotnet-templates
 - nservicebus/licensing
 - samples/hosting/docker
reviewed: 2018-02-28
---

Docker containers provide the ability to deploy endpoints in a self contained manner. To create an endpoint to host in a Docker container, download and install the [Docker endpoint template](/nservicebus/dotnet-templates.md), and create a new project using `dotnet new nsbdockerendpt`. The project that is created will have the required endpoint setup infrastructure in addition to the needed `DOCKERFILE` to create and deploy a container hosting one endpoint.

## Licensing
### Compliance
See the [Licensing page](https://particular.net/licensing) for license specifics.

When running Docker containers, the host operating system is considered a node for the purpose of licensing. If there are multiple containers running on the same machine, and those containers host NServiceBus endpoints in each of them, only a one node license is required for the host operating sytem.

### Inclusion
Each Docker container needs to have a `license.xml` file included in it. A placeholder of this file is created when the Docker endpoint template is used to create a new endpoint. This file will need to be replaced with a valid `license.xml` file prior to building the Docker container.

An endpoint running in Docker will look for the `license.xml` file in [the same locations](/nservicebus/licensing) as it would in any other hosting situation.
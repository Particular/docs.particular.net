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

Docker containers provide the ability to deploy endpoints in a self contained manner. To create an endpoint to host in a Docker container, download and install the [Docker endpoint template](nservicebus/dotnet-templates), and create a new project using `dotnet new nsbdockerendpt`. The project that is created will have the required endpoint setup infrastructure in addition to the needed `DOCKERFILE` to create and deploy a container hosting one endpoint.

WARN: At this time only endpoints running in Linux containers are supported. This can either be Linux containers running on a Linux or MacOS host, or Linux containers running on a Windows host. Details about the lack of [Windows container support can be found below](#windows-containers).

## Licensing
See the [Licensing page](https://particular.net/licensing) for license specifics.

Each Docker container needs to have a `license.xml` file included in it. A placeholder of this file is created when the Docker endpoint template is used to create a new endpoint. This file will need to be replaced with a valid `license.xml` file prior to building the Docker container.

An endpoint running in Docker will look for the `license.xml` file in [the same locations](nservicebus/licensing/#license-management) as it would in any other hosting situation.

## Windows Containers
At this time [Windows containers have a known issue](https://github.com/moby/moby/issues/25982) where there is no way for an application to react to the graceful shutdown (`docker stop <containerid>`) of the container. Because of this there is no way for an endpoint host to issue the `endpointInstance.Stop()` command. The result of this is that the endpoint cannot guarantee to properly conclude the processing that is currently in the pipeline which can cause message loss and/or incomplete process execution.

As a result of this limitation, Windows containers are not supported for endpoint hosting at this time.
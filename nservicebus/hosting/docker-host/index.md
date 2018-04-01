---
title: Docker Container Host
summary: Take advantage of process isolation by hosting endpoints in Docker containers
tags:
 - Hosting
related:
 - nservicebus/dotnet-templates
 - nservicebus/licensing
 - samples/hosting/docker
component: Templates
versions: '[2,]'
reviewed: 2018-02-28
---

Docker containers provide the ability to deploy endpoints in a self-contained manner. To create an endpoint to host in a Docker container, install the [Docker endpoint template](/nservicebus/dotnet-templates.md) and create a new project using `dotnet new nsbdockerendpoint`. The project that is created will have the required endpoint setup infrastructure in addition to the needed `Dockerfile` to create and deploy a container hosting one endpoint.


## Template overview

The `nsbdockerendpoint` template creates a project that contains all of the files necessary to build an endpoint that can be deployed to Docker.


### Host.cs

The endpoint's configuration will need to be added to the `Start()` method. 

snippet: DockerStartEndpoint

It is also has a `Stop()` method that can hold any operations that are required to gracefully shutdown the endpoint.

snippet: DockerStopEndpoint

There are also methods that handle endpoint failures and exceptions, which can be modified to fit the needs of the endpoint.

snippet: DockerErrorHandling


### license.xml

Each Docker container needs to have a `license.xml` file included in it. A placeholder of this file is created when the template is used to create a new endpoint. This file will need to be replaced with a valid `license.xml` file prior to building the Docker container.

An endpoint running in Docker will look for the `license.xml` file in [the same locations](/nservicebus/licensing/#license-management) as it would in any other hosting situation. By default, a project created using the `nsbdockerendpoint` template will put the `license.xml` file in the correct location in a Docker image.


### Dockerfile

This file contains the instructions for compiling the endpoint and creating the Docker image.

An endpoint will be hosted in a container that is based on the `microsoft/dotnet:2.0-runtime` image. The container image will contain the compiled artifacts of the endpoint project and will launch that endpoint when the container is run.

To create the Docker image run the following

snippet: docker-build

This command will both compile the endpoint and create the Docker image. Compilation is trigged by the `dotnet publish` command which will restore the endpoint's dependencies (`dotnet restore`), compile the endpoint (`dotnet build`), and finally perform the publish.

snippet: docker-publish

The files generated from these steps are then put in the docker image, the endpoint is configured to start when the container is run, and the container is built.

snippet: docker-entry


### Program.cs

The `Program.cs` file contains the infrastructure required to successfully start an endpoint and gracefully shut it down when the `docker stop` command has been issued.


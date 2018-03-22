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
reviewed: 2018-02-28
---

Docker containers provide the ability to deploy endpoints in a self-contained manner. To create an endpoint to host in a Docker container, download and install the [Docker endpoint template](/nservicebus/dotnet-templates.md) and create a new project using `dotnet new nsbdockerendpoint`. The project that is created will have the required endpoint setup infrastructure in addition to the needed `DOCKERFILE` to create and deploy a container hosting one endpoint.

## Template overview
The `nsbdockerendpoint` template creates a project that contains all of the files necessary to build an endpoint that can be deployed to Docker.
### Host.cs
The endpoint's configuration will need to be added to the `Start()` method. 

snippet: DockerStartEndpoint

It is also has a `Stop()` method that can hold any operations that are required to gracefully shutdown and endpoint.

snippet: DockerStopEndpoint

There are additional methods in place that endpoint failures and exceptions which can be modified to fit the needs of the endpoint.

snippet: DockerErrorHandling

### license.xml
Each Docker container needs to have a `license.xml` file included in it. A placeholder of this file is created when the Docker endpoint template is used to create a new endpoint. This file will need to be replaced with a valid `license.xml` file prior to building the Docker container.

An endpoint running in Docker will look for the `license.xml` file in [the same locations](/nservicebus/licensing/#license-management) as it would in any other hosting situation. By default, a project created using the `nsbdockerendpoint` template will put the `license.xml` file in the correct location in a DOCKER image.

### DOCKERFILE
This file contains the instructions for creating the Docker image. An endpoint will be hosted in a container that is based on the `microsoft/dotnet:2.0-runtime` image. The container image will contain the compiled artifacts of the endpoint project and will launch that endpoint when the container is run.

Building the Docker image first requires the endpoint project to be compiled. Next the image is built by running `docker build` in the project's directory.

By default, the template is designed to create a Docker image using the contents of the `bin/Release/netcoreapp2.0/publish` folder. To build the image the endpoint project needs to be compiled in Release mode. If a Docker image containing Debug artifacts is desired, the endpoint project can be compiled in Debug mode and the DOCKERFILE can be modified to change the `COPY` command to use `bin/Debug/netcoreapp2.0/publish`.

### Program.cs
The `Program.cs` file contains the infrastructure required to successfully start an endpoint and gracefully shut it down when the `docker stop` command has been issued. It also contains code to enable startup and shutdown when the endpoint is run using a debugger. The only change that may be required here is a modification to the `Main` method if using C# 7.1 is desired.

snippet: DockerCSharp71

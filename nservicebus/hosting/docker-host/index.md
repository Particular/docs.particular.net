---
title: Docker Container Host
summary: Take advantage of process isolation by hosting endpoints in Docker containers
related:
 - nservicebus/dotnet-templates
 - nservicebus/licensing
 - samples/hosting/docker
component: Templates
isLearningPath: true
versions: '[2,]'
reviewed: 2023-06-14
---

Docker containers provide the ability to deploy endpoints in a self-contained manner. To create and host an endpoint in a Docker container, use the `dotnet new` template in the [ParticularTemplates package](/nservicebus/dotnet-templates/). The project that is created will have the required endpoint setup infrastructure in addition to the `Dockerfile` needed to create and deploy a container hosting one endpoint.


## Template overview

partial: overview


partial: host


### license.xml

Each Docker container must have a `license.xml` file included in it. A placeholder for this file is created when the template is used to create a new endpoint. This file must be replaced with a valid `license.xml` file prior to building the Docker container.

An endpoint running in Docker will look for the `license.xml` file in [the same locations](/nservicebus/licensing/#license-management) as it would in any other hosting situation. By default, a project created using the `dotnet new` templates will put the `license.xml` file in the correct location in a Docker image.


### Dockerfile

This file contains the instructions for compiling the endpoint and creating the Docker image.

partial: docker-image

To compile the endpoint and create the Docker image, run the following command:

snippet: docker-build

Building the container will compile and publish the endpoint in `Release` mode.

```
RUN dotnet publish -c Release -o /app
```

The compiled endpoint is added to the docker image, the endpoint is configured to start when the container is run, and the container is built.

```
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MyEndpoint.dll"]
```


partial: program

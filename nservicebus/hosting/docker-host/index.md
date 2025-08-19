---
title: Docker Container Host
summary: Take advantage of process isolation by hosting endpoints in Docker containers.
related:
  - nservicebus/dotnet-templates
  - nservicebus/licensing
  - samples/hosting/docker
component: Templates
isLearningPath: true
versions: '[2,]'
reviewed: 2025-05-19
---

Hosting endpoints in Docker containers provides self-contained artifacts that can be deployed to multiple environments or managed by orchestration technologies such as [Kubernetes](https://kubernetes.io/docs/home/). To create and host an endpoint in a Docker container, use the `dotnet new` template from the [ParticularTemplates package](/nservicebus/dotnet-templates/). The generated project includes all required endpoint setup infrastructure, along with a `Dockerfile` needed to build and deploy a container hosting one endpoint.

## Template overview

partial: overview

partial: host

### license.xml

Each Docker container must include a `license.xml` file. A placeholder for this file is created when the template is used to generate a new endpoint project. Before building the Docker container, this placeholder must be replaced with a valid `license.xml`.

An endpoint running in Docker will look for the `license.xml` file in [the same locations](/nservicebus/licensing/#license-management) as it would in any other hosting scenario. By default, the `dotnet new` template places the `license.xml` in the correct location within the Docker image.

### Dockerfile

This file contains the instructions for compiling the endpoint and creating the Docker image.

partial: docker-image

To compile the endpoint and build the Docker image, run the following command:

snippet: docker-build

This step compiles and publishes the endpoint in `Release` mode:

```
RUN dotnet publish -c Release -o /app
```

The compiled endpoint is then added to the Docker image, and the container is configured to start the endpoint when it runs:

```
COPY --from=build /app .
ENTRYPOINT ["dotnet", "MyEndpoint.dll"]
```

partial: program

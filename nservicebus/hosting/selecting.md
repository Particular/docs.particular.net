---
title: Selecting a host
summary: A guide for selecting an NServicebus transport.
reviewed: 2019-07-15
isLearningPath: true
---
This document provides guidance for deciding how to [host](/nservicebus/hosting) [NServiceBus endpoints](/nservicebus/endpoints/).

This guide does not provide definitive answers for all situations and each option involves trade-offs. Particularly in cloud scenarios, there are many options as to what the best solution may be. If it is unclear what the best choice is, or there are very specific constraints, contact [Particular Software](https://particular.net/contactus) for advice.

NServiceBus endpoints can be hosted within any .NET process. This guidance groups the hosting options into:

 - [On-premises](#on-premises)
 - [Containers](#containers)
 - [Microsoft Azure](#microsoft-azure)
 - [Amazon Web Services](#amazon-web-services)

## On-premises

For on-premises hosting, endpoints are typically hosted in background processes running on servers, which are usually virtual machines. An endpoint can also be hosted in an interactive application with a user interface, but this guide focuses on server scenarios.

### Windows Services

In Windows, a [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) is the most common way to host an NServiceBus endpoint.

Windows Services run in the background, can immediately start when Windows is started, can be paused and restarted, and support [recoverability options](/nservicebus/hosting/windows-service.md#installation-setting-the-restart-recovery-options-configuring-service-recovery-via-windows-service-properties).

See [Windows Service Hosting](/nservicebus/hosting/windows-service.md) for details.


### Internet Information Services (IIS)

In Windows, IIS is a reliable host for web-based applications. An NServiceBus endpoint can be hosted in any .NET web application, including one running in IIS. However, the purpose of IIS is HTTP request-based hosting. That means IIS will automatically shut down any web application that has not received a request for some time.

This restricts IIS as a choice for hosting NServiceBus endpoints to two specific scenarios:

* [Send-only endpoints](/nservicebus/hosting/#self-hosting-send-only-hosting), which can send messages but do not receive any messages, and therefore do not need to initialize any receive infrastructure. Messages are sent during the handling of incoming HTTP requests or after user input in an interactive application.
* Web applications which provide [near real-time feedback](/samples/near-realtime-clients/) using queues for asynchronous and reliable communication.

See [Web Application Hosting](/nservicebus/hosting/web-application.md) for details.

### Linux background processes

In Linux, a background process is typically controlled by a system service controller. One of the most commonly used is [systemd](https://freedesktop.org/wiki/Software/systemd/). These controllers can be configured to start and stop any executable, typically a console app, as a background process when the operating system starts and shuts down, as well as more complex configurations.

## Containers

This section focuses on Docker, as one of the most well known container technologies.

The biggest difference in hosting NServiceBus endpoints using Docker, compared to a regular host OS, is that applications are isolated from the host, and other containers. This can be beneficial with respect to security. Another benefit is portability. It is easy to move containers from development to test and production.

Although Docker containers are popular, the tooling and guidance are less developed than with other hosting solutions. It may be necessary to consider whether an organization has the operational capability to support a container technology such as Docker.

See [Docker Container Host](/nservicebus/hosting/docker-host/) for details.

## Microsoft Azure

Azure offers a variety of solutions which can potentially host NServiceBus endpoints. Unfortunately, none of them are specifically designed to run continuous background processes, similar to Windows Services. This makes it challenging to choose the best hosting options for NServiceBus endpoints. For assistance, contact [Particular Software](https://particular.net/contactus).

The primary options for hosting NServiceBus endpoints are the following:

### AppServices

Within AppServices, [WebJobs](https://docs.microsoft.com/en-us/azure/app-service/webjobs-create) can host background processes. This is currently the best proven solution for hosting NServiceBus endpoints.

### Azure Functions

Azure Functions can be used to run short-lived NServiceBus endpoints triggered by Azure Service Messages. When a message triggers a function, an NServiceBus endpoint can be started to handle the message.

Starting an NServiceBus endpoint for each message adds considerable overhead. Particular Software is aware of this and is working on a better solution for using NServiceBus with Azure Functions.

### Service Fabric

Service Fabric works on top of Virtual Machine Scale Sets to provide clustered, stateful services. If dynamic scaling and clustering is a requirement, Service Fabric can be a good option to host NServiceBus endpoints.

### Cloud Services

Cloud Services provide worker roles for background processes. With the introduction of Azure AppServices, with its extended features, Cloud Services is less likely to be a good choice for NServiceBus endpoints.

## Amazon Web Services

Although there is a [comparison chart](https://docs.microsoft.com/en-us/azure/architecture/aws-professional/services#miscellaneous) that compares Amazon Web Services (AWS) to Microsoft Azure, AWS does not provide any comparable alternatives for hosting background processes. That leaves the following:

### Virtual Machines

AWS virtual machines provide the same options as those described in [on-premises](#on-premises).

### Containers

AWS containers provide the same options as those described in [containers](#containers) section.

### AWS Mesh

AWS Mesh is comparable to Azure [Service Fabric](#microsoft-azure-service-fabric).

---
title: Selecting a host
summary: A guide for selecting a host for NServiceBus endpoints.
reviewed: 2024-08-02
isLearningPath: true
---
This document provides guidance for deciding how to [host](/nservicebus/hosting) [NServiceBus endpoints](/nservicebus/endpoints/).

The guide does not provide definitive answers for all situations, and each option involves trade-offs. Several options exist, particularly in cloud scenarios, and the best solution depends on the requirements of individual scenarios. If it is unclear what the best choice is or there are specific constraints, contact [Particular Software](https://particular.net/contactus) for advice.

NServiceBus endpoints can be hosted in any .NET process. This guidance groups the hosting options into:

- [On-premises](#on-premises)
- [Containers](#containers)
- [Microsoft Azure](#microsoft-azure)
- [Amazon Web Services](#amazon-web-services)

## On-premises

For on-premises hosting, endpoints are typically hosted in background processes running on servers, which are usually virtual machines. An endpoint can also be hosted in an interactive application with a user interface, but this guide focuses on server scenarios.

### Windows Services

In Windows, a Windows Service is the most common way to host an NServiceBus endpoint.

Windows Services run in the background, can immediately start when Windows is started, can be paused and restarted, and support [recoverability options](/nservicebus/hosting/windows-service.md#installation-setting-the-restart-recovery-options-configuring-service-recovery-via-windows-service-properties).

See [Windows Service Hosting](/nservicebus/hosting/windows-service.md) for details.

### Internet Information Services (IIS)

In Windows, IIS is a reliable host for web-based applications. An NServiceBus endpoint can be hosted in any .NET web application, including one running in IIS. However, IIS's purpose is HTTP request-based hosting. That means IIS will automatically shut down any web application that has not received a request for some time.

This restricts IIS as a choice for hosting NServiceBus endpoints to two specific scenarios:

* [Send-only endpoints](/nservicebus/hosting/#self-hosting-send-only-hosting), which can send but don't receive messages and, therefore, don't need to initialize any receiving infrastructure. Messages are sent while handling incoming HTTP requests or after user input in an interactive application.
* Web applications which provide [near real-time feedback](/samples/near-realtime-clients/) using queues for asynchronous and reliable communication.

See [Web Application Hosting](/nservicebus/hosting/web-application.md) for details.

### Linux background processes

In Linux, a background process is typically controlled by a system service controller. One of the most commonly used is [systemd](https://freedesktop.org/wiki/Software/systemd/). These controllers can be configured to start and stop any executable, typically a console app, as a background process when the operating system starts and shuts down, as well as more complex configurations.

## Containers

One difference in hosting NServiceBus endpoints using containers compared to a regular host OS is that applications are isolated from the host and other containers. This has security and portability benefits. It is easy to move containers from development to test and production.

See [Docker Container Host](/nservicebus/hosting/docker-host/) for details on hosting endpoints in Docker.

## Microsoft Azure

Azure offers various solutions for hosting NServiceBus endpoints. Unfortunately, none of them are specifically designed to run continuous background processes, similar to Windows Services. So, it can be challenging to choose the best hosting options for NServiceBus endpoints. For assistance, contact [Particular Software](https://particular.net/contactus).

The primary options for hosting NServiceBus endpoints are:

### AppServices

Within AppServices, [WebJobs](https://docs.microsoft.com/en-us/azure/app-service/webjobs-create) can host background processes. This is the recommended solution for hosting NServiceBus endpoints.

### Azure Functions

Azure Functions can run NServiceBus endpoints in a serverless and dynamically scaled environment. See [Azure Functions with Azure Service Bus](/nservicebus/hosting/azure-functions-service-bus/) for more details.

### Service Fabric

Service Fabric works on top of Virtual Machine Scale Sets to provide clustered, stateful services. If dynamic scaling and clustering are required, Service Fabric can be a good option for hosting NServiceBus endpoints.

## Amazon Web Services

Although there is a [comparison chart](https://docs.microsoft.com/en-us/azure/architecture/aws-professional/services#miscellaneous) that compares Amazon Web Services (AWS) to Microsoft Azure, AWS does not provide any comparable alternatives for hosting background processes. That leaves the following options for Amazon Web Services:

### AWS Lambda

AWS Lambda can run NServiceBus endpoints in a serverless and dynamically scaled environment. See [AWS Lambda with SQS](/nservicebus/hosting/aws-lambda-simple-queue-service/) for more details.

### Virtual Machines

AWS virtual machines provide the same options as those described in [on-premises](#on-premises).

### Containers

AWS containers provide the same options described in the [containers](#containers) section.

### AWS Mesh

AWS Mesh is comparable to Azure [Service Fabric](#microsoft-azure-service-fabric).

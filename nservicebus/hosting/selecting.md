This document provides guidance for deciding how to [host](/nservicebus/hosting) your [NServiceBus endpoints](/nservicebus/endpoints/).

This guide does not provide definitive answers for all scenarios, and every decision involves trade-offs. Especially in cloud scenarios, there is currently no definitive answer on what the best solutions are for many scenarios. If it is unclear what the best choice is, or there are very specific constraints, contact [Particular Software](https://particular.net/contactus).

NServiceBus endpoints can be hosted within any .NET process. This guidance groups the hosting options into:

 - [On-premises](#on-premises)
 - [Windows Azure](#windows-azure)
 - [Amazon Web Services](#amazon-web-services)

## On-premises

For on-premises hosting, endpoints are typically hosted in background process running on servers, which are usually virtual machines. An endpoint can also be hosted in an interactive application, with a user interface, but this guidance will focus on server scenarios.

### Windows Services

On Windows, a [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) is the most common way to host NServiceBus endpoints.

The benefit is that Windows Services run in the background, immediately start when Windows is booted, can be paused and restarted, and support [recoverability options](/nservicebus/hosting/windows-service#installation-setting-the-restart-recovery-options-configuring-service-recovery-via-windows-service-properties).

### Internet Information Services (IIS)

On Windows, IIS is a reliable host for [web-based applications](/nservicebus/hosting/web-application). An NServiceBus endpoint can be hosted within any .NET web application, including those running in IIS. However, the focus of IIS is request based hosting, which means IIS will automatically shut down anything that has not received a request for a while. This is also true for any NServiceBus based endpoint.

This restrict IIS as a choice for hosting NServiceBus endpoints to two specific scenarios:

#### Send-only endpoints

A "send-only" endpoint is one which sends messages but does not receive any messages. Message are sent as the result of incoming HTTP requests.

#### Near real-time feedback

Some web applications give [near real-time feedback](/samples/near-realtime-clients/) using queues for asynchronous and reliable communcation.

## Windows Azure

Windows Azure offers a variety of solutions which can potentially host NServiceBus endpoints. Unfortunately, none of them are currently specifically designed to run continuous background processes, similar to Windows Services. This makes it challenging to choose the best hosting options for NServiceBus endpoints. For assistance, contact [Particular Software](https://particular.net/contactus).

The primary options for hosting NServiceBus endpoints are the following:

### AppServices

Within AppServices, [WebJobs](https://docs.microsoft.com/en-us/azure/app-service/webjobs-create) can host background processes. This is currently the best proven solution for hosting NServiceBus endpoints.

Todo: What are the drawbacks?

### Azure Functions

Azure Functions can be used to run short-lived NServiceBus endpoints that are triggered by Azure Service Messages. When a message triggers a function, an NServiceBus endpoint can be started to handle the message.

Starting an NServiceBus endpoint for each message add considerable overhead. Particular Software is aware of this and is working on a better solution for using NServiceBus with Azure Functions.

### Service Fabric

Service Fabric works on top of Virtual Machine scale sets to provide clustered, stateful services. If dynamic scaling and clustering is a requirement, Service Fabric may be a good option to host NServiceBus endpoints.

### Cloud Services

Cloud Services provide worker roles for background processes. With the introduction of Azure AppServices, with its extended features, Cloud Services is less likely to be a good choice for NServiceBus endpoints.

## Amazon Web Services

Todo: If the above is something we can live with, repeat the above for AWS.

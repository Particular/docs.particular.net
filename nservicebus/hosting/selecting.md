This document provides guidance for deciding how to [host](/nservicebus/hosting) your [NServiceBus endpoints](/nservicebus/endpoints/).

This guide does not provide definitive answers for all scenarios. Every decision has trade-offs. Especially in cloud scenarios, there is currently not a definitive answer on what the best solution is. If it is unclear what the best choice is, or there are very specific constraints, contact [Particular Software](https://particular.net/contactus).

Since NServiceBus endpoints can be hosted within any .NET process, this guidance will differentiate the different ones between the following topics:

 - [On premise](#on-premise-hosting)
 - [Docker containers](#docker-hosting)
 - [Windows Azure](#windows-azure-hosting)
 - [Amazon Web Services](#amazon-web-services-hosting)

## On Premise hosting
With on premise hosting the focus is mainly on (virtual) machine with the Windows operating system installed. Although an endpoint can even be hosted within a console application, this guidance will focus on the following options.

### Windows Services
For on premise solutions a [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) is the most common way to host NServiceBus endpoints.

The benefit is that Windows Services run in the background, immediately start when Windows is booted and can be paused and restarted and support [recoverability options](/nservicebus/hosting/windows-service#installation-setting-the-restart-recovery-options-configuring-service-recovery-via-windows-service-properties).

### Internet Information Services (IIS)
With Windows machines, IIS is a reliable host for [web based applications](/nservicebus/hosting/web-application). An NServiceBus endpoint can be hosted within any .NET web application and thus inside IIS. However, the focus of IIS is request based hosting, which means IIS will automatically shut down anything that has not received a request for a while. This is also true for any NServiceBus based endpoint.

This makes IIS less of an option to host NServiceBus endpoints, except for two very specific scenarios. One is where NServiceBus will act as a 'SendOnly' endpoint. The result is that it will not process any message, but will only send messages after user interaction within the web application. Another scenario is where the web application might give [near real-time feedback](/samples/near-realtime-clients/) using queues for asynchronous and reliable communcation.

## Docker hosting
With .NET Core, Microsoft made it possible to host applications in other environments than Windows. One of the most well known options is [Docker containers](https://www.docker.com/resources/what-container).

The biggest differences of hosting NServiceBus endpoints using Docker, as compared to Windows, is that applications are isolated from the host and other containers. This includes other applications but also any security settings.
Another benefit is the portability of containers, where it is possible and easy to move containers from development to test and production.

Although Docker containers are very popular, the tooling, guidance and guidance on the internet are still not as good as with other hosting solutions. A consideration should be if the operations department is able to support a solution like Docker containers.

## Windows Azure hosting
With Windows Azure there is a variety of solutions one can choose from to host NServiceBus endpoints. Unfortunately none of them focus on running background applications like Windows Services do. As a result, selecting a technology to host an NServiceBus endpoint in Windows Azure is not an easy task. Contact [Particular Software](https://particular.net/contactus) to discuss the different options.

The primary options to host any NServiceBus endpoint are the following:

### Azure AppServices
Within Azure AppServices, Microsoft offers [Azure WebJobs](https://docs.microsoft.com/en-us/azure/app-service/webjobs-create) to host background processes. This is currently the best mainstream solution for hosting your endpoints.

Todo: What are the drawbacks?

### Azure Functions
With Azure Functions, Microsoft offers another solution for running short-lived endpoints that can be triggered by Azure Service Messages. As each message triggers execution of code, NServiceBus instances are started for each message. Which makes Azure Functions not a great fit at this moment.

Particular Software is aware of this and working on a solution that better fits Azure Functions.

### Azure Service Fabric
With Service Fabric, Microsoft build a solution on top of Virtual Machine scale sets to provide clustered, stateful services. If dynamic scaling and clustering is a requirement, Service Fabric can be an option to host (stateful) NServiceBus endpoints.

### Azure Cloud Services
Cloud Services provide worker roles for background processes. With the introduction of Azure AppServices, with its extended features, Cloud Services is less likely to be a better solution for NServiceBus endpoints.

## Amazon Web Services hosting
Todo: If the above is something we can live with, repeat the above for AWS.

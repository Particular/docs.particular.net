---
title: Azure Computing Options
summary:
reviewed: 2023-07-18
---

Azure offers a wide range of different hosting options that will host and run application code. The available options range from full control of the machine, including the operating system, to fully managed serverless services.

## Hosting models

![](azure-compute-overview.png)

NServiceBus can be used across different Azure services hosting models:

### Serverless

Also described as _Functions as a service_ (FaaS). In serverless models, application code is deployed to the service which automatically runs it.

[Azure Functions](https://azure.microsoft.com/en-us/products/functions) is Azure's serverless hosting model. NServiceBus supports integration with Azure Functions that allows deployment of new or existing NServiceBus applications into serverless environments with minimal effort. NServiceBus applications on Azure Functions can directly consume messages from Azure Service Bus. [Other Azure Functions triggers](https://learn.microsoft.com/en-us/azure/azure-functions/functions-triggers-bindings?tabs=csharp) can also use NServiceBus to send messages to Azure Service Bus.

[**Host NServiceBus applications on Azure Functions →**](/nservicebus/hosting/azure-functions-service-bus/)

### Platform as a Service

_Platform as a Service_ (PaaS) models provide managed hosting environments where applications can be deployed to without having to manage the underlying infrastructure, operating system or runtime environments.

#### Azure App Services

[Azure App Services](https://azure.microsoft.com/en-us/products/app-service/) supports deplpoyment of .NET applications natively. NServiceBus can be integrated into [ASP.NET Core web applications](/nservicebus/hosting/asp-net.md) or run as dedicated [background task using WebJobs](https://learn.microsoft.com/en-us/azure/app-service/webjobs-create).

#### Containers

Containers are a popular mechanism to deploy and host applications in PaaS services. NServiceBus can be used by containerized applications and deployed to services like:
* [Azure App Services](https://azure.microsoft.com/en-us/products/app-service/) using containers
* [Azure Container Instances](https://azure.microsoft.com/en-us/products/container-instances/)
* [Azure container Apps](https://azure.microsoft.com/en-us/products/container-apps/)
* [Azure Kubernetes Services](https://azure.microsoft.com/en-us/products/kubernetes-service/)

[**Host NServiceBus applications in containers →**](/nservicebus/hosting/docker-host/)

#### Service Fabric

[Azure Service Fabric](https://azure.microsoft.com/en-us/products/service-fabric/) is a distributed systems platform by Microsoft, designed for the development, deployment, and management of microservices and containerized applications. It handles the intricacies of partitioning, scaling, and maintaining these applications, supporting stateful and stateless services with built-in load balancing and resilience. It's particularly useful for crafting applications that demand high availability, low latency, and streamlined updates, and it can be used both in cloud environments like Azure and on-premises setups. It handles the intricacies of partitioning, scaling, and maintaining these applications, supporting stateful and stateless services with built-in load balancing and resilience.

[**Host NServiceBus applications in Azure Service Fabric →**](/nservicebus/hosting/service-fabric-hosting/)

### Infrastructure as a Service

_Infrastructure as a Service_ (IaaS) Provides virtualized computing resources like virtual machines, storage and networking that can be used to build and manage the required infrastructure.

NServiceBus applications can easily be hosted on virtual machines. Popular techniques include:
* [Integrating NServiceBus with the Microsoft Generic Host](/nservicebus/hosting/extensions-hosting.md)
* [Custom hosted web applications](/nservicebus/hosting/web-application.md)
* [Installing NServiceBus endpoints as Windows Services](/nservicebus/hosting/windows-service.md).
* [Manually controlling NServiceBus lifecycle in an executable (e.g. Console or GUI applications)](/nservicebus/hosting/#self-hosting)
* [Custom-managed Kubernetes clusters hosting container applications](/nservicebus/hosting/docker-host)


## Chosing a hosting moel

The right hosting option may depend on desired characteristics like:

* **Scalability**: Different hosting options offer different approaches to scaling. Managed solutions are typically easier to scale on demand and can scale in more granular levels. In addition to the scalability itself, elasticity (the time required to scale up or down) might also be a decision criteria. Refer to [Azure subscription and service limits](https://learn.microsoft.com/en-us/azure/azure-resource-manager/management/azure-subscription-service-limits) to better understand scalability limits and constraints.
* **Pricing:** Managed services typically offer more dynamic pricing models that adjust to the actual consumption of the application compared to more fixed pricing models for infrastructure services. However, managed services typically charge more for their pricing units, making infrastructure highly competitive for consistent demand. Refer to the service's documentation or use the [Azure pricing calculator](https://azure.microsoft.com/en-us/pricing/calculator/) to better understand a service's pricing model.
* **Portability:** Serverless models are primarily built on proprietery programming models heavily tied to the cloud service vendor. Hosting models built on open standards make it easier to run applications in other hosting environments. Additionally, consider whether the software should also be able to run in on-premises hardware or on local machines.
* **Flexibility:** Lower-level infrastructure provides more control over the configuration and management of applications. Serverless offerings offer less flexibility due to higher levels of abstractions exposed to the developers. 
* **Manageability:** Serverless and PaaS models can hide a lot of the underlying infrastructure challenges (e.g. automatic scaling, OS updates, load balancing, etc.), typically at the cost of flexibility. Managing and maintaining infrastructure might require signficant amount of resources and knowledge.

For further information about Azure hosting options, refer to [Microsoft's compute service overview](https://learn.microsoft.com/en-us/azure/architecture/guide/technology-choices/compute-decision-tree).

## Additional resources

* [Explore all available NServiceBus hosting options](/nservicebus/hosting/selecting.md).
* [Azure compute option for microservices](https://learn.microsoft.com/en-us/azure/architecture/microservices/design/compute-options) 
* [Azure pricing](https://azure.microsoft.com/en-us/pricing/)
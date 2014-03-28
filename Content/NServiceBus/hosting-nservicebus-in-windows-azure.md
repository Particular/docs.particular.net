---
title: Hosting NServiceBus in Windows Azure
summary: Overview on using Windows Azure Cloud Services, websites, and virtual machines to host NServiceBus, with links to detailed articles.
tags: 
- Windows Azure
- Cloud
---

The Windows Azure platform and NServiceBus are a perfect fit. The Azure platform offers the scalable and flexible platform for your designs, while NServiceBus makes development on this highly distributed environment a breeze.

Windows Azure offers various ways to host applications. Each of these hosting options can be used in the context of NServiceBus, but there are some things to keep in mind for each.

General Considerations
----------------------

Because of the size and service nature of the Windows Azure platform, you cannot rely on distributed transactions in this environment. Therefore, you cannot rely on any setup that would require distributed transactions, including the MSMQ transport. For details, refer to ['Understanding transactions in Windows Azure'](/nservicebus/understanding-transactions-in-windows-azure).

Windows Azure Virtual Machines
------------------------------

The Virtual Machines hosting model is similar to any other virtualization technology in your datacenter. Machines are created from a virtual machine template, you are responsible for managing their content, and any change you make to them automatically persists in Windows Azure storage services.

The installation model is therefore also the same as any on-premise NServiceBus project. Use `NServiceBus.Host.exe` to run your endpoint, or use the `Configure` API to self-host your endpoint, for example, in a website.

The main difference, as outlined above, is that you should not rely on any technology that itself depends on 2PC. In other words, MSMQ is not a good transport in this environment. Instead, use `AzureStorageQueues` or `AzureServiceBus`. Other options include deploying `RabbitMQ` or another non-DTC transport to an Azure Virtual Machine.

For more information about enabling the Azure storage queues or Azure Service Bus transports, refer to the following documentation:

* [Azure storage queues](/nservicebus/using-azure-storage-queues-as-transport-in-nservicebus)
* [Azure Service Bus](/nservicebus/using-azure-servicebus-as-transport-in-nservicebus)

For persistence you can rely on any option, including RavenDB, SQL Server installed on a Virtual Machine, SQL Azure or Windows Azure storage services.


Windows Azure Websites
----------------------

Another deployment model is Windows Azure Websites, where you use a regular website and push it to your favorite source control repository (like GitHub).  On your behalf, Microsoft takes the latest issue from the repository, builds the binaries, runs your tests, and deploys to production.

As for an NServiceBus programming model, this is roughly the same as any other self-hosted endpoint in a website. You use the `Configure` API to set things up and it will work.

The only quirk in this model is that Windows Azure website is built with cheap hosting in mind. By default, its technology puts your website in suspended mode when there is no traffic. This also implies that if you have an NServiceBus endpoint hosted here, it is also suspended and stops processing messages. However, the 'Always on' feature periodically sends requests to your website to keep it active. This feature requires standard mode and is not available in the free edition. 

The advised transports in this environment are `AzureStorageQueues` and `AzureServiceBus`. As it is impossible to include these websites in the same virtual network as Windows Azure virtual machines, it is impossible to use any other hosted transport such as RabbitMQ, unless you don't mind offering your transport infrastructure through a public endpoint.

The same applies to the persistence infrastructure if you select an 'as a service' option such as Windows Azure storage or SQL Azure, as you cannot put the websites in the same virtual network as your hosted infrastructure.

To learn more about enabling persistence with Windows Azure storage, refer to [Azure storage persisters](/nservicebus/using-azure-storage-persistence-in-nservicebus).


Cloud Services 
--------------

The third hosting model available on the Windows Azure platform is 'Cloud Services'. In this hosting model, which is intended for applications with huge scalability demands, you define a layout for your application in a service definition file. 

This layout is based on a concept called `Roles`. Roles define what a specific set of machines should look like, where all should be identical to what is defined in the role. By default, one NServiceBus endpoint translates to a role, meaning that it will be hosted by multiple identical machines at the same time. You specify how many machines should be in each role when deployed (we advise at least two), but the Windows Azure platform will manage them for you, automatically monitoring and updating the machines.

But it does this in a very particular way! To ensure an identical set of machines (i.e., identical to the role template) it will simply destroy a machine and install a new one. This means that anything that was on the disk of a machine will be lost! This fact makes any transport or persistence option that relies on a disk unsuitable for this environment, including MSMQ, RabbitMQ, ActiveMQ, SQL Server, RavenDB, and so on.

The advised transports in this environment are `AzureStorageQueues` or `AzureServiceBus`, and the Windows Azure storage persisters for persistance purposes.

**NOTE:** It is possible to put Cloud Services and Virtual Machines in the same virtual network, so a hybrid architecture with some of the above transports and storage options might still be suitable (as long as you don't rely on the DTC).

Next to your endpoint, the role definition will also include additional services that are deployed to the role instances, of which the most important for your application are these:

* Configuration system: This system allows you to update configuration settings from the Windows Azure management portal, or through the service management API, and the platform will promote these configuration settings to all instances in your roles without downtime.
* Diagnostics service: This system allows you to collect diagnostics information from the different role instances (application logs, event logs, performance counters, etc.) and aggregate them in a central storage account.

To integrate these facilities with your endpoint code, we have provided a specific `NServiceBusRoleEntrypoint` that wires our regular host into a role entrypoint. In addition, there are specific NServiceBus `Roles` (not to be confused with Azure roles) such as `AsA_Worker` and `Profiles`, similar to `Development` or `Production` in the `NServiceBus.Hosting.Azure` package.

To learn more about the details of hosting in windows azure cloud services, refer to [Cloud Services](/nservicebus/hosting-nservicebus-in-windows-azure-cloud-services).


Cloud Services - Shared Hosting
-------------------------------

The Cloud Services model is beautiful when it comes to building large scale systems, but in reality only a few systems need size from the very beginning, and find this model quite expensive. Most want to start out small and cheap, then grow larger over time. 

To support this need to start small, we also provide a shared hosting option, using the `AsA_Host` role. In this model, the role entry point doesn't actually host an endpoint itself. Instead, it downloads, invokes, and manages other worker role entry points as child processes on the same machine.

If you want to learn more about the shared hosting options, please refer to [Cloud Services - Shared hosting](/nservicebus/shared-hosting-nservicebus-in-windows-azure-cloud-services).


Sample
------

To see these hosting models in action, refer to the [Video store samples.](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/).

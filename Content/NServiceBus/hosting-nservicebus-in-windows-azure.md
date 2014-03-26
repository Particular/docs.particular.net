---
title: Hosting NServiceBus in Windows Azure
summary: Using Windows Azure Cloud Services, Websites and virtual machines to host NServiceBus.
tags: []
---

The Windows Azure Platform and NServiceBus make a perfect fit. On the one hand the azure platform offers us the scalable and flexible platform that we are looking for in our designs, on the other hand NServiceBus makes development on this highly distributed environment a breeze.

Windows Azure offers various ways to host applications, each of these hosting options can be used in context of NServicebus, but there are some things to keep in mind for each of them.

General Consideration
-----------------

Because the size and service nature of the windows azure platform, you cannot rely on distributed transactions in this environment and therefore you cannot rely on any setup that would require distributed transactions (this includes the msmq transport). For more information on this topic please consult ['understanding transactions in windows azure'](/nservicebus/understanding-transactions-in-windows-azure)

Windows Azure Virtual Machines
-----------------

The Virtual Machines hosting model, is much the same as any other virtualization technology that you may have in your own datacenter. Machines are created from a virtual machine template, you are responsible for managing their content and any change you make to them is persisted in windows azure storage services automatically.

The installation model is therefore also the same as any on premise nservicebus project, use `NServiceBus.Host.exe` to run your endpoint, or use the `Configure` api to self host your endpoint in for example a website.

The main difference, as outlined above, is that you should not rely on any technology that itself depends on 2PC. In other words, Msmq is not a good transport in this environment, it's better to go with `AzureStorageQueues` or `AzureServiceBus` instead, other options include deploying `RabbitMQ` or other non-DTC transport to an azure Virtual Machine.

For more information about enabling the azure storage queues or azure servicebus transports, see following documentation:

* [Azure storage queues](/nservicebus/using-azure-storage-queues-as-transport-in-nservicebus)
* [Azure servicebus](/nservicebus/using-azure-servicebus-as-transport-in-nservicebus)

For persistence you can rely on any option, including RavenDB, SQL Server installed on a Virtual Machine, Sql Azure or Windows Azure storage services.


Windows Azure Websites
-----------------

Another interesting deployment model is called Windows Azure Websites. In this deployment model, you can use a regular website, push it to your favorite source control repository (like github), and Microsoft will take it from there. They will get the latest version, build the binaries, run your tests and deploy to production on your behalf.

As from an NServiceBus programming model, this is roughly the same as any other self hosted endpoint in a website, you use the `Configure` api to set things up and it will work.

The only quirk in this model though, is that Windows Azure Websites has been built with 'cheap hosting' in mind. It comes with technology that will put your website in a suspended mode, by default, when there is no traffic. This also implies that if you have an NServiceBus endpoint hosted here, it will also become suspended and stop processing messages. You can however enable 'Always on' mode which will periodically send requests to your website in order to keep it alive. This feature requires standard mode and is not available in the free edition. 

The advised transports in this environment are `AzureStorageQueues` or `AzureServiceBus`. As it is impossible to include these websites in the same virtual network as windows azure VM's, it is impossible to use any other hosted transport like RabbitMQ, unless you're fine with offering your transport infrastructure through a public endpoint.

The same applies to the persistence infrastructure, you probably want to go for an 'as a service' option like windows azure storage or sql azure as you cannot put the websites in the same virtual network as your hosted infrastructure.

To learn more about enabling persistence with windows azure storage, check out:

* [Azure storage persisters](/nservicebus/using-azure-storage-persistence-in-nservicebus)


Cloud Services 
-----------------

The third hosting model available on the windows azure platform is called 'Cloud Services'. In this hosting model, which is intended for applications with huge scalability demands, you define a layout for your application in a service definition file. 

This layout is based on a concept called `Roles`. Roles define what a specific set of machines should look like, all should be identical to what is defined in the role. By default one nservicebus endpoint will translate to a role, meaning that it will be hosted by multiple, identical, machines at the same time. You specify how many machines there should be in each role when deployed, advised is at least 2, but the windows azure platform will manage them for you, it will automatically monitor and update the machines for you.

But it does this in a very particular way! To ensure an identical set of machines (identical to the role template that is) it will simply destroy a machine and install a new one. This implies that anything that was put on the disk of a machine will be lost! This fact makes any transport or persistence option that relies on a disk unsuitable for this environment, including msmq, rabbitmq, activemq, sql server, ravendb, ...

The advised transports in this environment are `AzureStorageQueues` or `AzureServiceBus`, and the windows azure storage persisters for persistance purposes.

**Note** that it is possible to put Cloud Services and Virtual Machines in the same virtual network, so a hybrid architecture with some of the above transports and storage options might still be suitable (as long as you don't rely on the DTC).

Next to your endpoint, the role definition will also include additional services that will be deployed to the role instances, of which the most important for your application are:

* Configuration system: This system allows you to update configuration settings from the windows azure management portal, or through the service management API, and the platform will promote these configuration settings to all instances in your roles without downtime.
* Diagnostics service: This system allows you to collect diagnostics information from the different role instances (application logs, event logs, performance counters, etc..) and aggregate them in a central storage account.

To integrate these facilities with your endpoint code, we have provided a specific `NServiceBusRoleEntrypoint` that wires our regular host into a role entrypoint, furthermore there are specific NServiceBus `Roles` (not to be confused with azure roles) like `AsA_Worker` and `Profiles` like `Development` or `Production` in the `NServiceBus.Hosting.Azure` package.

To learn more about the details of hosting in windows azure cloud services, refer to:

* [Cloud Services](/nservicebus/hosting-nservicebus-in-windows-azure-cloud-services)


Cloud Services - Shared hosting
-----------------

The cloud services model is beautiful when it comes to building large scale systems, but in reality only a few systems need size from the very beginning, and find this model quite expensive. Most want to start out small & cheap and than grow larger over time. 

To support this need to start small, we also provide a shared hosting option, using the `AsA_Host` role. In this model, the role entry point doesn't actually host an endpoint itself, but instead it downloads, invokes and manages other worker role entry points as child processes on the same machine.

If you want to learn more about the shared hosting options, please refer to:

* [Cloud Services - Shared hosting](/nservicebus/shared-hosting-nservicebus-in-windows-azure-cloud-services)


Sample
------

Want to see these hosting models in action? Checkout the [Video store samples.](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/).

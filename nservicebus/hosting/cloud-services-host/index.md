---
title: Hosting in Azure Cloud Services
summary: Using Azure Cloud Services to host NServiceBus.
tags:
 - Hosting
 - Worker Roles
 - Web Roles
 - Azure
 - Cloud
 - Logging
redirects:
 - nservicebus/hosting-nservicebus-in-windows-azure-cloud-services
 - nservicebus/azure/hosting-nservicebus-in-windows-azure-cloud-services
 - nservicebus/windows-azure-transport
related:
 - samples/azure/shared-host
 - nservicebus/lifecycle
---

The Azure Platform and NServiceBus make a perfect fit. On the one hand the Azure platform offers the scalable and flexible platform required, on the other hand NServiceBus makes development on this highly distributed environment a breeze.

If real scale is required (eg in tens, hundreds or even thousands of machines hosting each endpoint) then Cloud Services is the required deployment model.

## Cloud Services - Worker Roles

Reference the assembly that contains the Azure role entry point integration. The recommended way of doing this is by adding a NuGet package reference to the `NServiceBus.Hosting.Azure` package to the project.

NOTE: When self-hosting everything can be configured using code API and extension methods available in the NServiceBus Azure related packages. In such a case it's not required to reference the hosting package in the project. Self-hosting is described later in this article.

To integrate the NServiceBus generic host into the worker role entry point, create a new instance of `NServiceBusRoleEntrypoint` and call it's `Start` and `Stop` methods in the appropriate `RoleEntryPoint` override.

snippet:HostingInWorkerRole

Next to starting the role entry point, define the endpoint behavior. The role has been named `AsA_Worker`. Specify the transport to use, using the `UseTransport<T>`, as well the persistence using the `UsePersistence<T>` configuration methods.

snippet:AzureServiceBusTransportWithAzureHost

This will integrate and configure the default infrastructure:

 * Configuration setting will be read from the `app.config` file, merged with the settings from the service configuration file.
 * Logs will be sent to the `Trace` infrastructure, which should have been configured with Azure diagnostic monitor trace listener by the Visual Studio tooling.
 * Subscriptions are persisted in the chosen persistence store (in case of the AzureStorageQueue transport, AzureServiceBus has it's own subscription facility).
 * Saga's are enabled by default and persisted in the chosen persistence store.
 * Timeouts are enabled by default and persisted in the chosen persistence store.

For more information on configuration refer to the [configuration page](configuration.md). For more information on setting up logging, refer tot the [logging page](logging.md) 

## Cloud Services - Web Roles

Next to worker roles, cloud services also has a role type called 'Web Roles'. These are worker roles which have IIS configured properly, this means that they run a worker role process (the entry point is in `webrole.cs`) and an IIS process on the same codebase.

Usually NServiceBus is configured as a client in the IIS process. This needs to be approached in the same way as any other website, by means of self-hosting. When self-hosting everything can be configured using the configuration API and the extension methods found in the NServiceBus Azure related packages. No reference to the hosting package is required in that case.

The configuration API is used with the following extension methods to achieve the same behavior as the `AsA_worker` role:

snippet:HostingInWebRole

A short explanation of each:

 * `AzureConfigurationSource`: overrides any settings known to the NServiceBus Azure configuration section within the app.config file with settings from the service configuration file.
 *  Logs will be sent to the `Trace` infrastructure, which should have been configured with Azure diagnostic monitor trace listener by the visual studio tooling.
 * `UseTransport<AzureStorageQueueTransport>`: Sets [Azure storage queues](/nservicebus/azure-storage-queues/) as the [transport](/nservicebus/transports).
 * `UsePersistence<AzureStoragePersistence>`: Configures [Azure storage](/nservicebus/azure-storage-persistence/) for [persistence](/nservicebus/persistence).

For more information on configuration refer to the [Azure Cloud Services Host Endpoint Configuration](configuration.md) page. For more information on setting up logging, refer tot the [Logging in Azure Cloud Services](logging.md) page 
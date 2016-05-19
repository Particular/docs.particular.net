---
title: Configuration
summary: Configuring the endpoint when hosting in Azure Cloud Services
tags:
- Azure
- Cloud
- Configuration
---

## Configuring for Cloud Services hosting

Cloud Services is a hosting model provided by the Azure cloud, which is specifically designed for hosting large applications. For a detailed description of the cloud service configuration in Azure, see [What is the Cloud Service model and how do I package it?](https://azure.microsoft.com/en-us/documentation/articles/cloud-services-model-and-package/).

When configuring NServiceBus for hosting in Cloud Services, it needs to connect to a specific Azure storage account (for Azure Queues) or a Azure Service Bus namespace. For more information on the connection string formats, refer to [the windows azure connection string formats](http://www.connectionstrings.com/windows-azure/).

## Configuring an endpoint

When using the Azure Cloud Services hosting entrypoint to host an endpoint, it should be configured using and implementation of `IConfigureThisEndpoint`.

### Enabling the Transport

To enable a transport of choice, the `UseTransport<T>` should be called on the endpoint configuration and a connection string must be provided.

For example using Azure Service Bus Transport

snippet:AzureServiceBusTransportWithAzureHost

Or using the Azure Storage Queues Transport:

snippet:AzureStorageQueueTransportWithAzureHost

### Enabling the Persistence

The Azure Storage Persistence can be enabled by specifying the `UsePersistence<AzureStoragePersistence>` on the endpoint config as well.

snippet:PersistenceWithAzureHost

NOTE: In Version 4, when hosting in the Azure role entrypoint provided by `NServiceBus.Hosting.Azure`, these persistence strategies will be enabled by default.

## Configuration override convention

Because Azure cloud services has it's own configuration model, but NServiceBus is typically used with it's configuration in app.config, a convention based configuration source is provided. Where most of the configuration is in app.config, but any setting can be added 'by convention' to the service configuration file to override the original value in app.config. This makes it easy to develop locally (without the service runtime), but still make use of this feature in production.

NOTE: As most implementations for NServiceBus (like transports and persisters) are moving towards a code only configuration model, this feature will lose most of it's value over time.

The configuration source can be turned on like this:

snippet:AzureConfigurationSource

This convention based override model works for all configuration sections used by NServicebus, but for the sake of example, let's use the `AzureServiceBusQueueConfig` section which is available in Azure Service Bus transport version 6 and below:

Snippet:AzureServiceBusQueueConfigSection

This setting can then be set in the app.config file, by specifying this config section:

Snippet:AzureServiceBusQueueConfig

When hosting in a Azure cloud service this setting can be overridden in the service configuration (`.cscfg`) file.

First define the setting in the service definition file (`.csdef`).

Snippet:AzureServiceBusQueueConfigCsDef

Then specify the value for every cloud service deployment in the Cloud Services project.

Snippet:AzureServiceBusQueueConfigCsCfg

The name to be used for the property override is always structured like this: `TagName.PropertyName`. If tags were nested it's a repetition of the pattern `ParentTagName.ChildTagName.PropertyName`. It's currently impossible to override parent tags that would contain multiple of the same child tag, therefore it's impossible to override `MessageEndpointMappings` in this way.

The override order used in this example applies, lowest priority is the default value set on the config section, then the app.config value is applied, and then the service configuration value is applied.

### Applying configuration changes

Azure Cloud Services allows to change the configuration settings from within the Azure Portal. But, any changes made to them will not be applied automatically into the NServiceBus configuration system, because the original values have often been copied into settings fields or private member fields of downstream components.

If configuration changes should result in a reconfiguration of the endpoint, consider instructing the `RoleEnvironment` to restart the role instances by subscribing to the [RoleEnvironment.Changing event](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.serviceruntime.roleenvironment.changing.aspx) and setting `e.Cancel = true;`

If at least 2 role instances are running, then this will result in a configuration change without inflicting downtime on the overall system. Each instance may reboot individually in the process, but this is orchestrated across update and fault domains so that at any point in time an instance is operational.
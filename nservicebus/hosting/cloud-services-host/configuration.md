---
title: Configuration
summary: Configuring the endpoint when hosting in Azure Cloud Services
tags:
- Azure
reviewed: 2016-09-21
---

include: cloudserviceshost-deprecated-warning

## Configuring for Cloud Services hosting

Cloud Services is a hosting model provided by the Azure cloud, which is specifically designed for hosting large applications. For a detailed description of the cloud service configuration in Azure, see [What is the Cloud Service model packaging](https://docs.microsoft.com/en-us/azure/cloud-services/cloud-services-model-and-package).

When NServiceBus is hosted in Cloud Services, it needs to connect to a specific Azure storage account (for Azure Storage Queues) or an Azure Service Bus namespace. For more information on required connection string formats refer to [the windows azure connection string formats](https://www.connectionstrings.com/windows-azure/) article.


## Configuring an endpoint

When an endpoint is hosted in Azure Cloud Services, then it should be configured by implementing the  `IConfigureThisEndpoint` interface.


### Enabling the Transport

To enable a given transport, the `UseTransport<T>` should be called on the endpoint configuration and a connection string must be provided.

For example using Azure Service Bus Transport

snippet: AzureServiceBusTransportWithAzureHost

Or using the Azure Storage Queues Transport:

snippet: AzureStorageQueueTransportWithAzureHost


### Enabling the Persistence

The Azure Storage Persistence can be enabled by specifying the `UsePersistence<AzureStoragePersistence>` on the endpoint config as well.

snippet: PersistenceWithAzureHost

NOTE: In Version 4, when hosting in the Azure RoleEntryPoint provided by `NServiceBus.Hosting.Azure`, these persistence strategies will be enabled by default.


## Convention to override configuration

NServiceBus is typically configured using an `app.config` file, however Azure Cloud Services have their own configuration model. That makes settings management between various environments (e.g. local machine and production) complicated. In order to simplify the process, NServiceBus also supports a convention-based configuration. It allows for adding any NServiceBus setting to the service configuration file. The value specified in the service configuration file will override the value specified in the `app.config` file.

NOTE: NServiceBus is moving towards a code only configuration model, in NServiceBus Version 6 code configuration is the recommended model. When using code configuration model, the convention-based overrides are no longer necessary.

The configuration source can be turned on like this:

snippet: AzureConfigurationSource

The convention-based override model works for all configuration sections used by NServiceBus. For example, it's possible to override the `AzureServiceBusQueueConfig` section which is available in Azure Service Bus transport Version 6 and below:

snippet: AzureServiceBusQueueConfigSection

It is configured in the `app.config` file by specifying a dedicated config section:

snippet: AzureServiceBusQueueConfig

That setting can then be overridden in the service configuration file (`.cscfg`), when hosting in the Azure Cloud Service.

First define the setting in the service definition file (`.csdef`).

snippet: AzureServiceBusQueueConfigCsDef

Then specify the value for every cloud service deployment in the Cloud Services project.

snippet: AzureServiceBusQueueConfigCsCfg

Names used for property overrides always have the following structure:  `TagName.PropertyName`. Tags can be nested: `ParentTagName.ChildTagName.PropertyName`. It's currently not possible to override parent tags that contain multiple child tags with the same name, therefore `MessageEndpointMappings` can't be overridden using this approach.

The default value set in the config section has the lowest priority. It can be overridden by the value specified in the `app.config` file. The value provided in the service configuration file takes precedence over the value specified in the `app.config` file.


### Applying configuration changes

Azure Cloud Services allows to change the configuration settings from within the Azure Portal. However, the changes made in the Azure Portal are not automatically applied to the all NServiceBus components.

If configuration changes should result in a reconfiguration of the endpoint, consider instructing the `RoleEnvironment` to restart the role instances by subscribing to the [RoleEnvironment.Changing event](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.serviceruntime.roleenvironment.changing.aspx) and setting `e.Cancel = true;`

If at least 2 role instances are running, then this will result in a configuration change without inflicting downtime on the overall system. Each instance may reboot individually in the process, but this is orchestrated across update and fault domains so that at any point in time an instance is operational.

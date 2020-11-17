---
title: Configuration
summary: Configuring the endpoint when hosting in Azure cloud services
reviewed: 2020-02-27
---

include: cloudserviceshost-deprecated-warning

## Configuring for cloud services hosting

Cloud services is a hosting model provided by the Azure cloud, which is specifically designed for hosting large applications. For a detailed description of the cloud service configuration in Azure, see [What is the Cloud Service model packaging](https://docs.microsoft.com/en-us/azure/cloud-services/cloud-services-model-and-package).

When NServiceBus is hosted in cloud services, it needs to connect to a specific Azure storage account (for Azure Storage Queues) or an Azure Service Bus namespace. For more information on required connection string formats, refer to [the Windows Azure connection string formats](https://www.connectionstrings.com/windows-azure/) article.


## Configuring an endpoint

When an endpoint is hosted in a cloud service, it should be configured by implementing the `IConfigureThisEndpoint` interface.


### Enabling the transport

To enable a given transport, `UseTransport<T>` should be called on the endpoint configuration and a connection string must be provided.

For example, using the Azure Service Bus transport:

snippet: AzureServiceBusTransportWithAzureHost

or using the Azure Storage Queues transport:

snippet: AzureStorageQueueTransportWithAzureHost


### Enabling the persistence

The Azure Table persistence can be enabled by calling `UsePersistence<AzureTablePersistence>` on the endpoint config as well.

snippet: PersistenceWithAzureHost

NOTE: In Azure Table Persistence version 4, when hosting in the Azure RoleEntryPoint provided by `NServiceBus.Hosting.Azure`, these persistence strategies will be enabled by default.


## Convention to override configuration

NServiceBus is typically configured using an `app.config` file, however Azure Cloud Services have their own configuration model. That makes settings management between various environments (e.g. local machine and production) complicated. To simplify the process, NServiceBus supports a convention-based configuration which allows for adding any NServiceBus setting to the service configuration file. The value specified in the service configuration file will override the value specified in the `app.config` file.

NOTE: In NServiceBus version 6, configuration via code is the recommended model. With this model, convention-based overrides are no longer necessary.

The configuration source can be turned on like this:

snippet: AzureConfigurationSource

The convention-based override model works for all configuration sections used by NServiceBus. For example, it's possible to override the `AzureServiceBusQueueConfig` section which is available in Azure Service Bus Transport version 6 and below:

snippet: AzureServiceBusQueueConfigSection

It is configured in the `app.config` file by specifying a dedicated config section:

snippet: AzureServiceBusQueueConfig

That setting can then be overridden in the service configuration file (`.cscfg`) when hosting in a Azure cloud service.

First, define the setting in the service definition file (`.csdef`).

snippet: AzureServiceBusQueueConfigCsDef

Then, specify the value for every cloud service deployment in the cloud service project.

snippet: AzureServiceBusQueueConfigCsCfg

Names used for property overrides always have the following structure:  `TagName.PropertyName`. Tags can be nested: `ParentTagName.ChildTagName.PropertyName`. It's currently not possible to override parent tags that contain multiple child tags with the same name, therefore `MessageEndpointMappings` can't be overridden using this approach.

The default value set in the config section has the lowest priority. It can be overridden by the value specified in the `app.config` file. The value provided in the service configuration file takes precedence over the value specified in the `app.config` file.


### Applying configuration changes

Azure cloud services allow changing the configuration settings from within the Azure portal. However, the changes made in the Azure portal are not automatically applied to the all NServiceBus components.

If configuration changes should result in a reconfiguration of the endpoint, consider instructing the `RoleEnvironment` to restart the role instances by subscribing to the [RoleEnvironment.Changing event](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.serviceruntime.roleenvironment.changing.aspx) and setting `e.Cancel = true;`

If at least two role instances are running, this will result in a configuration change without inflicting downtime on the overall system. Each instance may reboot individually in the process, but this is orchestrated across update and fault domains so that at any point in time an instance is operational.
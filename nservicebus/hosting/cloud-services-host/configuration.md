---
title: Configuration
summary: Configuring the endpoint when hosting in Azure cloud services
component: CloudServicesHost
reviewed: 2020-02-27
---

include: cloudserviceshost-deprecated-warning

## Configuring for cloud services hosting

Cloud services is a hosting model provided by the Azure cloud, which is specifically designed for hosting large applications. For a detailed description of the cloud service configuration in Azure, see [What is the Cloud Service model packaging](https://docs.microsoft.com/en-us/azure/cloud-services/cloud-services-model-and-package).

When NServiceBus is hosted in cloud services, it needs to connect to a specific Azure storage account (for Azure Storage Queues) or an Azure Service Bus namespace. For more information on required connection string formats, refer to [the Windows Azure connection string formats](https://www.connectionstrings.com/windows-azure/) article.


## Configuring an endpoint

When an endpoint is hosted in a cloud service, it should be configured by implementing the `IConfigureThisEndpoint` interface.

snippet: ConfigureEndpointWithAzureHost


## Convention to override configuration

NServiceBus is typically configured using an `app.config` file, however Azure Cloud Services have their own configuration model. That makes settings management between various environments (e.g. local machine and production) complicated. To simplify the process, NServiceBus supports a convention-based configuration which allows for adding any NServiceBus setting to the service configuration file. The value specified in the service configuration file will override the value specified in the `app.config` file.

NOTE: In NServiceBus version 6, configuration via code is the recommended model. With this model, convention-based overrides are no longer necessary.

The configuration source can be turned on like this:

snippet: AzureConfigurationSource

Names used for property overrides always have the following structure:  `TagName.PropertyName`. Tags can be nested: `ParentTagName.ChildTagName.PropertyName`. It's currently not possible to override parent tags that contain multiple child tags with the same name, therefore `MessageEndpointMappings` can't be overridden using this approach.

The default value set in the config section has the lowest priority. It can be overridden by the value specified in the `app.config` file. The value provided in the service configuration file takes precedence over the value specified in the `app.config` file.


### Applying configuration changes

Azure cloud services allow changing the configuration settings from within the Azure portal. However, the changes made in the Azure portal are not automatically applied to the all NServiceBus components.

If configuration changes should result in a reconfiguration of the endpoint, consider instructing the `RoleEnvironment` to restart the role instances by subscribing to the [RoleEnvironment.Changing event](https://msdn.microsoft.com/en-us/library/microsoft.windowsazure.serviceruntime.roleenvironment.changing.aspx) and setting `e.Cancel = true;`

If at least two role instances are running, this will result in a configuration change without inflicting downtime on the overall system. Each instance may reboot individually in the process, but this is orchestrated across update and fault domains so that at any point in time an instance is operational.
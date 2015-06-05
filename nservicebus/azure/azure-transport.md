---
title: Azure Transports
summary: Azure transports enable Azure Storage Queues and Azure Service Bus as underlying NServiceBus transports, in cloud-hosting and hybrid scenarios.
tags:
- Cloud
- Azure
- Transports
redirects:
 - nservicebus/windows-azure-transport
---

The Azure transports for NServiceBus enable the use of Azure Queues and Azure Service Bus as the underlying transports used by NServiceBus. It can be used in multiple scenarios:

 * Cloud hosting scenario: An NServiceBus endpoint is hosted as a cloud service and communicates with another endpoint located in another cloud service through the use of one of the Azure transports for NServiceBus.

 * Hybrid scenario: An NServiceBus endpoint is hosted on-premise and uses one of the Azure transports for NServiceBus to communicate with another NServiceBus endpoint hosted on a Cloud Service and/or on-premise.

As part of the Azure support for NServiceBus, you can choose between two options provided by the Azure platform:

-   Azure Storage Queues
-   Azure Service Bus

Each of these two options has separate features, capabilities, and usage characteristics. A detailed comparison and discussion of when to select which is beyond the scope of this document. To help decide which option best suits your application's needs, review the MSDN article "[Azure Queues and Azure Service Bus Queues - Compared and Contrasted](https://msdn.microsoft.com/library/azure/hh767287.aspx)".

## Samples

Samples for various scenario's are available on Github

-   [Azure Service Bus transport hosted in cloud services](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/VideoStore.AzureServiceBus.Cloud)
-   [Azure Service Bus transport hosted in an on-premises host](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/VideoStore.AzureServiceBus.OnPremises)
-   [Azure Queues transport hosted in cloud services](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/VideoStore.AzureStorageQueues.Cloud)
-   [Azure Queues transport hosted in cloud services with multiple endpoints hosted on the same role instances](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/VideoStore.AzureStorageQueues.Cloud.DynamicHost)

To run these samples, update the configuration connection strings and queue names settings in the relevant configuration files (as described below).

The Azure transport for NServiceBus are available from nuget. You can download the latest (and previous) release from the
[nuget website](https://www.nuget.org/profiles/nservicebus/).

## Prerequisites

The Azure transport for NServiceBus and its samples require the following:

-   [NServiceBus V4.0 or later](http://particular.net/downloads)
    -   Note that Azure transport for NServiceBus is supported by NServiceBus V3 and later. It is, however, recommended that you use it with NServiceBus V4, and the samples require NServiceBus V4.

-   [Microsoft Azure SDK version 2.0](http://azure.microsoft.com/en-us/downloads/)

## Configuring for cloud service hosting

For a detailed description of the cloud service configuration in Azure, see "[Set Up a Cloud Service for Azure](https://msdn.microsoft.com/library/azure/hh124108.aspx#bk_Config)".

To configure NServiceBus to connect to a specific Azure storage account (for Azure Queues) or a Azure Service Bus namespace, you must set the [appropriate connection string for each option](http://www.connectionstrings.com/windows-azure/).

### Azure Storage Queues

In the Azure Service Configuration file (ServiceConfiguration.cscfg), add the following sections:

```
<?xml version="1.0" encoding="utf-8"?>
<ServiceConfiguration serviceName="AzureService" 
xmlns="http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration" 
schemaVersion="2013-03.2.0">
  <Role name="{role name}">
    <Instances count="1" />
    <ConfigurationSettings>
      <Setting name="AzureQueueConfig.ConnectionString" 
      value="DefaultEndpointsProtocol=https;AccountName={your account name here};
      AccountKey={your account key here}" />
      <Setting name="AzureQueueConfig.QueueName" value="{your queue name here}" />
...
    </ConfigurationSettings>
  </Role>
</ServiceConfiguration>
```

The "AzureQueueConfig.ConnectionString" for Azure Queues follows this format:

    DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;QueueEndpoint=customEndpoint;

Alternatively, you can use the Azure development environment emulator by using this connection string:

    UseDevelopmentStorage=True;

In your NServiceBus solution, specify the Endpoint Configuration to use AzureStorageQueue transport:

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
	{
	    public void Customize(BusConfiguration builder)
	    {
	        builder.UseTransport<AzureStorageQueueTransport>();
	    }
	}

### Azure Service Bus 

In the Azure Service Configuration file
(ServiceConfiguration.cscfg), add the following sections:

```
<?xml version="1.0" encoding="utf-8"?>
<ServiceConfiguration serviceName="AzureService" 
xmlns="http://schemas.microsoft.com/ServiceHosting/2008/10/ServiceConfiguration" 
schemaVersion="2013-03.2.0">
  <Role name="{role name}">
    <Instances count="1" />
    <ConfigurationSettings>
      <Setting name="AzureServiceBusQueueConfig.ConnectionString" 
      value="Endpoint=sb://{your namespace here}.servicebus.windows.net/;
      SharedAccessKeyName=RootManageSharedAccessKey;
      SharedAccessKey={your shared access key here}" />
...
    </ConfigurationSettings>
  </Role>
</ServiceConfiguration>
```


The "AzureQueueConfig.ConnectionString" for Azure Service Bus namespace connection string can be retrieved from the Azure portal using an authorized account.

In your NServiceBus solution, specify the endpoint configuration to use AzureServiceBus transport:

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
	{
	    public void Customize(BusConfiguration builder)
	    {
	        builder.UseTransport<AzureServiceBusTransport>();
	    }
	}

## Configuring for on-premise hosting

In the configuration file for the application (web.config or app.config), add an "NServiceBus\\Transport" element as follows:

```
<configuration>  
...
 <connectionStrings>
    <add name="NServiceBus/Transport" connectionString="DefaultEndpointsProtocol=https;
               AccountName={your account name here};AccountKey={your account key here}"/>
  </connectionStrings>
... 
</configuration>
```

The connection string value should be set according to the selected Azure options selected (either a Azure Queues connection string or a Azure Service Bus namspace).

NOTE: Setting the connection string in the application configuration files (web.config or app.config) is overridden by any settings placed in the service configuration file (ServiceConfiguration.cscfg) if one exists. This allows a cloud hosting scenario to override an on-premise deployment scenario, with minimal changes to the configuration, while allowing easy updates to the deployment configuration through the service configuration files only, with no need to update the applications configuration files.


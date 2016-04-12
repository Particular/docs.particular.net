---
title: Azure Cloudservices Host Endpoint Configuration
summary: Configuring your endpoint when hosting in azure cloud services
tags:
- Azure
- Cloud
---

## Enabling the Transport

When using one of the NServiceBus provided hosting processes, the `UseTransport<T>` should be called on the endpoint configuration. For example using Azure Service Bus transport

snippet:AzureServiceBusTransportWithAzureHost


Example using azure storage queues transport:

snippet:AzureStorageQueueTransportWithAzureHost


## Enabling the Persistence

The Azure storage persistence can be enabled by specifying the `UsePersistence<AzureStoragePersistence>` on the endpoint config.

snippet:PersistenceWithAzureHost

NOTE: In Version 4, when hosting in the Azure role entrypoint provided by `NServiceBus.Hosting.Azure`, these persistence strategies will be enabled by default.

## Configuring for cloud service hosting

For a detailed description of the cloud service configuration in Azure, see [What is the Cloud Service model and how do I package it?](https://azure.microsoft.com/en-us/documentation/articles/cloud-services-model-and-package/).

To configure NServiceBus to connect to a specific Azure storage account (for Azure Queues) or a Azure Service Bus namespace, one must set the [appropriate connection string for each option](http://www.connectionstrings.com/windows-azure/).


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
      value="DefaultEndpointsProtocol=https;AccountName={the account name here};
      AccountKey={the account key here}" />
      <Setting name="AzureQueueConfig.QueueName" value="{the queue name here}" />
...
    </ConfigurationSettings>
  </Role>
</ServiceConfiguration>
```

The "AzureQueueConfig.ConnectionString" for Azure Queues follows this format:

    DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;QueueEndpoint=customEndpoint;

Alternatively, one can use the Azure development environment emulator by using this connection string:

    UseDevelopmentStorage=True;

In the NServiceBus solution, specify the Endpoint Configuration to use AzureStorageQueue transport:

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


The "AzureServiceBusQueueConfig.ConnectionString" for Azure Service Bus namespace connection string can be retrieved from the Azure portal using an authorized account.

In the NServiceBus solution, specify the endpoint configuration to use AzureServiceBus transport:

    public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
	{
	    public void Customize(BusConfiguration builder)
	    {
	        builder.UseTransport<AzureServiceBusTransport>();
	    }
	}
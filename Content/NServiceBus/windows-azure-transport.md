---
title: Windows Azure Transport
summary: Windows Azure transport enables Windows Azure Queues and Windows Azure Service Bus as underlying NServiceBus transports, in cloud-hosting and hybrid scenarios.
originalUrl: http://www.particular.net/articles/windows-azure-transport
tags:
- Cloud
- Windows Azure
- NServiceBus
- Transport
- Sample
---

The Windows Azure transport for NServiceBus enables the use of Windows Azure Queues and Windows Azure Service Bus as the underlying transports used by NServiceBus. It can be used in multiple scenarios:

 * ### Cloud hosting scenario
 An NServiceBus endpoint is hosted as a cloud service and communicates, through the use of the Windows Azure transport for NServiceBus, with another endpoint located in another cloud service

 * ### Hybrid scenario
 An NServicebus is hosted on-premise and uses Windows Azure transport for NServiceBus to communicate with another NServiceBus endpoint hosted on a Cloud Service and/or on-premise

As part of Windows Azure transport for NServiceBus, you can choose between two options provided by the Windows Azure platform:

-   Windows Azure Queues
-   Windows Azure Service Bus

Each of these two options has separate features, capabilities and usage characteristics. A detailed comparison and discussion of when to select which is beyond the scope of this document. To help decide which option best suits your application's needs, review the MSDN article "[Windows Azure Queues and Windows Azure Service Bus Queues - Compared and Contrasted](http://msdn.microsoft.com/en-us/library/windowsazure/hh767287.aspx)".

Download Samples
----------------

-   [Windows Azure Queues Publish / Subscribe](http://d214svlj19ktn4.cloudfront.net/NServiceBus/Samples/Azure/AzurePubSub.zip)
-   [Windows Azure Service Bus Publish / Subscribe](http://d214svlj19ktn4.cloudfront.net/NServiceBus/Samples/Azure/AzureServiceBusPubSub.zip)

In order to run these samples, you will need to update the configuration connection strings and queue names settings in the relevant configuration file (as described below).

The Windows Azure transport for NServiceBus ships with NServiceBus. You can download the latest (and previous) release of NServiceBus from the
[downloads page](/downloads).

Prerequisites
-------------

The Windows Azure transport for NServiceBus and its samples require the following:

-   [NServiceBus V4.0 or later](/downloads)
    -   Note that Windows Azure transport for NServiceBus is supported by NServiceBus V3 and later. It is, however, recommended that you use it with NServiceBus V4, and the samples require NServiceBus V4.

-   [Microsoft Windows Azure SDK version
    2.0](http://www.windowsazure.com/en-us/downloads/)

Configuring for Cloud Service Hosting
-------------------------------------

For a detailed description of the cloud service configuration in Windows Azure, see "[Set Up a Cloud Service for Windows Azure](http://msdn.microsoft.com/en-us/library/windowsazure/hh124108.aspx#bk_Config)".

To configure NServiceBus to connect to a specific Windows Azure storage account (for Windows Azure Queues) or a Windows Azure Service Bus namespace, you must set the [appropriate connection string for each option](http://www.connectionstrings.com/windows-azure).

### Windows Azure Queues

In the Windows Azure Service Configuration file (ServiceConfiguration.cscfg), add the following sections:

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


The "AzureQueueConfig.ConnectionString" for Windows Azure Queues follows this format:

    DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;QueueEndpoint=customEndpoint;

Alternatively, you can use the Windows Azure development environment emulator by using this connection string:

    UseDevelopmentStorage=True;

In your NServiceBus solution, specify the Endpoint Configuration to use AzureStorageQueue transport:

```
public class EndpointConfiguration : IConfigureThisEndpoint, AsA_Worker
      , UsingTransport<AzureStorageQueue>
    {
        public EndpointConfiguration()
        {
            ...
        }
    }
```

### Windows Azure ServiceBus 

In the Windows Azure Service Configuration file
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


The "AzureQueueConfig.ConnectionString" for Windows Azure Service Bus namespace connection string can be retrieved from the Windows Azure portal using an authorized account.

In your NServiceBus solution, specify the endpoint configuration to use AzureServiceBus transport:

```
public class EndpointConfiguration : IConfigureThisEndpoint, AsA_Worker, UsingTransport
{
    public EndpointConfiguration()
    {
        ...
    }
}
```

Configuring for On-Premise Hosting
----------------------------------

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

The connection string value should be set according to the selected Windows Azure options selected (either a Windows Azure Queues connection string or a Windows Azure Service Bus namspace).

**NOTE**: Setting the connection string in the application configuration files (web.config or app.config) is overridden by any settings placed in the service configuration file (ServiceConfiguration.cscfg) if one exists. This allows a cloud hosting scenario to override an on-premise deployment scenario, with minimal changes to the configuration, while allowing easy updates to the deployment configuration through the service configuration files only, with no need to update the applications configuration files.


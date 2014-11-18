---
title: Using Azure Storage Queues as transport in NServiceBus
summary: NServiceBus can use windows azure storage queues to take advantage of their peek-lock mechanism in environments where one cannot rely on the DTC
tags: 
- Windows Azure
- Cloud
---

In some environments, like very large cloud networks or hybrid network scenarios, it's not possible or recommended to rely heavily on the DTC, and thus on msmq, to ensure transactional behavior and retry in case of failures. A good alternative to using msmq in this case might be to use Windows Azure Storage queues instead.

Windows Azure Queue storage is a service, hosted on the Windows Azure platform, for storing large numbers of messages that can be accessed from anywhere in the world via authenticated calls using HTTP or HTTPS.
 
- The main advantage of this service is that it offers a highly reliable and very cheap queuing service ($0.1 per million messages). A single queue message can be up to 64 KB in size, and a queue can contain millions of messages, up to the total capacity limit of a storage account (200 TB). Furthermore it is capable to emulate local transactions using it's queue peek lock mechanism.
- The main disadvantage of this service is latency introduced by it's remoteness and the fact that it only supports HTTP based communication.

## How to enable the transport

First you need to reference the assembly that contains the azure storage queue transport definition. The recommended way of doing this is by adding a nuget package reference to the  `NServiceBus.Azure.Transports.WindowsAzureStorageQueues` package to your project.

Once you've done that you can use the fluent `Configure` API to setup NServiceBus, all you need to do is specify `.UseTransport<AzureStorageQueueTransport>()` to override the default transport.

	Configure.With()
         ...
         .UseTransport<AzureStorageQueueTransport>()
         ...
         .CreateBus()

Alternatively, when using one of the NServiceBus provided hosting processes, you should supply the `UsingTransport<AzureStorageQueueTransport>` on the endpoint configuration. In the windows azure role entrypoint host, for example, it would look like this.

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
	{
	    public void Customize(BusConfiguration builder)
	    {
	        builder.UseTransport<AzureStorageQueueTransport>();
	    }
	}

## Setting the connection string

The default way of setting the connection string is using the .net provided connectionstrings configuration section in app.config or web.config, with the name `NServicebus\Transport`

```
<connectionStrings>
   <add name="NServiceBus/Transport" connectionString="UseDevelopmentStorage=true" />
</connectionStrings> 
```

Note that multiple connection string formats apply when working with windows azure storage services. When you're running against the emulated environment the format is `UseDevelopmentStorage=true`, but when running against a cloud hosted storage account the format is `DefaultEndpointsProtocol=https;AccountName=myAccount;AccountKey=myKey;` 

## Detailed configuration

If you need more fine grained control on how the azure storage queue transport behaves, you can override the default settings by adding a configuration section called `AzureQueueConfig` to your web.config or app.config. For example:

	<configSections>
	    <section name="AzureQueueConfig" type="NServiceBus.Config.AzureQueueConfig, NServiceBus.Azure.Transports.WindowsAzureStorageQueues" />   
	</configSections>
	<AzureQueueConfig ConnectionString="UseDevelopmentStorage=true" />

Using this configuration setting you can change the following values.

- `ConnectionString`: Overrides the default "NServiceBus/Transport" value and defaults to "UseDevelopmentStorage=true" if not set. Best to set this value when specifying the configuration setting to prevent surprises.
- `QueueName`: Allows you to specify the queue name, if not set NServiceBus will create a queue name for you based on the endpoint naming convention.
- `PeekInterval`: Represents the amount of time that the transport waits before polling the queue in milliseconds, defaults to 50 ms.
- `MaximumWaitTimeWhenIdle`: The transport will back of linearly when no messages can be found on the queue to save some money on the transaction operations, but it will never wait longer than the value specified here, also in milliseconds and defaults to 1000 (1 second)
- `PurgeOnStartup`: Instructs the transport to remove any existing messages from the queue on startup, defaults to false.
- `MessageInvisibleTime`: The peek lock system, supported by azure storage queues relies on a period of time that a message becomes locked/invisible after being read. If the processing unit fails to delete the message by the specified time it will reappear on the queue so that another process can retry. This value is defined in milliseconds and defaults to 30000 (30 seconds).
- `BatchSize`: The number of messages that the transport tries to pull at once from the storage queue. Defaults to 10. Depending on the load you expect, I would vary this value between 1 and 1000 (which is the limit)
- `QueuePerInstance`: Tells nservicebus to create a separate queue for every instance of the endpoint hosted. This is useful in pub sub scenario's where you want a message to arrive at every instance of the endpoint, f.e. in a webfarm that has local cache on every machine. Defaults to false.

## Sample

Want to see this transport in action? Checkout the [Video storage sample.](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/VideoStore.AzureStorageQueues.Cloud)


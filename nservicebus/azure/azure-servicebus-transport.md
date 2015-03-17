---
title: Azure Service Bus as Transport
summary: NServiceBus can use Windows Azure Service Bus to take advantage of its peek-lock mechanism in environments where one cannot rely on the DTC.
tags: 
- Windows Azure
- Cloud
redirects:
 - nservicebus/using-azure-servicebus-as-transport-in-nservicebus
---

In some environments such as very large cloud networks or hybrid network scenarios, it is not possible or recommended to rely heavily on the DTC, and thus on MSMQ, to ensure transactional behavior and retry in case of failures. A good alternative to MSMQ in this case is to use Windows Azure Service Bus instead.

Windows Azure Service Bus is messaging infrastructure that sits between applications, allowing them to exchange messages in a loosely coupled way for improved scale and resiliency. Service Bus Queues offer simple first in, first out guaranteed message delivery and supports a range of standard protocols (REST, AMQP, WS*) and APIs to put/pull messages on/off a queue. Service Bus Topics deliver messages to multiple subscriptions and easily fan out message delivery at scale to downstream systems.
 
- The main advantage of this service is that it offers a highly reliable and (relatively) low latency remote messaging infrastructure. A single queue message can be up to 256 KB in size, and a queue can contain many messages, up to 5GB in total. Furthermore it is capable of emulating local transactions using its queue peek-lock mechanism and has many built-in features that you (and NServiceBus) can take advantage of, such as message deduplication and deferred messages.
- The main disadvantage of this service is its dependency on TCP (if you want low latency), which may require you to open some outbound ports on your firewall. Additionally, the price may be steep, depending on your scenario ($1 per million messages).

## Enabling the Transport

First, reference the assembly that contains the Azure Service Bus transport definition. The recommended method is to add a NuGet package reference to the  `NServiceBus.Azure.Transports.WindowsAzureServiceBus` package to your project.

Then, use the Fluent `Configure` API to set up NServiceBus, by specifying `.UseTransport<AzureServiceBusTransport>()` to override the default transport:

	Configure.With()
         ...
         .UseTransport<AzureServiceBusTransport>()
         ...
         .CreateBus()

Alternatively, when using one of the NServiceBus provided hosting processes, you should call the `UseTransport<AzureServiceBusTransport>` on the endpoint configuration. In the Windows Azure role entrypoint host, for example, it looks like this:

	public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker
	{
	   public void Customize(BusConfiguration builder)
	   {
		 builder.UseTransport<AzureServiceBusTransport>();
	   }
	}

## Setting the Connection String

The default way to set the connection string is using the .net provided connectionStrings configuration section in app.config or web.config, with the name `NServicebus\Transport`:

	<connectionStrings>
	   <add name="NServiceBus/Transport" connectionString="Endpoint=sb://{yournamespace}.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue={yourkey}" />
	</connectionStrings> 

## Configuring in Detail

If you need fine grained control on how the Azure Service Bus transport behaves, you can override the default settings by adding a configuration section called `AzureServiceBusQueueConfig` to your web.config or app.config files. For example:

	<configSections>
	    <section name="AzureServiceBusQueueConfig" type="NServiceBus.Config.AzureServiceBusQueueConfig, NServiceBus.Azure.Transports.WindowsAzureServiceBus" />   
	</configSections>
	<AzureServiceBusQueueConfig ConnectionString="Endpoint=sb://{yournamespace}.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue={yourkey}" />

Using this configuration setting you can change the following values. NOTE: Most of these values are applied when a queue or topic is created and cannot be changed afterwards).

- `ConnectionString`: Overrides the default "NServiceBus/Transport" connectionstring value.
- `QueueName`: Allows you to specify the queue name. If not set, NServiceBus creates a queue name for you based on the endpoint naming convention.
- `ConnectivityMode`: Allows you to switch between HTTP and TCP based communication. Defaults to TCP. Very useful when behind a firewall.
- `ServerWaitTime`: The transport uses longpolling to communicate with the Azure Service Bus entities. This value specifies the amount of time, in seconds, the longpoll can take. Defaults to 300 seconds. 
- `BackoffTimeInSeconds`: The transport will back off linearly when no messages can be found on the queue to save some money on the transaction operations. This value specifies how fast it will back off. Defaults to 10 seconds.
- `LockDuration`: The peek-lock system supported by Azure Service Bus relies on a period of time that a message becomes locked/invisible after being read. If the processing unit fails to delete the message by the specified time, it will reappear on the queue so that another process can retry. This value is defined in milliseconds and defaults to 30000 (30 seconds). 
- `EnableBatchedOperations`: Specifies whether batching is enabled. Defaults to true.
- `BatchSize`: The number of messages that the transport tries to pull at once from the queue. Defaults to 1000. 
- `MaxDeliveryCount`: Specifies the number of times a message can be delivered before being put on the dead letter queue. Defaults to 6 (so the NServiceBus first and second level retry mechanism gets preference).
- `MaxSizeInMegabytes`: Specifies the size in MB. Defaults to 1024 (1GB). Allowed values are 1024, 2048, 3072, 4096 5120.
- `RequiresDuplicateDetection`: Specifies whether exactly once delivery is enabled. Defaults to false, meaning that the same message can arrive multiple times.
- `DuplicateDetectionHistoryTimeWindow`:  Specifies the amount of time in milliseconds that the queue should perform duplicate detection. Defaults to 60000 ms (1 minute).
- `RequiresSession`: Specifies whether sessions are required. Defaults to false (NServiceBus makes no use of this feature).
- `DefaultMessageTimeToLive`: Specifies the time that a message can stay in the queue without being delivered. Defaults to int64.MaxValue, which is roughly 10.000 days.
- `EnableDeadLetteringOnMessageExpiration`: Specifies whether messages should be moved to a dead letter queue upon expiration. Defaults to false (TTL is so large it wouldn't matter anyway). This assumes there have been no attempts to deliver. Errors on delivery will still put the message on the dead letter queue.
- `EnableDeadLetteringOnFilterEvaluationExceptions`: Specifies whether messages should be moved to a dead letter queue upon filter evaluation exceptions. Defaults to false.
- `QueuePerInstance`: Tells NServiceBus to create a separate queue for every instance of the endpoint hosted. This is useful in pub/sub scenarios where you want a message to arrive for every instance of the endpoint, i.e., in a webfarm that has a local cache on every machine. Defaults to false.


### Connection string sample

```xml
<connectionStrings>
   <!-- Azure ServiceBus -->
   <add name="NServiceBus/Transport"
        connectionString="Endpoint=sb://[namespace].servicebus.windows.net;
                                      SharedSecretIssuer=owner;
                                      SharedSecretValue=someSecret"/>
</connectionStrings>
```

## Sample

To see this transport in action, see the [Video store sample.](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/VideoStore.AzureServiceBus.Cloud)


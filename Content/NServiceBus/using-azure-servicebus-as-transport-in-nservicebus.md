
---
title: Using Azure Servicebus as transport in NServiceBus
summary: NServiceBus can use windows azure servicebus to take advantage of it's peek-lock mechanism in environments where one cannot rely on the DTC
tags: []
---

In some environments, like very large cloud networks or hybrid network scenarios, it's not possible or recommended to rely heavily on the DTC, and thus on msmq, to ensure transactional behavior and retry in case of failures. A good alternative to using msmq in this case might be to use Windows Azure Servicebus instead.

Windows Azure Service Bus is messaging infrastructure that sits between applications allowing them to exchange messages in a loosely coupled way for improved scale and resiliency. Service Bus Queues offer simple first in, first out guaranteed message delivery and supports a range of standard protocols (REST, AMQP, WS*) and API’s to put/pull messages on/off a queue. Service Bus Topics deliver messages to multiple subscriptions and easily fan out message delivery at scale to downstream systems.
 
- The main advantage of this service is that it offers a highly reliable and (relatively) low latency remote messaging infrastructure. A single queue message can be up to 256 KB in size, and a queue can contain a lot of messages, up to a 5GB in total. Furthermore it is capable to emulate local transactions using it's queue peek lock mechanism and has a lot of built in features that  you (and nservicebus) can take advantage of, like message deduplication, defered messages and lot's more.
- The main disadvantage of this service is it's dependency on TCP (if you want low latency) which may require you to open some outbound ports on your firewall, further more the price may be steep depending on your scenario ($1 per million messages).

How to enable the transport
---------------------------

First you need to reference the assembly that contains the azure servicebus transport definition. The recommended way of doing this is by adding a nuget package reference to the  `NServiceBus.Azure.Transports.WindowsAzureServiceBus` package to your project.

Once you've done that you can use the fluent `Configure` API to setup NServiceBus, all you need to do is specify `.UseTransport<AzureServiceBus>()` to override the default transport.


```C#
Configure.With()
         ...
         .UseTransport<AzureServiceBus>()
         ...
         .CreateBus()
```

Alternatively, when using one of the NServiceBus provided hosting processes, you should supply the `UsingTransport<AzureServiceBus>` on the endpoint configuration. In the windows azure role entrypoint host, for example, it would look like this.

```C#
public class EndpointConfig : IConfigureThisEndpoint, AsA_Worker, UsingTransport<AzureServiceBus>
{
}
```

Setting the connection string
----------------------------

The default way of setting the connection string is using the .net provided connectionstrings configuration section in app.config or web.config, with the name `NServicebus\Transport`

```XML
<connectionStrings>
   <add name="NServiceBus/Transport" connectionString="Endpoint=sb://{yournamespace}.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue={yourkey}" />
</connectionStrings> 
```


Detailed configuration
----------------------

If you need more fine grained control on how the azure servicebus transport behaves, you can override the default settings by adding a configuration section called `AzureServiceBusQueueConfig` to your web.config or app.config. For example:

```XML
<configSections>
    <section name="AzureServiceBusQueueConfig" type="NServiceBus.Config.AzureServiceBusQueueConfig, NServiceBus.Azure.Transports.WindowsAzureServiceBus" />   
</configSections>
<AzureServiceBusQueueConfig ConnectionString="Endpoint=sb://{yournamespace}.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue={yourkey}" />
```

Using this configuration setting you can change the following values. (It's important to know that most of these values are applied when a queue or topic is created and cannot be changed afterwards).

- `ConnectionString`: Overrides the default "NServiceBus/Transport" connectionstring value.
- `QueueName`: Allows you to specify the queue name, if not set NServiceBus will create a queue name for you based on the endpoint naming convention.
- `ConnectivityMode`: Allows you to switch between Http and Tcp based communication, defaults to Tcp. Very usefull when behind a firewall.
- `ServerWaitTime`: Our transport uses longpolling to communicate with the azure servicebus entities. This value specifies the amount of time, in seconds, the longpoll can take. Defaults to 300 seconds. 
- `BackoffTimeInSeconds`: The transport will back of linearly when no messages can be found on the queue to save some money on the transaction operations, this value specifies how fast it will back off. Default is 10 seconds.
- `LockDuration`: The peek lock system, supported by azure servicebus relies on a period of time that a message becomes locked/invisible after being read. If the processing unit fails to delete the message by the specified time it will reappear on the queue so that another process can retry. This value is defined in milliseconds and defaults to 30000 (30 seconds). 
- `EnableBatchedOperations`: specifies whether batching is enabled, defaults to true
- `BatchSize`: The number of messages that the transport tries to pull at once from the queue. Defaults to 1000. 
- `MaxDeliveryCount`: Specifies the number of times a message can be delivered before being put on the dead letter queue, defaults to 6 (So that nservicebus first and second level retry mechanism gets preference)
- `MaxSizeInMegabytes`: Specifies the size, defaults to 1024 (1GB), allowed values are 1, 2, 3, 4, 5 GB
- `RequiresDuplicateDetection`: Specifies whether exactly once delivery is enabled, defaults to false
- `DuplicateDetectionHistoryTimeWindow`:  Specifies the amount of time in milliseconds that the queue should perform duplicate detection, defaults to 60000 ms (1 minute)
- `RequiresSession`: Specifies whether sessions are required, defaults to false (NServicebus makes no use of this feature)
- `DefaultMessageTimeToLive`: Specifies the time that a message can stay on the queue, without being delivered, defaults to int64.MaxValue which is roughly 10.000 days
- `EnableDeadLetteringOnMessageExpiration`: Specifies whether messages should be moved to a dead letter queue upon expiration, defaults to false (TTL is so large it wouldn't matter anyway). This assumes no attempt of delivery have been taken, errors on delivery will still put the message on the dead letter queue.
- `EnableDeadLetteringOnFilterEvaluationExceptions`: Specifies whether messages should be moved to a dead letter queue upon filter evaluation exceptions, defaults to false.
- `QueuePerInstance`: Tells nservicebus to create a separate queue for every instance of the endpoint hosted. This is useful in pub sub scenario's where you want a message to arrive at every instance of the endpoint, f.e. in a webfarm that has local cache on every machine. Defaults to false.

Sample
------

Want to see this transport in action? Checkout the [Video store sample.](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/VideoStore.AzureServiceBus.Cloud)


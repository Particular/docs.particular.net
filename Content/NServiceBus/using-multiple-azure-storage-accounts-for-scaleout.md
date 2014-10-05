---
title: Using Multiple Azure Storage Accounts for Scale out
summary: NServiceBus can use multiple Azure storage accounts for scale out
tags: 
- Windows Azure
- Cloud
- Azure Storage
---

NServiceBus based system running on Azure Storage Queues transport using a single storage account is a subject of potential throttling once maximum number of messages is written into a storage account. In order to overcome this limitation, multiple storage account can be utilized. To understand better scale out options on storage account level, you need to understand Azure storage account scalability and performance.

## Azure Storage Scalability and Performance

All of the messages in a queue are accessed via a single queue partition. A single queue is targeted to be able to process up to 2,000 messages per second. Scalability targets for storage accounts will very based on region with up to up to 20,000 messages per second (throughput achieved using an object size of 1 KB). This is subject to change and should be  periodically verified using [MSDN provided information](http://msdn.microsoft.com/library/azure/dn249410.aspx).

When the number of messages exceeds this quota, storage service responds with an HTTP 503 Server Busy message. This message indicates that the platform is throttling the queue. If a single storage account is unable to handle an application`s request rate, an application could also leverage several different storage accounts, using a storage account per endpoint. This ensures application scalability without choking a single storage account. This also allows discrete control over queue processing based on the sensitivity and priority of the messages that are handled by different endpoints. High priority endpoints could have more workers dedicated to them than low priority endpoints.

## Scaling Out

A typical implementation uses a single storage account to send and receive messages. All endpoints are configured to receive and send messages using the same storage account. 

![Single storage account](../images/NServiceBus/azure01.png)

When number of instances with endpoints are increased, all endpoints continue reading and writing to the same storage account. Once limit of 2,000 message/sec per queue or 20,000 message/sec per storage account is reached, Azure will throttled message throughput.

![Single storage account with scaled out endpoints](../images/NServiceBus/azure01.png)

While an NServiceBus endpoint can only read from a single Azure storage account, it can send messages to multiple storage accounts. Configured this by specifying connection string on message mapping. 

Example: Endpoint 1 sends messages to Endpoint 2. Endpoint 1 will define message mapping with connection string associated with Endpoint 2 Azure storage account. Same idea applies to Endpoint 1 sending messages to Endpoint 2.

Message Mapping for Endpoint 1

```xml
<MessageEndpointMappings>
	<add Messages="Contracts" Namespace="Contracts.Commands.ForEndpoint2" 
		 Endpoint="Endpoint2@connection_string_for_endpoint_2" />
</MessageEndpointMappings>
```

Message Mapping for Endpoint 2

```xml
<MessageEndpointMappings>
	<add Messages="Contracts" Namespace="Contracts.Commands.ForEndpoint1" 
		 Endpoint="Endpoint1@connection_string_for_endpoint_1" />
</MessageEndpointMappings>
```

Each endpoint is using its own Azure storage account and by that increases messages throughput.


![Scale out with multiple storage accounts](../images/NServiceBus/azure03.png)

## Sample

Want to see this in action? Checkout the [Video storage sample.](https://github.com/Particular/NServiceBus.Azure.Samples/tree/master/VideoStore.AzureStorageQueues.Cloud)

**NOTE:** You will need to use real Azure storage accounts since Azure storage emulator can only supports a single fixed account named `devstoreaccount1`.


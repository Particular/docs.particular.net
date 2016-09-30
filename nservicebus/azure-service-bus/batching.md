---
title: Batching
summary: Batching with Azure Service Bus and how it works
component: ASB
tags:
- Cloud
- Azure
- Transport
reviewed: 2016-04-26
---


## Batching store access

In order to increase message throughput, the Azure Service Bus batches operations prior to writing to its internal store. This involves delaying write operations by 20 ms. Read operations are not affected.

Batching store operations should be disabled for low throughput scenarios requiring low latency.

Batching store operations is enabled by default. In order to disable it, set `EnableBatching(false)` on [queues](/nservicebus/azure-service-bus/configuration/full.md#Queues), [topics](/nservicebus/azure-service-bus/configuration/full.md#Topics), and [subscriptions](/nservicebus/azure-service-bus/configuration/full.md#Subscriptions).


## Client-side batching

Client-side batching allows messages, being sent to a queue or topic, to be delayed for a short period of time. Additional messages sent to the same queue or topic within the specified time period will be grouped and transmitted together in a single batch. This setting affects the Azure Service Bus Message Senders and can be configured using [`BatchFlushInterval`](/nservicebus/azure-service-bus/configuration/full.md#messaging-factories). Client-side batching is enabled by default.

Batching store operations should be disabled for low throughput scenarios requiring low latency. To do so, set the `BatchFlushInterval` to `TimeSpan.Zero`. For high-throughput scenarios, increase the `BatchFlushInterval`.

Batched store access is only available for asynchronous Send and Complete operations. Synchronous operations are immediately sent to the Service Bus service. Batching does not occur for Peek or Receive operations, nor does batching occur across clients.


## Batching messages sent from a handler

Azure Service Bus optimizes multiple message sends from a handler by batching send operations into a single operation. For example, when a handler is sending multiple messages, Azure Service Bus will batch those and send in as few operations as possible. Size of a batch cannot exceed the maximum size on a `BrokeredMessage`. The maximum message size is configured using [`MaximumMessageSizeInKilobytes`](/nservicebus/azure-service-bus/configuration/full.md#message-senders) setting of the Message Senders.

NOTE: `BrokeredMessage` size is different between [tiers](https://azure.microsoft.com/en-us/documentation/articles/service-bus-premium-messaging/) of Azure Service Bus.

When batching messages sent from a handler, the underlying implementation of batching serializes messages. A serialized batch is usually bigger than the original messages combined. To ensure successful batch sending operation, batch should not exceed the `BrokeredMessage` size. To cater for the overhead caused by serialization, the final batch size is estimated using `MessageSizePaddingPercentage` setting. By default, it's set to 5%. It can be configured using [`MessageSizePaddingPercentage`](/nservicebus/azure-service-bus/configuration/full.md#message-senders) configuration of the Message Senders.

By default, message batches exceeding the maximum allowed size by Azure Service Bus, will throw a `MessageTooLargeException`. The default behavior can by changed with [`OversizedBrokeredMessageHandler<T>(T)`](/nservicebus/azure-service-bus/configuration/full.md#message-senders) configuration of the Message Senders.


### Padding and estimated batch size calculation

The following are taken into consideration for batch size calculation:

 * Raw body size in bytes.
 * Custom headers size (keys and values) in bytes.
 * Estimated standard properties size in bytes. For string properties, assumed size is 256 bytes.
 * Additional `MessageSizePaddingPercentage` to account for serialization and internals added by Azure Service Bus library upon sending.

The default value for `MessageSizePaddingPercentage` is 5%. The custom percentage might be required when dealing with consistent size pattern messages. Tables below demonstrate 5% padding and its affect on the message size estimate for various payload sizes.

`BrokeredMessageBodyType` set to `SupportedBrokeredMessageBodyTypes.ByteArray`.
`MessageSizePaddingPercentage` set to 5%

| Single message payload size   | Size reported by the broker  | Estimated (padded) size | Increase |
|---|---:|---:|:---:|
|0K  | 1,426  | 3,088 | 117% |
|1K   | 2,859 | 4,538 | 59% |
| 10K  | 15,147 | 17,440 | 16% |
| 100K  | 138,029 | 146,464 | 7% |
| 170K  | 233,550 | 246,815 | 6% |
| 180K  | 247,201 | 261,149 | 6% |


`BrokeredMessageBodyType` set to `SupportedBrokeredMessageBodyTypes.Stream`.
`MessageSizePaddingPercentage` set to 5%

| Single message payload size   | Size reported by the broker  | Estimated (padded) size | Increase |
|---|---:|---:|:---:|
|0K  | 1,401  | 3,101 | 122% |
|1K   | 2,769 | 4,538 | 64% |
| 10K  | 15,057 | 17,440 | 16% |
| 100K  | 137,937 | 146,464 | 7% |
| 170K  | 233,509 | 246,815 | 6% |
| 180K  | 247,161 | 261,149  | 6% |

WARNING: When `TransportTransactionMode.SendsAtomicWithReceive` is used, Azure Service Bus limits number of outgoing operations from a handler context to 100. If number of outgoing operations exceeds the limit, `TransactionContainsTooManyMessages` exception will be thrown and incoming message will be eventually moved to the error queue. To send more than 100 outgoing operations, lower transport transaction mode. 

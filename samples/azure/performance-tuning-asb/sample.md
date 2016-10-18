---
title: ASB Performance Tuning
summary: Samples that show the impact of different settings and api usage patterns on the performance of the ASB transport.
component: ASB
reviewed: 2016-10-18
related:
 - nservicebus/azure-service-bus
 - samples/azure/azure-service-bus
---

## Prerequisites

include: asb-connectionstring 

Environment variables named `AzureServiceBus.ConnectionString1` and `AzureServiceBus.ConnectionString2` with a different connection string to an Azure Service Bus namespace each.

include: asb-transport

## Code walk-through

This sample has three endpoints

* `Sender`
* `Receiver`
* `SenderReceiver`

## Sender

`Sender` sends `SomeMessage` to `Receiver`.

snippet: SomeMessage

The sender sends a large amount of messages to the receiver and times how long it takes to do so. The resulting time will be divided by the number of messages sent in order to compute the average throughput. 

There are two variations of `Sender` to show different ways of sending messages, outside the scope of a receive operation, nd illustrate what impact they have on performance.

## Receiver

`Receiver` receives `SomeMessage` and computes every second how many it was able to receive in that timeframe

Again there are different permutations of settings that have impact on the receive throughput.

## SenderReceiver

`SenderReceiver` is a variation on `Receiver` that does not only receive messages, but it also sends messages to a destination queue.

## Running the sample

Run without debugging (CTRL + F5) for optimal results.

 * Run the sender standalone to test send performance, but also to fill up the receive queue.
 * Run the receiver standalone to test receive performance, if the queue empties, just run the sender again.
 * Run the sender receiver standalone
 
## Variations
 
### Slow (Sequential) Sender

In the slow sender:
 * Only 1 factory is defined, this means that only 1 tcp connection is maintained with the broker.
 * Client side batching of the ASB SDK is turned off, so the SDK will send message by message.

Snippet: slow-send-config

In this scenario each send is awaited individually, which prevents batching (even if batching is enabled on the client side).

Snippet: slow-send  

### Fast (Concurrent) Sender

In the fast sender scenario:
 * Multiple connections are established by configuring multiple messaging factories.
 * Each factory is matched with a sender object (keep these equal so that there is 1 sender per factory)
 * Client side batching is on, allowing the ASB SDK to send many messages at once

Snippet: fast-send-config

Individual sends are not awaited, instead the entire batch is awaited. Code execution will continue when all batches have been sent.

Snippet: fast-send

### Slow (Sequential) Atomic Receiver

In the slow atomic receiver scenario:
 * Only 1 factory is defined, this means that only 1 tcp connection is maintained with the broker.
 * Prefetching is turned off, meaning that the receiver will fetch message by message
 * No concurrent operations allowed, messages are processed one message at a time.

Snippet: slow-atomic-receiver-config

### Fast (Concurrent) Atomic Receiver

In the fast atomic receiver scenario:
 * Multiple connections are established with the broker.
 * Each connection is matched with exactly 1 receive client.
 * Ensures that at least 16 connections are established when using partitioned queues, as each partitioned queue consists of 16 partitions. Depending on available bandwidth configure more until the network saturates.
 * TransactionMode AtomicSendsWithReceive will execute complete operations in a serializable transaction, which means one by one. Therefore it is not beneficial to allow high per receiver concurrency. A value of 2 allows one receive to start while another is allowed to complete. The more time execution takes the higher this value can be, but never use really high values here.
 * PrefetchCount is set to a relatively low number, since receiver concurrency and throughput are also low.

Snippet: fast-atomic-receiver-config

### Fast (Concurrent) Atomic Sender Receiver

The fast atomic sender receiver scenario is similar to the fast atomic receiver scenario, except:
 * It established twice the amount of connections, so that the receive clients and send clients can leverage different connections.

Snippet: fast-atomic-send-receive-config

### Slow (Sequential) Non Atomic Receiver

In the slow none atomic receiver sample, everything is configured wrong:
 * Only 1 factory is defined, this means that only 1 tcp connection is maintained with the broker.
 * Prefetching is turned off, meaning that the receiver will fetch message by message
 * No concurrent operations allowed, messages are processed one by one.

Snippet: slow-non-atomic-receiver-config

### Fast (Concurrent) Non Atomic Receiver

In the fast non atomic receiver scenario:
 * Multiple connections are established with the broker.
 * Each connection is matched with exactly 1 receive client.
 * Ensures that at least 16 connections are established when using partitioned queues, as each partitioned queue consists of 16 partitions. Depending on available bandwidth configure more until the network saturates.
 * TransactionMode ReceiveOnly can batch message completions, allowing receive operations to work independently. Therefore it is very beneficial to allow high per receiver concurrency. For example 128 operations per receiver if the operation can handle this kind of concurrency. (What this means really depends on the code inside the handler, a database connection may not like 128 concurrent operations, while an in memory operation on a concurrentdictionary may)
 * PrefetchCount is set to a high number as well, usually 1x or 2x the allowed per receiver concurrency.

Snippet: fast-non-atomic-receiver-config

### Fast (Concurrent) Non Atomic Sender Receiver

The fast non atomic sender receiver scenario is similar to the fast non atomic receiver scenario, except:
 * It established twice the amount of connections, so that the receive clients and send clients can leverage different connections.

Snippet: fast-non-atomic-sender-receiver-config
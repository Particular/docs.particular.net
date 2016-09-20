---
title: ASB Performance Tuning
summary: Samples that show the impact of different settings and api usage patterns on the performance of the ASB transport.
component: ASB
reviewed: 2016-09-06
related:
 - nservicebus/azure-service-bus
 - samples/azure/azure-service-bus
---


## Prerequisites

include: asb-connectionstring environment variables named `AzureServiceBus.ConnectionString1` and `AzureServiceBus.ConnectionString2` with a different connection string to an Azure Service Bus namespace each.

include: asb-transport


## Code walk-through

This sample has three endpoints

* `Sender`
* `Receiver`
* `InfiniteReceiver`



## Sender

`Sender` sends `SomeMessage` to `Receiver`.

snippet: SomeMessage

The sender sends a large amount of messages to the receiver and times how long it takes to do so. The resulting time will be divided by the number of messages sent in order to compute the throughput. 

There are multiple variations of `Sender` to show different ways of sending messages, outside the scope of a receive operation, and what the impact is on throughput.

## Receiver

`Receiver` receives `SomeMessage` and computes every second how many it was able to receive in that timeframe

Again there are different permutations of settings that have impact on the combines send & receive throughput.

## InfiniteReceiver

`InfiniteReceiver` is a variation on `Receiver` that does not only receive messages, but it also sends them back to itself to go in an infinite send and receive loop.

## Running the sample

 * Run the sender standalone to test send performance, but also to fill up the receive queue.
 * Run the receiver standalone to test receive performance, if the queue empties, just run the sender again.
 * Run the infinite receiver standalone

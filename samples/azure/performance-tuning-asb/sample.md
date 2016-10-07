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

There are two variations of `Sender` to show different ways of sending messages, outside the scope of a receive operation, and what the impact is on throughput.

## Receiver

`Receiver` receives `SomeMessage` and computes every second how many it was able to receive in that timeframe

Again there are different permutations of settings that have impact on the combines send & receive throughput.

## SenderReceiver

`SenderReceiver` is a variation on `Receiver` that does not only receive messages, but it also sends messages to a destination queue.

## Running the sample

 * Run the sender standalone to test send performance, but also to fill up the receive queue.
 * Run the receiver standalone to test receive performance, if the queue empties, just run the sender again.
 * Run the sender receiver standalone
 
## Variations
 
### Slow Sender

Snippet: slow-send-config
Snippet: slow-send  

### Fast Sender

Snippet: fast-send-config
Snippet: fast-send

### Slow Atomic Receiver

Snippet: slow-atomic-receiver-config

### Fast Atomic Receiver

Snippet: fast-atomic-receiver-config

### Fast Atomic Sender Receiver

Snippet: fast-atomic-send-receive-config

### Slow Non Atomic Receiver

Snippet: slow-non-atomic-receiver-config

### Fast Non Atomic Receiver

Snippet: fast-non-atomic-receiver-config

### Fast Non Atomic Sender Receiver

Snippet: fast-non-atomic-sender-receiver-config
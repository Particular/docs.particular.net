---
title: Performance Tuning
tags:
 - Azure
 - Cloud
reviewed: 2016-09-21
---

partial:body

## Tuning send performance outside the scope of a handler

Only settings at the ASB SDK level impact this scenario, more specifically

  * `MessagingFactories().BatchFlushInterval()`: Controls the time that the ASB SDK batches client side requests before sending them out. A value of TimeSpan.Zero turns this feature off. Values between 100 and 500 milliseconds appear to work best.
  * `MessagingFactories().NumberOfMessagingFactoriesPerNamespace()`: Each factory manages a tcp connection to a front end node in the broker, this connection has throughput limits. Opening more factories significantly improves send performance. Values between 2 and 8 seem to work best for sending.
  * `NumberOfClientsPerEntity()`: Keep this value equal to the number of factories, this ensures there is 1 send client per factory.

The way the send api is invoked matters a lot as well. Using `await` on each `Send()` operation will make the ASB SDK wait until the send operation completes and therefore makes client side batching useless. It's better to maintain a list of send tasks and await them all together instead.

Slow:

Snippet: asb-slow-send

Fast:

Snippet: asb-fast-send

## Tuning receive performance

Tuning receive performance depends on quite a few things that vary per scenario, but the following settings bring improvements for all scenarios.

  * `MessagingFactories().NumberOfMessagingFactoriesPerNamespace()`: Each factory manages a tcp connection to a front end node in the broker, this connection has throughput limits. Opening more factories significantly improves receive performance. Values between 16 and 40 seem to work best for receiving.
  * `NumberOfClientsPerEntity()`: Keep this value equal to the number of factories, this ensures there is 1 receive client per factory.

Other settings that impact performance a lot, but their values differ quite a bit per scenario, are:

  * `LimitMessageProcessingConcurrencyTo()`: This is the global concurrency limit across all receive entities. Ensure this value matches the per receiver desired concurrency for the scenario, multiplied by the number of receivers.
  * `MessageReceivers().PrefetchCount()`: Value is per receiver, in general the following rule applies: The lower the throughput rate of an individual receiver, the lower this value should be.
  * `Transactions()`: The most significant performance impact comes from the transactional guarantees needed, `SendsAtomicWithReceive` will offer less performance then `ReceiveOnly`.

### Sends Atomic With Receive

In order to guarantee that sends occur as an atomic operation with the receive confirmation, the ASB SDK requires that both operations are enclosed in a `Serializable` transaction. This implies that:

  * All atomic operations are executed in a 'Serial' fashion, aka 1 by 1, therefore it does not make sense to allow many concurrent operations per client, nor does it make much sense to prefetch a lot of messages.
	  - Concurrency: 2 or 4 per client seems to work best
	  - PrefetchCount: 20 or less seem to be good values
  * Setting up the transactional connection takes up quite some time, this causes the first operational minutes to perform very 'bumpy', but after a while the throughput becomes consistent.
  * All send operations that occur in the scope of a single receive (inside a handler), must fit into the transaction. Therefore a maximum of 100 messages can be sent this way. The good news is that doing so has little additional implications on performance tuning as 1 send operation or 100 is actually the same thing except for the size of the operation.

### Receive only

This mode has a lot less constraints and will work better with high numbers for concurrency and prefetch count, which should result in a pretty high throughput (likely over 1000/s even for remote connections)

  * Concurrency: Values in the range of 128 per receive client seems to work well.
  * PrefetchCount: Set this to 1x or 2x the per receiver concurrency, so 128 or 256.

But, sending messages out at the same time does influence this mode more. If the average handler sends out only a single message then the above values still hold true. However if the handler sends out more messages, let's say 10 or 100 messages per receive, the concurrency and prefetchcount should be dialed down to allow more bandwidth to the senders.

  * Concurrency: Values in the range of 16 per receive client seem to work well in this scenario.
  * PrefetchCount: Set this to 1x or 2x the per receiver concurrency, so 16 or 32.

## Performance Tuning Samples

Check out the [performance tuning samples](/samples/azure/performance-tuning-asb/) to experiment with each of the settings in each scenario mentioned.





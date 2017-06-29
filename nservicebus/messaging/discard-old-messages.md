---
title: Discarding Old Messages
summary: Automatically discard messages if they have not been processed within a given period of time.
reviewed: 2017-06-29
component: Core
related:
 - nservicebus/operations/auditing
redirects:
 - nservicebus/how-do-i-discard-old-messages
---

A message sent through the Particular Service Platform may have Time-To-Be-Received (TTBR) set, according to the users’ decision. TTBR indicates to the platform that a delayed message will be discarded, instead of processed, if not handled within a specified period. A discarded message might no longer have any business value, and discarding it frees up system resources.

Setting TimeToBeReceived might be beneficial in environments with high volumes of messages where there is little business value in processing a delayed message since it will already be replaced by a newer, more relevant version.

Only messages that have not been handled will have the TTBR set. A failed message moved to the error queue or a successfully processed message is considered handled, as well as its possible audit message. By removing TTBR from handled messages, it is ensured that no message will be lost after it has been processed. The TTBR from the original message can be inspected by looking at the `NServiceBus.TimeToBeReceived` [header](/nservicebus/messaging/headers.md).

If a message cannot be received by the target process in the given time frame, including all time in queues and in transit, it may be desirable to discard it by using TimeToBeReceived.


To discard a message when a specific time interval has elapsed:


## Using an Attribute

snippet: DiscardingOldMessagesWithAnAttribute


## Using a custom convention

snippet: DiscardingOldMessagesWithCode


## Clock synchronization issues

When sending a message with a certain TimeToBeReceived value, it could happen that the receiver drops the message if clocks are too much out of sync. For example, if TimeToBeReceived is 1 minute and the receiver's clock is 1 minute ahead compared to the sender the message could potentially never be delivered thus processed.

Because clocks usually are at most a few minutes out of sync this issue only applies to relatively small TimeToBeReceived values.

For this reason, it is wise to add the maximum amount of allowed clock offset, called clock drift, to the TTBR value. For example, when using a TimeToBeReceived value of 90 seconds, one should allow for 300 seconds of maximum clock drift, so the TTBR value becomes 90 + 300 = 390 seconds.


## Discarding old messages at startup

In certain situations, it may be required that old messages are not processed after restarting the endpoint. Usually, this functionality is used in development and test environments, but may also be appropriate when messages contain information that gets outdated, e.g. change notifications, readings from sensors in IoT apps, etc.

NOTE: Discarding old messages at startup should be used only in exceptional scenarios when such functionality is desired. As a general rule, it's not recommended to be used in production as it may lead to subtle message loss situations.

To discard old messages at startup:

snippet: PurgeMessagesAtStartup


## Caveats

TimeToBeReceived relies on underlying functionality in the transport infrastructure to discard expired messages. This feature's runtime behavior is highly affected by the actual implementation in the different transports.


### MSMQ transport

MSMQ continuously checks the TimeToBeReceived of all queued messages. As soon as the message has expired, it is removed from the queue, and disk space reclaimed. 

MSMQ however only allows a single TimeToBeReceived for all messages in a transaction. If multiple messages enlist in a single transaction, then TimeToBeReceived from the first message will be used for all messages, leading to a potential message loss scenario. To prevent message loss, `TimeToBeReceived` is not supported for endpoints with [transaction mode](/transports/transactions.md) `TransportTransactionMode.AtomicSendsWithReceive` or `Transaction Scope (Distributed Transaction)`.

partial: msmq


### RabbitMQ transport

RabbitMQ continuously checks the TimeToBeReceived, but only for the first message in each queue. Expired messages are not removed from the queue, and their disk space is not reclaimed until they reach the front of the queue. Using TimeToBeReceived as a disk-saving measure on RabbitMQ is not ideal for queues with long-lived messages, like audit and forward unless it is ensured that all messages use the same TimeToBeReceived. The TimeToBeReceived of other messages in front of a message in a queue will affect when the message is removed.


### Azure transports

The Azure transports only evaluate the TimeToBeReceived for a message when the message is received from the queue. Expired messages are not removed from the queue and their disk space will not be reclaimed until they reach the front of the queue and a consumer tries to read them. Using TimeToBeReceived as a disk saving measure on the Azure transports is not a great choice for queues with long-lived messages like audit and forward.


### SQL transport

partial: sql

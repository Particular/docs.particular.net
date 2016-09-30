---
title: Discarding Old Messages
summary: Automatically discard messages if they have not been processed within a given period of time.
component: Core
tags:
 - Message Expiration
 - TimeToBeReceived
related:
 - nservicebus/operations/auditing
redirects:
 - nservicebus/how-do-i-discard-old-messages
---

A message sent through the Particular Service Platform may have TTBR (TimeToBeReceived) set, according to the users’ decision. TTBR indicates to the platform that a delayed message will be discarded, instead of processed, if not handled within a specified period of time. A discarded message might no longer have any business value, and discarding it frees up storage, CPU and memory resources.

This is important mainly in environments with high volumes of messages where there is little business value in processing a delayed message since it will already be replaced by a newer, more relevant version.

Only messages that have not been handled will have the TTBR set. A failed message moved to the error queue or a successfully handled message, including its possible audit message, is considered handled. By removing TTBR from handled messages it is ensured that no message will be lost after it has been processed. The TTBR from the original message can always be inspected by looking at the `NServiceBus.TimeToBeReceived` [header](/nservicebus/messaging/headers.md).

If a message cannot be received by the target process in the given time frame, including all time in queues and in transit, it may be desirable to discard it by using TimeToBeReceived.


To discard a message when a specific time interval has elapsed:


## Using an Attribute

snippet:DiscardingOldMessagesWithAnAttribute


## Using a custom convention

snippet:DiscardingOldMessagesWithCode


## Caveats

TimeToBeReceived relies on underlying functionality in the transport infrastructure to discard expired messages. This feature's usefulness is highly affected by the actual implementation in the different transports.


### MSMQ transport

MSMQ continuously checks the TimeToBeReceived attribute of all queued messages. As soon as the message has expired, it is removed from the queue, and disk space reclaimed. MSMQ will however only allow a single TimeToBeReceived for all messages in a transaction. It will silently copy the TimeToBeReceived from the first message enlisted to all other messages in the transaction, leading to a potential message loss scenario if the later message require a higher TTBR value.

If TTBR is required than enable the [outbox](https://docs.particular.net/nservicebus/outbox/). Messages are first stored in the outbox and after that dispatched individually which makes TTBR work.

WARNING: [immediate message dispatch](send-a-message.md#dispatching-a-message-immediately) can be used with the caveat that this message will be send even though an error occurs after sending.

### RabbitMQ transport

RabbitMQ also continuously checks the TimeToBeReceived attribute, but only for the first message in each queue. Expired messages are not removed from the queue, and their disk space is not reclaimed until they reach the front of the queue. Using TimeToBeReceived as a disk saving measure on RabbitMQ is not ideal for queues with long-lived messages, like audit and forward, unless it is ensured that all messages have the same TimeToBeReceived set. The TimeToBeReceived of other messages in front of a message in a queue will affect when the message is actually removed.


### Azure transports

The Azure transports only evaluate the TimeToBeReceived attribute for a message when the message is received from the queue. Expired messages are not removed from the queue and their disk space will not be reclaimed until they reach the front of the queue and a consumer tries to read them. Using TimeToBeReceived as a disk saving measure on the Azure transports is not a great choice for queues with long-lived messages like audit and forward.


### SQL transport

The SQL transport runs a periodic task that removes expired messages from the queue. The task is first executed when the endpoint starts and is subsequently scheduled to execute 5 minutes after the previous run of the task has been completed. The transport also evaluates the TimeToBeReceived attribute for a message when the message is received from the queue. This helps to promptly discard expired messages that arrive at the front of the queue between scheduled executions of the purging task.

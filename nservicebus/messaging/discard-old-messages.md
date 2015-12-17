---
title: Discarding Old Messages
summary: Automatically discard messages if they have not been processed within a given period of time.
tags:
- Message Expiration
- TimeToBeReceived
redirects:
- nservicebus/how-do-i-discard-old-messages
---

A message sent through the Particular Service Platform may have TTBR (TimeToBeReceived) set, according to the users’ decision. TTBR indicates to the platform that a delayed message will be discarded, instead of processed, if not handled within a specified period of time. A discarded message might no longer have any business value, and discarding it frees up storage, CPU and memory resources.

This is important mainly in environments with high volumes of messages where there is little business value in processing a delayed message since it will already be replaced by a newer, more relevant version.

Only messages that have not been handled will have the TTBR set. A failed message moved to the error queue or a successfully handled message, including its possible audit message, is considered handled. By removing TTBR from handled messages we assure that no message will be lost after it has been processed. The TTBR from the original message can always be inspected by looking at the `NServiceBus.TimeToBeReceived` [header](/nservicebus/messaging/headers.md).

If a message cannot be received by the target process in the given time frame, including all time in queues and in transit, you may want to discard it by using TimeToBeReceived.


To discard a message when a specific time interval has elapsed:


## Using an Attribute

snippet:DiscardingOldMessagesWithAnAttribute


## Using your own convention

snippet:DiscardingOldMessagesWithCode

## Caveats
TimeToBeReceived relies on underlying functionality in the transport infrastructure to discard expired messages. This feature's usefulness is highly affected by the actual implementation in the different transports.

### MSMQ transport
MSMQ continuously checks the TimeToBeReceived attribute of all queued messages. As soon as the message has expired, it is removed from the queue, and disk space reclaimed. MSMQ will however only allow a single TimeToBeReceived for all messages in a transaction. It will silently copy the TimeToBeReceived from the first message enlisted to all other messages in the transaction, leading to a potential message loss scenario. This issue, unfortunately, make it impossible for us to support setting TimeToBeReceived for messages sent from a transactional MSMQ endpoint.

### RabbitMQ transport
RabbitMQ also continuously checks the TimeToBeReceived attribute, but only for the first message in each queue. Expired messages are not removed from the queue, and their disk space is not reclaimed until they reach the front of the queue. Using TimeToBeReceived as a disk saving measure on RabbitMQ is not a great choice for queues with long-lived messages like audit and forward unless you make sure all messages have the same TimeToBeReceived set. The TimeToBeReceived of other messages in front of a message in a queue will affect when the message is actually removed.

### Azure transports
The Azure transports only evaluate the TimeToBeReceived attribute for a message when the message is received from the queue. Expired messages are not removed from the queue and their disk space will not be reclaimed until they reach the front of the queue and a consumer tries to read them. Using TimeToBeReceived as a disk saving measure on the Azure transports is not a great choice for queues with long-lived messages like audit and forward.

### SQL transport
The SQL transport evaluates the TimeToBeReceived attribute for a message when the message is received from the queue. Expired messages are not removed from the queue and their disk space will not be reclaimed until they reach the front of the queue and a consumer tries to read them. Using TimeToBeReceived as a disk saving measure on the SQL transports is not a great choice for queues with long-lived messages like audit and forward.

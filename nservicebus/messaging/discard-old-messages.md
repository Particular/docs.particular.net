---
title: Discarding Old Messages
summary: Automatically discard messages if they have not been processed within a given period of time.
tags:
- Message Expiration
- TimeToBeReceived
redirects:
- nservicebus/how-do-i-discard-old-messages
---

A message sent through the Particular Service Platform may have TTBR (TimeToBeReceived) set, according to the users’ decision. TTBR indicates to the platform that a delayed message will be discarded, instead of processed, if not handled after a specified period of time. A discarded message might no longer have any business value, and discarding it frees up storage, CPU and memory resources. 

This is important mainly in environments with high volumes of messages where there is little business value in processing a delayed message since it will already be replaced by a newer, more relevant version.

Only messages that have not been handled will have the TTBR set. A failed message moved to the error queue or a successfully handled message, including its possible audit message, is considered handled. By removing TTBR from handled messages we  assure that no message will be lost after it has been processed. The TTBR from the original message can always be inspected by looking at the `NServiceBus.TimeToBeReceived` [header](/nservicebus/messaging/message-headers.md).

If a message cannot be received by the target process in the given time frame, including all time in queues and in transit, you may want to discard it by using TimeToBeReceived.


To discard a message when a specific time interval has elapsed:

## Using an Attribute

<!-- import DiscardingOldMessagesWithAnAttribute -->

## Using the fluent API

<!-- import DiscardingOldMessagesWithFluent -->


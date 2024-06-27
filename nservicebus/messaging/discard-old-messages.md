---
title: Discarding Old Messages (time-to-be-received or time-to-live)
summary: Automatically discard messages if they have not been processed within a given period of time.
reviewed: 2023-11-30
component: Core
related:
 - nservicebus/operations/auditing
redirects:
 - nservicebus/how-do-i-discard-old-messages
---

A message sent through the Particular Service Platform may have Time-To-Be-Received (TTBR) set, according to the users’ decision. TTBR indicates to the platform that a delayed message can be discarded, if not handled within a specified period. A discarded message might no longer have any business value, and discarding it frees up system resources.

> [!NOTE]
> Time-to-be-received (TTBR) is also know as Time-to-live (TTL)

Setting TTBR might be beneficial in environments with high volumes of messages where there is little business value in processing a delayed message since it will already be replaced by a newer, more relevant version.

TTBR applies only to messages that have not been handled. A failed message moved to the error queue or a successfully processed message is considered handled, as well as audit message when generated. Removing TTBR from handled messages ensures no messages are lost. The TTBR value from the original message can be inspected by looking at the `NServiceBus.TimeToBeReceived` [header](/nservicebus/messaging/headers.md).

If there is no value in a message being received by the target process after a given time period it may be desirable to indicate that it can be discarded by specifying TimeToBeReceived.

A TTBR interval for a message can be specified:

## Using an Attribute

snippet: DiscardingOldMessagesWithAnAttribute

## Using a custom convention

snippet: DiscardingOldMessagesWithCode

## Clock synchronization issues

When sending a message with a TimeToBeReceived value, it could happen that the receiver drops all messages due to clocks of the sender and the receiver being too much out of sync. For example, if TimeToBeReceived is 1 minute and the receiver's clock is 1 minute ahead compared to the sender’s the message becomes immediately stale - thus never processed.

In most deployments clocks are at most a few minutes out of sync. As a result, this issue applies mostly to small TimeToBeReceived values.

It is advised to add the maximum amount of allowed clock offset, called clock drift, to the TTBR value. For example, when using a TimeToBeReceived value of 90 seconds, one should allow for 300 seconds of maximum clock drift, so the TTBR value becomes 90 + 300 = 390 seconds.

## Discarding messages at startup

In certain situations, it may be required that messages in the incoming queue should not be processed after restarting the endpoint. This usually applies to development and test environments, but may also be appropriate for messages containing information that gets outdated or otherwise unneeded, e.g. change notifications, readings from sensors in IoT apps, etc.

> [!NOTE]
> It's not recommended to discard messages at startup in a production environment because it may lead to subtle message loss situations that can be hard to diagnose.

To discard all existing messages in the incoming queue at startup:

snippet: PurgeMessagesAtStartup

## Behavior

Time-to-be-received relies on the transport infrastructure to discard expired messages. As a result runtime behavior is highly affected by the implementation in the different transports.

Review the **Transport at a glance** section, specifically the **time-to-be-received (TTBR)** feature for each [transport](/transports/).

## Auditing

Auditing can also use TTBR but the [auditing TTBR behavior is version specific](/nservicebus/operations/auditing.md#configuring-auditing-time-to-be-received).

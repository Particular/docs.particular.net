---
title:  Fixing malformed messages
summary: Describes the concept of fixing and retrying malformed messages in ServicePulse
component: ServicePulse
reviewed: 2024-07-26
---

If a message cannot be successfully retried it is possible to fix the malformed message prior to retrying it. Both the headers and the body of a message can be edited. This capability can be accessed when looking at an individual message.

![Edit Malformed Messages](images/edit-message-details.png 'width=500')

> [!NOTE]
> This feature works in the following way:
> 1. A copy of the failed message with a new [message ID](/nservicebus/messaging/message-identity.md) is created.
> 2. The headers and body of the newly created copy can be edited.
> 3. The copied message is dispatched, and the original failed message is marked as resolved.

## Enabling the feature

> [!WARNING]
> Editing malformed messages is a potentially destructive operation, which if used improperly can have consequences that affect the correctness of the system. If a malformed message is edited improperly, it could still be processed successfully, without any way to undo the effects. The possible effects of editing a malformed message are system-specific and ServicePulse is unable to determine if a potential edit operation is safe for the system. Currently there is no way to limit failed message editing only to certain security groups or to specific message types.
>
> System designers may prefer to disallow message editing in order to guarantee the correctness of the system. As a result, the message editing feature is currently considered experimental and is **disabled by default**.
>
> Before enabling the feature, carefully evaluate possible use cases for editing malformed messages and consider alternative solutions for those use cases such as [ServiceControl retry redirects](/samples/servicecontrol/fix-messages/), especially for use cases that are common and recurring. For help with alternatives for specific situations, contact [Particular Support](https://particular.net/support) for assistance.

> [!NOTE]
> Editing malformed messages requires both ServiceControl 4.1.0+ and ServicePulse 1.21.0+.

The feature in ServicePulse is enabled via the configuration file for ServiceControl:

1. Find the ServiceControl installation directory using ServiceControl Management Utility.
2. Edit the ServiceControl configuration file `ServiceControl.exe.config` with elevated privileges.
3. Add the following entry in the `<appSettings>` section of the file:
    ```
    <add key="ServiceControl/AllowMessageEditing" value="true" />
    ```
4. Restart ServiceControl.

## Headers

Headers on all failed messages can be edited using this feature. There are three categories of headers, each with different editing restrictions.

![Editing Headers](images/edit-message-headers.png 'width=500')

* **Locked headers:** Headers that are critical to the operation of NServiceBus cannot be edited. These headers will have a lock image next to their name.

* **Sensitive headers:** Headers that, when their values change, may result in unexpected or unwanted behavior are categorized as "Sensitive". When these headers are modified, the :warning: icon indicates their sensitive nature to the user.

* **Custom headers:** Any headers that have not been been categorized as Locked or Sensitive will be fully editable. This will include headers created as part of a system customization. It is also possible to delete (and retrieve) these headers during the editing process. Once a message has been edited and retried, any deleted headers will be lost forever.

## Editing the message body

Message bodies can be edited before they are retried. This is only possible for message bodies that are serialized using unencrypted JSON or XML.

## Retrying edited messages

> [!WARNING]
> Retrying messages after editing the message headers can cause message processing failures and/or visualization issues in ServicePulse.

When retrying an edited message it is possible that the original failed message will have been resolved by another user, retried successfully by another user, or [expired as part of the automated processes](/servicecontrol/how-purge-expired-data.md). In those scenarios, the new message will not be dispatched and the retry of the edited message will fail.

As soon as a message has been dispatched for retrying the originally failing message will be marked as resolved. If the retry message subsequently fails it will appear as a new failed message in the user interface. That new failed message will be marked as having been edited and also have a link to the original message.

Retrying a message, regardless whether it is processed correctly or not, dispatches also a [`MessageEditedAndRetried` event](/servicecontrol/contracts.md#other-events). That event includes an ID of the original message.

The copy of the failed message receives a new [message ID](/nservicebus/messaging/message-identity.md). The copied message is related to the original failed message through the [`NServiceBus.CorrelationId`](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-correlationid) header that is set to the ID of the original message. The copy carries also the same [`NServiceBus.ConversationId`](/nservicebus/messaging/headers.md#messaging-interaction-headers-nservicebus-conversationid) as the original.

## Limitations and restrictions

A failed message must not have been resolved by a user, successfully retried, or [expired as part of the automated processes](/servicecontrol/how-purge-expired-data.md) for edit and retry to function.

The bodies of messages with encrypted properties cannot be edited.

The edited message will be assigned a new message ID before dispatching. The new message ID is automatically generated and is a GUID stored as a string in the edited message's headers.

Resolving a failed message doesn't publish any [ServiceControl events](/servicecontrol/contracts.md#other-events).

---
title:  Fixing malformed messages
summary: Describes the concept of fixing and retrying malformed messages in ServicePulse
component: ServicePulse
reviewed: 2019-07-04
---

If a message cannot be successfully retried it is possible to fix the malformed message prior to retrying it. Both the headers and the body of a message can be edited. This capability can be accessed when looking at an individual message. 

![Edit Malformed Messages](images/edit-message-details.png 'width=500')

## Enabling the feature

The message edit and retry feature is not enabled by default. To enable it, contact [Particular Support](https://particular.net/support) for assistance.

## Headers

Headers on all failed messages can be edited using this feature. There are three categories of headers, each with different editing restrictions.

![Editing Headers](images/edit-message-headers.png 'width=500')

* **Locked headers:** Headers that are critical to the operation of NServiceBus cannot be edited. These headers will have a lock image next to their name.

* **Sensitive headers:** Headers that, when their values change, may result in unexpected or unwanted behavior are categorized as "Sensitive". When editing these headers the user will be warned of their sensitive nature.

* **Custom headers:** Any headers that have not been been categorized as Locked or Sensitive will be fully editable. This will include headers created as part of a system customization. It is also possible to delete (and retrieve) these headers during the editing process. Once a message has been edited and retried, any deleted headers will be lost forever.

## Editing the message body

Message bodies can be edited before they are retried. This is only possible for message bodies that are serialized using unencrypted JSON or XML.

## Retrying edited messages

WARN: Retrying messages after editing the message headers can cause message processing failures and/or visualization issues in the ServicePulse and ServiceInsight.

When retrying an edited message it is possible that the original failed message will have been resolved by another user, retried successfully by another user, or [expired as part of the automated processes](/servicecontrol/how-purge-expired-data.md). In those scenarios, the retry of the edited message will fail.

As soon as a message has been dispatched for retrying the originally failing message will be marked as resolved. If the retry message subsequently fails it will appear as a new failed message in the user interface. That new failed message will be marked as having been edited and also have a link to the original message.


## Limitations and restrictions

A failed message must not have been resolved by a user, successfully retried, or [expired as part of the automated processes](/servicecontrol/how-purge-expired-data.md) for edit and retry to function.

The bodies of messages with encrypted properties cannot be edited.

The edited message will be assigned a new message id before dispatching. The new message id is automatically generated and is a GUID stored as a string in the edited message headers.

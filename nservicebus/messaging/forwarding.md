---
title: Message forwarding
summary: Describes how to set up message forwarding
component: Core
reviewed: 2020-11-02
related:
 - nservicebus/operations/auditing
---

Use this feature to forward successfully processed messages from an endpoint to a specific destination endpoint. Forwarding messages is particularly useful in complex upgrade scenarios when the old version and new version of a particular endpoint are running side-by-side.

## Message processing abortion

Forwarding should not be used to abort message processing. Messages cannot be forwarded to the `error` queue as the required metadata for failed messages would not be present in the forwarded message's metadata.  Attempting to use forwarding as a mechanism to abort processing can result in partial updates.

To abort processing throw an exception in the message handler. This ensures that no [ghost message](/nservicebus/concepts/glossary.md#ghost-message) are transmitted and any transactional state changes are rolled back.

To prevent unnecessary retries declare an [unrecoverable exception](/nservicebus/recoverability/#unrecoverable-exceptions) or create a [custom recoverability policy](/nservicebus/recoverability/custom-recoverability-policy.md).

## Auditing vs. Forwarding

[Auditing](/nservicebus/operations/auditing.md) and Forwarding are very similar. Both send a copy of the processed message to another queue. The main difference is the intended usage scenarios.

Auditing is used for collecting information on what is happening in the system. The audited message is enriched with additional [information regarding the processing of it](/nservicebus/operations/auditing.md#message-headers). Forwarding would send a copy of the processed message without the additional auditing information.

The forwarded messages do **not** contain the [audit message headers](/nservicebus/messaging/headers.md#audit-headers).

partial: config

## Forwarding a message from the handler

Individual messages can be forwarded directly from the handler:

snippet: ForwardingMessageFromHandler

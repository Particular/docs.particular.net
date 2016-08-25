---
title: Error notifications
summary: Subscribing to error notifications
reviewed: 2016-08-23
component: Core
versions: '[5.0,)'
redirects:
 - nservicebus/subscribing-to-push-based-error-notifications
 - nservicebus/errors/subscribing-to-push-based-error-notifications
 - nservicebus/errors/subscribing-to-error-notifications
related:
 - samples/logging/notifications
---

Error notifications are available for several events.

 * When a [Immediate Retry](/nservicebus/recoverability/#immediate-retries) occurs.
 * When a [Delayed Retry](/nservicebus/recoverability/#delayed-retries) occurs.
 * When a message fails all retries and is forwarded to the error queue.

The following example shows how to be notified every time a message is handled by [recoverability](/nservicebus/recoverability/). While this code writes to the console any other action could be taken, for example sending an email or writing to a monitoring system.

snippet: SubscribeToErrorsNotifications

include: notificationThread

The notification instance is also injected into the [container](/nservicebus/containers/).


partial: reactive


## Unsubscribing

Since notifications are global for the current endpoint it is also important to ensure no longer required subscriptions removed so as to not unnecessarily impact performance.

partial: unsubscribing
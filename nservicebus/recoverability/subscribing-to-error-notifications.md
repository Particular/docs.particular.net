---
title: Error notifications
summary: Subscribing to error notifications
reviewed: 2019-10-23
component: Core
versions: '[5.0,)'
redirects:
 - nservicebus/subscribing-to-push-based-error-notifications
 - nservicebus/errors/subscribing-to-push-based-error-notifications
 - nservicebus/errors/subscribing-to-error-notifications
related:
 - samples/logging/notifications
---

Error notifications are available for several events:

 * When an [immediate retry](/nservicebus/recoverability/#immediate-retries) occurs.
 * When a [delayed retry](/nservicebus/recoverability/#delayed-retries) occurs.
 * When a message fails all retries and is forwarded to the error queue.

The following example shows how to be notified every time a message is handled by [recoverability](/nservicebus/recoverability/). While this code writes to the console any other action could be taken, for example sending an email or writing to a monitoring system.

NOTE: Since Version 6 errors happening in satellites like the Gateway will not trigger error notification. See the [Gateway Version 1 to 2 upgrade guide](/nservicebus/upgrades/gateway-1to2.md) for more information.

snippet: SubscribeToErrorsNotifications

include: notificationThread

partial: reactive

partial: unsubscribing


## Message Body

include: error-notifications-message-body

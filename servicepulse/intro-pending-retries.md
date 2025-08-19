---
title: Pending Retries Message Management
summary: Describes how ServicePulse detects and monitors failed messages in the pending state, and allows retrying, or deleting them.
reviewed: 2025-02-20
---

The pending retries view shows failed messages that have been requested to be retried but have not completed yet. Pending retries messages can be found by navigating to the pending retries screen.

![Pending Retries Tab](images/pending-retries.png 'width=500')

A pending retry message that is retried will remain in the pending retry list until it is resolved or fails again.


> [!NOTE]
> Supported in ServicePulse versions 1.6.6 and above.

## Enabling the Pending Retries view

To make it visible, follow the steps according to the ServicePulse version that is used.

> [!NOTE]
> In ServicePulse version 1.7.0 and above, the Pending Retries screen is hidden by default. 

### Containers

Add the environment variable `SHOW_PENDING_RETRIES=true` to the container configuration.

### Windows Service

#### ServicePulse version 1.20.0 and above

Add a `showPendingRetry` value in `<path-to-ServicePulse-installation>\app\js\app.constants.js` set to `true`:

```
window.defaultConfig = {
    default_route: '/dashboard',
    version: '1.2.0',
    service_control_url: 'http://localhost:33333/api/',
    monitoring_urls: ['http://localhost:33633/'],
    showPendingRetry: true
};
```

#### ServicePulse version 1.7.0 to 1.20.0 

Change the following value in `<path-to-ServicePulse-installation>\app\js\app.constants.js` to `true`:

```
.constant('showPendingRetry', true)
```

## Messages that are Pending Retries

The Pending Retries screen shows failed messages for which a retry was requested and for which the retry is still pending. The status of retried failures is updated when the message is processed again as either an audited message (i.e. a successful delivery) or as a failed message.

Failed messages that are retried may stay in the pending state for the following reasons:

- The retrying endpoint is not working (e.g. crashed or is scaled-out) and the retried messages are waiting in the queue and have not yet been processed.
- The retry operation failed and the message is in ServiceControl's Dead Letter Queue.
- The retrying endpoint does not have auditing enabled but has successfully processed the retried message(s).

The messages displayed on this screen can be filtered based on the time period by selecting one of the options, such as messages in the last two hours, or messages in the previous day or week. The default option is set to display all pending messages.

![Period Filter](images/pending-retries-period-selection.png 'width=500')

Results can be filtered by queue name using the search functionality:

![Queue Filter](images/pending-retries-filter-queues.png 'width=500')

Detailed information about the message, such as the failure timestamp, endpoint, stack trace of the error, etc., is displayed in the same manner as on the [Failed Messages](intro-failed-messages.md) page, additionally providing information about [redirects](/servicepulse/redirect.md) if one is created for this queue.

## Retrying a message

> [!WARNING]
> Failed messages that are currently in the pending status can be retried; however this feature should be used with care. Retrying pending messages can cause the same message to be processed multiple times. Do not retry a message if it has been processed by the endpoint. In this context "processed" includes both the successful handling of the message and the failure state of it being sent to the error queue.

To retry a message that is pending a retry, select the failed message(s) in the list and click the `Retry Selected` button.

Alternatively a queue can be selected and the `Retry All` option can be used to retry all messages that are targeted for the queue.

Retrying a message will use [message redirects](redirect.md) if the original endpoint has been redirected in ServicePulse.

> [!WARNING]
> A pending retry message that is retried will remain in the pending retry list until it is resolved or fails again.

### Mark as complete

Retried messages are moved to the `Processed` state as soon as ServiceControl receives and processes the retry confirmation from the endpoint.

> [!NOTE]
> Systems running NServiceBus version 7.4 (or earlier) and ServiceControl version 4.19 (or earlier) require [auditing to be enabled both on the endpoint](/nservicebus/operations/auditing.md) and in ServiceControl. Otherwise, the failed message will show as *retry pending* indefinitely even after the message has been successfully processed by the endpoint. In this scenario, use the `Mark as complete` feature to manually mark the failed message as resolved. Once the message is marked as resolved, it will no longer appear in the pending retries message list.


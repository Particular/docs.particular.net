---
title: Pending Retries Message Management
summary: Describes how ServicePulse detects and monitors failed messages in the pending state, and allows retrying, or deleting them.
reviewed: 2019-07-26
---

In Versions 1.6.6 and above, ServicePulse includes an additional screen to view and manage failed messages that have been requested to be retried but have not completed yet.

Pending retries messages can be found by going to the pending retries screen.

![Pending Retries Tab](images/pending-retries.png 'width=500')


### Displaying the Pending Retries view

In ServicePulse version 1.7.0 and above, the Pending Retries screen is hidden by default. It's possible to make it available in the ServicePulse UI. The type of change depends on the ServicePulse version.

### ServicePulse version 1.7.0 until 1.20.0

Change the following value in `<path-to-ServicePulse-installation>\app\js\app.constants.js` to `true`:

```
.constant('showPendingRetry', true)
```

### ServicePulse version 1.20.0 and above

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

### Pending Retries Messages

The Pending Retries Messages screen shows failed messages which have been retried, but the status of that retry is pending. The status of retried failures is updated when either the message is processed again as either an Audited message (success) or as a failed message.

Failed messages that are retried may stay in the pending state for the following reasons:

 * The retrying endpoint is not working (e.g. crashed or is scaled-out) and the retried messages is waiting in the queue and has not yet been processed.
 * The retry operation failed and the message is in ServiceControl's Dead Letter Queue.
 * The retrying endpoint does not have auditing enabled but has successfully processed the retried message.

The messages displayed in this screen can be filtered based on the time period by selecting one of the options, such as messages in the last 2 hours, messages in the previous day or week. The default option is set to display all of the pending messages.

![Period Filter](images/pending-retries-period-selection.png 'width=500')

Results can also be filtered by queue name using the search functionality:

![Queue Filter](images/pending-retries-filter-queues.png 'width=500')

The information about the message such as Failure timestamp, endpoint, stack trace of the error, etc.,  is displayed in the same manner as it on the [Failed Messages](intro-failed-messages.md) page providing additional information as follows:
 
 * **Redirect** Information if redirect is created for this queue.


### Message Retry

WARNING: Failed messages that are currently in the pending status can be retried, however this feature should be used with care. Retrying pending messages can cause the same message to be processed multiple times. Do not retry a message if it has been processed by the endpoint. In this context "processed" includes both the successful handling of the message and the failure state of it being sent to the error queue.

To retry a pending retry message, select the failed message(s) in the list and click the "Retry Selected" button.

Alternately a queue can be selected and the "Retry All" option can be used to retry all the messages targeted for the queue.

Message retry will use [message redirects](redirect.md) if the original endpoint has been redirected in ServicePulse.

WARNING: A pending retry message that is sent for retry will remain in the pending retry list until it is resolved or fails again.


### Mark as complete

When the audit feature is disabled in the endpoint that processes the failed message, the entry will remain in the pending state indefinitely even after the message has been successfully reprocessed by that endpoint. In this scenario, use the `Mark as complete` feature to manually mark the failed message as resolved. Once the message is marked as resolved, it will no longer appear in the pending retries message list.

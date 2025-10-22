---
title: Redirects
summary: Describes what message redirects are and how to use them with failed messages in ServicePulse
related:
- servicepulse/troubleshooting
- samples/pipeline/fix-messages-using-behavior
reviewed: 2025-10-22
---

ServicePulse offers a redirect feature to route messages to a different endpoint than what is specified in the failed message headers. This is useful in cases when a failed message needs to be retried, but the destination endpoint no longer exists.

> [!WARNING]
> Message redirects are a feature of ServicePulse/ServiceControl only; they do not alter the routing for NServiceBus endpoints.

Redirects can be managed from the Configuration page by selecting the `Retry Redirects` tab.

![Redirects Tab](images/redirects.png 'width=500')

## Managing redirects

This page displays the configured redirects that are currently in effect along with the following information:

 * **Source Queue**: the queue for which this redirect will be applied.
 * **Destination Queue**: the queue that will be the new destination when retrying.
 * **Last modified**: a timestamp of the last modification.

To create a redirect, click `Create Redirect`. A dialog will appear.

![Create Redirects Dialog](images/redirects-create.png 'width=500')

Select a source queue from the `From physical address` dropdown list. Type the queue name for the target of the redirect in the `To Physical Address` field. The `To Physical Address` tries to auto-complete the queue name listing all queue names known to ServiceControl. 

> [!IMPORTANT]
> The `To Physical Address` field allows entering arbitrary values to select a queue that is not yet known to ServiceControl. When that happens, a message is shown warning about possible message loss.

To immediately retry all unresolved failed messages from the source address, check the box with the label `Immediately retry any matching failed messages`. This does not apply to failed messages whose retry status is in the Pending state.

Any existing redirects can be deleted using the `End Redirect` link.

To change the target of a redirect, click the `Modify Redirect` link. Only the target queue can be modified. The source queue remains read-only.

### Validation errors

When adding and editing redirects, the following validation rules apply:

 * **Duplicate** There can be only one redirect for a given source queue. When attempting to add multiple redirects for the same source queue, an error message will be displayed: "Can not create redirect to queue *QueueName* as it already has a redirect. Provide a different queue or end the redirect."
 * **Dependent** Redirects cannot be chained i.e., a destination queue in one redirect can not be used as the source queue for another redirect. For example, if there is a configured redirect that redirects messages from QueueA -> QueueB, there cannot be a second rule configured to redirect from QueueB -> QueueC. An error message will be displayed when attempting to chain multiple redirects: "Failed to create a redirect, can not create a redirect to a queue that already has a redirect or is a target of a redirect."

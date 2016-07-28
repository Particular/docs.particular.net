---
title: Redirects
summary: Describes what are redirects and how this feature should be used.
tags:
- ServicePulse
related:
- servicepulse/troubleshooting
---


## Redirect

Creating redirects is a new feature allowing to set up a rule that will be used during message redirection to route messages to a different destination. Redirect will only be used when retrying from ServiceControl, this rule will have no impact on messages send directly using NServiceBus.

To manage redirects go to Configuration page and select tab called `Redirects`.

//TODO: here goes image

### Managing redirects
On the redirect page a list of redirects is being displayed containing following information.
 - **Source Queue** This is the queue for which this redirect will be applied.
 - **Destination Queue** This is the queue that will be new destination when doing retry.
 - **Last modified** This is a timestamp of last modification.

Every redirect can be deleted using `End Redirect` button. This action requires an confirmation. 

When editing a redirect only destination queue can be modified. Source queue is read-only. 

Adding a redirect is very similar to editing with the difference that source queue is editable and need to be filled out. 

#### Validation errors

When adding and editing redirects following validation rules are checked:
 - **Duplicate** There can not be more than one rule for given Source Queue. In that case the following message will be displayed:
 "Can not create redirect to a queue *QueueName* as it already has a redirect. Provide a different queue or end the redirect."
 - **Dependent** There can never be two redirects that one destination queue in one is a destination queue in another. To use an example: when there is a rule QueueA -> QueueB. There can not be a second rule QueueB -> QueueC or QueueC -> QueueA. When this error occur the following message will be displayed:
 "Failed to create a redirect, can not create a redirect to a queue that already has a redirect or is a target of a redirect."

### Immediately retry any matching failed messages
When editing or adding a redirect a checkbox is present allowing to immediately retry all failed messages to which this rule will apply. This will apply only to messages that are failed and not the ones that are pending for retry.

 
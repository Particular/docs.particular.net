---
title: Redirects
summary: Describes what message redirects are and how to use them.
tags:
- ServicePulse
related:
- servicepulse/troubleshooting
---

## Message Redirects

In Versions **1.6.6** and above ServicePulse includes a screen to view and manage failed message redirects that have been created to send failed messages to an alternate queue when the original processing endpoint address is no longer available.

INFO: Message redirects are only a feature of ServicePulse/ServiceControl and will not alter the routing for normal NServiceBus messages in your system.

To manage redirects go to Configuration page and select tab called `Retry Redirects`.

![Redirects Tab](images/redirects.png 'width=500')

### Managing redirects
On the redirect page a list of redirects is being displayed containing following information.
 - **Source Queue** This is the queue for which this redirect will be applied.
 - **Destination Queue** This is the queue that will be new destination when doing retry.
 - **Last modified** This is a timestamp of last modification.

To create a redirect click the `Create Redirect` button. A dialog will appear.

![Create Redirects Dialog](images/redirects-create.png 'width=500')

Choose a source queue from the dropdown. Enter the queue name for the target of the redirect in `To Physical Address` input. If you would like to immediately retry all unresolved failed messages from the sources address check the box with the label `Immediately retry any matching failed messages`.

Once created every redirect can be deleted using `End Redirect` link. This action requires an confirmation. 

To change the target of a redirect click the `Modify Redirect` link. Only the target queue can be modified, the source queue remains read-only.

#### Validation errors

When adding and editing redirects following validation rules are checked:
 - **Duplicate** There can not be more than one rule for given Source Queue. In that case the following message will be displayed:
 > Can not create redirect to a queue *QueueName* as it already has a redirect. Provide a different queue or end the redirect.
 - **Dependent** There can never be two redirects that one destination queue in one is a destination queue in another. To use an example: when there is a rule QueueA -> QueueB. There can not be a second rule QueueB -> QueueC or QueueC -> QueueA. When this error occur the following message will be displayed:
 > Failed to create a redirect, can not create a redirect to a queue that already has a redirect or is a target of a redirect.

### Immediately retry any matching failed messages
When editing or adding a redirect a checkbox is present allowing to immediately retry all failed messages to which this rule will apply. This will apply only to messages that are failed and not the ones that are pending for retry.

 
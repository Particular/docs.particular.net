---
title: Edit message
summary: Describe editing of a message
reviewed: 2018-10-22
component: Core
versions: '[5.0,)'
related:
 - samples/edit-message
---

Editing content of a message is a technique that can be used to fix number of error messages that fail processing due to information in the body. In such case the recommendation is to change to code that sends those messages and repeat the process of sending. IF for any reason this step is not possible editing the message can be a solution to that problem.

This solution makes use of ServiceControl and ServicePulse which are necessary to fulfill this task.

# Editing message

Editing content of a message assumes following steps to be followed:
 - All failed messages that should be edited to be redirected to additional specially created endpoint
 - That endpoint modify the content of that messages and send them back to originating endpoints

## Redirecting error message

To forward error messages to another endpoint [redirect](/servicepulse/redirect.md) need to be created to send all retried messages from Service Pulse/Service Control to newly created endpoint. Redirects does not influence routing with the business system, only retries that originates from the platform tools.

## Editing endpoint

Solution needs another endpoint to be created which will process the error messages, modify their content and send them to originating endpoint. In that  way number of failed messages may be processed and resubmitted for further processing.

### Logging

Introducing an endpoint that modify message content and sends it further introduces risk in the system that can be mitigated by introducing logging. It is recommended to log what message was edited and what values where changed. In case of having error in this code logging might give insight into what values were previously stored.

### Limitations

Using endpoint to edit message allow for modification on body and headers that were successfully deserialized. As this solution used NServiceBus and message contracts that were used for sending message need to be properly read for this code to work. 

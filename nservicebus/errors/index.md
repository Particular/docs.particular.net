---
title: Error handling
summary: Don't try to handle exceptions in your message handlers. Let NServiceBus do it for you.
tags:
- Exceptions
- Error Handling
- Automatic Retries
redirects:
 - nservicebus/how-do-i-handle-exceptions
---

Don't.

NServiceBus has exception catching and handling logic of its own which surrounds all calls to user code. When an exception bubbles through to the NServiceBus infrastructure, it rolls back the transaction on a transactional endpoint, causing the message to be returned to the queue, and any messages that the user code tried to send or publish to be undone as well.

At that point, NServiceBus retries to handle that message a configurable number of times (default of 5) and if the message fails on every one of those retries, the message is then moved to the configured error queue. For details, see discussion on [Second-level Retries](second-level-retries.md).

Administrators should monitor that error queue so that they can see when problems occur. The message in the error queue contains the source queue and machine so that the administrator can see what's wrong with that node and possibly correct the problem (like bringing up a database that went down).

Monitoring and handling of failed messages with [ServicePulse](/servicepulse) provides access to full exception details (including stacktrace, and throught ServiceInsight it also enables advanced debugging with all message context. It also provides a manual "retry" option (i.e. send the message for re-processing). for more details, see [Introduction to Failed Messages Monitoring in ServicePulse](/servicepulse/intro-failed-messages.md). 

If ServicePulse or ServiceInsight are not available in your environment, you can use of the  `ReturnToSourceQueue.exe` tool to send the relevant message back to its original queue so that it can be processed again. The `ReturnToSourceQueue` tool is specific to MSMQ, and can be found in the [NServiceBus GitHub repository](https://github.com/Particular/NServiceBus).

For more information on this process, [Transactions Message Processing](/nservicebus/operations/transactions-message-processing.md).

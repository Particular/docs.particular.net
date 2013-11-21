---
title: How to Handle Exceptions
summary: Don&#39;t try to handle exceptions in your message handlers. Let NServiceBus do it for you.
originalUrl: http://www.particular.net/articles/how-do-i-handle-exceptions
tags:
- Exceptions
- Error Handling
- Automatic Retries
---

Don't.

NServiceBus has exception catching and handling logic of its own which surrounds all calls to user code. When an exception bubbles through to the NServiceBus infrastructure, it rolls back the transaction on a transactional endpoint, causing the message to be returned to the queue, and any messages that the user code tried to send or publish to be undone as well.

At that point, NServiceBus retries to handle that message a configurable number of times (default of 5) and if the message fails on every one of those retries, the message is then moved to the configured error queue.

Administrators should monitor that error queue so that they can see when problems occur. The message in the error queue contains the source queue and machine so that the administrator can see what's wrong with that node and possibly correct the problem (like bringing up a database that went down).

Once the administrator corrects the problem, they can use the ReturnToSourceQueue.exe tool to send the relevant message back to its original queue so that it can be processed again; this time, successfully.

The tool itself is in the tools folder in the NServiceBus [download binaries](downloads).

For more information on this process, [click here](transactions-message-processing.md).


---
title: Declaring usages of queues
summary: Explains how to tell NServiceBus about queues being used by a feature
tags:
 - queue
 - QueueBindings
---

NServiceBus features may require additional queues to work. A built-in example is the audit feature which needs the audit queue. During start-up NServiceBus ensures the declared queues are present and aborts the start-up procedure if they are not (with an exception of MSMQ remote queues which are optional).

<!-- import queuebindings -->


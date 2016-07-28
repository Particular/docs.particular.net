---
title: Critical exception wait time
summary: A critical error is raised when timeout retrieval fails.
reviewed: 2016-03-02
tags:
- timeout
redirects:
 - nservicebus/how-do-i-specify-time-to-wait-before-raising-critical-exception-for-timeout-outages
---

When using [Timeout Manager](/nservicebus/messaging/delayed-delivery#caveats) messages schedule for delayed delivery will be stored using configured persistence mechanism. If there are any problems with timeout storage by default a wait of 2 seconds is done to allow the storage to come back online.

To change the default wait time:

snippet:TimeToWaitBeforeTriggeringCriticalErrorOnTimeoutOutages

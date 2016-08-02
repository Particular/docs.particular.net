---
title: Individualizing queue names
summary: Enabling individualizing queue names
tags:
- Scale Out
redirects:
 - nservicebus/individualizing-queues-when-scaling-out
---

By default all instances of a given NServiceBus endpoint use the same input queue in a competing consumers pattern. For certain specific usage scenarios this behavior can be changed so that each instance has a unique own queue.

NOTE: The endpoint name stays the same for all instances.

snippet:content 

---
title: When Endpoint Instance Starts and Stops
summary: An interface that allows hooking into into the startup and shutdown sequence of an endpoint instance.
reviewed: 2016-12-02
component: core
tags:
 - life-cycle
related:
 - samples/startup-shutdown-sequence
redirects:
 - nservicebus/lifecycle/iwanttorunwhenbusstartsandstops
---


Classes that plug into the startup/shutdown sequence are invoked just after the endpoint instance has been started and just before it is stopped. Use this approach for any tasks that need to execute with the same life-cycle as the endpoint instance.


partial: content
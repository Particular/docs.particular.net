---
title: Scheduler
summary: A simple example of the NServiceBus scheduler API
reviewed: 2021-07-28
component: Core
related:
 - nservicebus/scheduling
---

This sample illustrates a simple example of the [scheduler](/nservicebus/scheduling/) API. Two common use cases are:

 1. Sending a message.
 1. Executing some custom code.

The scheduling API is accessed via in instance of `IEndpointInstance`.

snippet: Schedule

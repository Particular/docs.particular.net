---
title: Scheduler
summary: A simple example of the NServiceBus scheduler API
reviewed: 2024-07-15
component: Core
related:
 - nservicebus/scheduling
---

This sample illustrates a simple example of the built-in [scheduler](/nservicebus/scheduling/?version=core_7) API. Two common use cases are:

 1. Sending a message.
 1. Executing some custom code.

The scheduling API is accessed via an instance of `IEndpointInstance`.

snippet: Schedule

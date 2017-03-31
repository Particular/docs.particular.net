---
title: Host Default Logging
summary: The default logging capability of the NServiceBus host.
reviewed: 2016-03-30
component: Host
tags:
- Logging
related:
- nservicebus/logging
---

## Code walk-through

This sample shows how to customize logging when running inside the NServiceBus host.

Open the app.config file. See the Logging configuration section at the top as well as its contents:

snippet: ThresholdConfig

This code instructs NServiceBus to only output logs at a level of INFO or higher.
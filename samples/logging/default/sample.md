---
title: Default Logging
summary: The default logging capability of NServiceBus.
reviewed: 2026-05-27
component: Core
versions: '[, 9)'
related:
- nservicebus/logging
---

> [!NOTE]
> Starting with NServiceBus 10.2, endpoints hosted with the [.NET Generic Host](/nservicebus/hosting/core-hosting.md) using `AddNServiceBusEndpoint` automatically use `Microsoft.Extensions.Logging` with rolling file and console defaults. The `DefaultFactory` API shown in this sample is deprecated. See [Logging](/nservicebus/logging/) for modern configuration options.

## Introduction

This sample shows the [default logging functionality](/nservicebus/logging/#default-logging) in NServiceBus.

### Enabling Logging

snippet: ConfigureLogging


## Verifying that the sample works correctly

 A log file called `nsb_log_[current_date]_0` is created inside the `[path_to_the_sample]\bin\[Debug/Release]` directory.




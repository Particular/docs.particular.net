---
title: NServiceBus.Extensions.Logging Usage
summary: A sample that uses Microsoft.Extensions.Logging with NLog
component: Extensions.Logging
reviewed: 2026-05-27
related:
 - nservicebus/logging
---

partial: intro

### Configure NLog

NLog in this example is configured in code:

snippet: NLogConfiguration

> [!NOTE]
> There is no preference on how NLog is configured. Based on the NLog documentation, the most used method is with an [NLog configuration file](https://github.com/nlog/nlog/wiki/Configuration-file#configuration).

> [!WARNING]
> When using self-hosted endpoints, it is important that the NLog and Microsoft.Extensions.Logging abstractions are initialized before `Endpoint.Start` is invoked. When hosting with the .NET Generic Host, logging providers are registered on the host builder and the initialization is handled automatically by the host lifecycle.

### Configure logging abstractions

The following snippet shows how to register NLog as a logging provider. NLog has its own provider extensions for Microsoft.Extensions.Logging and needs an `NLogLoggerFactory` provider that implements an `Microsoft.Extensions.Logging.ILoggerFactory` instance so that `Microsoft.Extensions.Logging` can use NLog.

snippet: MicrosoftExtensionsLoggingNLog

Endpoints can be created or started only after the logging initialization has completed.

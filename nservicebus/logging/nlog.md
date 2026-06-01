---
title: NLog
summary: Logging to NLog
reviewed: 2026-05-27
component: NLog
---

partial: obsolete-warning

## Logging integration into the host

When using NServiceBus 10.2 or later with the [.NET Generic Host](/nservicebus/hosting/core-hosting.md), configure NLog directly on the host builder. The bridge package is not required:

```csharp
var builder = Host.CreateApplicationBuilder();

builder.Logging.AddNLog();

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);
```

For more information, see [Hosting with Microsoft.Extensions.Hosting](/nservicebus/hosting/core-hosting.md).

partial: usage


## Filtering

NServiceBus can write a significant amount of information to the log. To limit this information use the filtering features of the underlying logging framework.

For example to limit log output to a specific namespace.

Here is a code configuration example for adding a [Rule](https://github.com/nlog/NLog/wiki/Configuration-file#rules).

snippet: NLogFiltering

partial: nlog-exception-data

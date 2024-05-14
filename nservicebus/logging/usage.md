---
title: Logging using the NServiceBus logging abstraction
reviewed: 2024-05-14
component: Core
redirects:
- nservicebus/logging-writing
---

> [!Note]
> It is recommended to directly use the [`Microsoft.Extensions.Logging`](https://learn.microsoft.com/en-us/dotnet/core/extensions/logging/) to log entries as it also supports semantic logging. Please see [Logging in .NET Core and ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/) for further details.

In legacy endpoints the NServiceBus logging abstraction is used for writing log messages from user code.

Set up a single static field to an `ILog` in the classes, and then use it in all methods:

snippet: UsingLogging

> [!WARNING]
> Make sure that logging is correctly initialized before resolving the `ILog` instance. Not doing so can result in a logger using an incorrect configuration

> [!NOTE]
> To avoid unnecessary processing, especially when logging more verbose messages, such as `Debug`, make sure to first check if logging at that level is enabled.

> [!NOTE]
> Since `LogManager.GetLogger(..);` is an expensive call, it is important that the field is `static` so that the call happens only once per class and has the best possible performance.

> [!NOTE]
> The `*Format` APIs pass their message and format arguments to the corresponding APIs of the underlying logging framework so their behavior varies. Some frameworks, like NLog, use special syntax to create structured log entries. Refer to the documentation of the specific logging framework for details. The built-in logging uses `string.Format` to generate the message that is written.

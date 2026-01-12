---
title: Critical Errors
summary: How to handle critical errors in NServiceBus that adversely affect messaging in an endpoint.
reviewed: 2026-01-12
component: Core
---

## What are critical errors?

NServiceBus has the ability to handle message processing failures through the [recoverability feature](/nservicebus/recoverability/). However, there may be other types of errors outside of message processing that NServiceBus does not have enough context to handle gracefully. These tend to be deeper infrastructure issues that cannot be caught by the recoverability feature of message processing. NServiceBus raises these as critical errors.

Examples of **critical errors** include:

* An exception occurs when NServiceBus attempts to execute the recoverability policy, including moving a message to the error queue. The context will contain a specific error `Failed to execute recoverability policy for message with native ID: "{message.MessageId}"`
* There are repeated failures in reading information from the required storage.
* An exception occurs while reading from the input queue.

## What happens when a critical error occurs in NServiceBus?

From version 7.0, the default behaviour is to log the exception and keep retrying indefinitely.

Often, critical errors are transient (e.g. a database was temporarily unavailable). An immediate retry can be successful in these cases, where the system will continue processing where it left off.

## How do I deal with persistent critical errors?

Critical errors can sometimes be persistent. When a critical error persists, it is often unknown if the issue is recoverable. [Stopping the endpoint](#how-do-i-deal-with-persistent-critical-errors-stop-the-endpoint) along with [terminating and restarting the process](#how-do-i-deal-with-persistent-critical-errors-terminate-and-restart-the-process) is recommended.

### Stop the endpoint

[Microsoft Generic Host's](/nservicebus/hosting/extensions-hosting.md) `IHostApplicationLifetime.Stop` method stops the NServiceBus endpoint gracefully.

Alternatively, a call to `criticalErrorContext.Stop` can be used.

snippet: StopEndpointInCriticalError

> [!WARNING]
Calling `criticalErrorContext.Stop` without terminating the host process will only stop the NServiceBus endpoint, without affecting the host process or other components running within the same process. This is why restarting the process after stopping the endpoint is the recommended approach.

### Terminate and restart the process

1. Terminate the process. If using `Environment.FailFast` or `IHostApplicationLifetime.Stop`, the NServiceBus endpoint can attempt a graceful shutdown, which can be useful in non-transactional processing environments.
2. Ensure the environment is configured to automatically restart processes when they stop.
  * IIS: The IIS host will automatically spawn a new instance.
  * Windows Service: The OS can restart the service after 1 minute if [Windows Service Recovery](/nservicebus/hosting/windows-service.md#installation-setting-the-restart-recovery-options) is enabled.
  * Docker: Ensure that containers are configured with `restart=always`. See [Start containers automatically (Docker.com)](https://docs.docker.com/config/containers/start-containers-automatically/)

## What if I need to override the default behavior?

The default behavior is often appropriate for the lifetime of most systems. However, it is possible to override the default behavior to accommodate business needs.

For example, the default behavior can be modified with:

* Sending a real-time notification to support personnel when the endpoint has raised a critical error.
* Limiting the retries of the endpoint handler, e.g. when it might affect costs.
* Automatically restarting the endpoint and resetting the transport connection to attempt to resolve underlying issues in receiving or dispatching messages.

To override the default behavior a custom [action](https://learn.microsoft.com/en-us/dotnet/api/system.action-1) needs to be provided:

snippet: DefiningCustomHostErrorHandlingAction

### Example of a custom implementation

> [!NOTE]
> The following implementation assumes that the endpoint instance is hosted in isolation and that the hosting environment of the process will restart the process after it has been killed.

snippet: CustomHostErrorHandlingAction

### Implementation concerns

> [!NOTE]
> If the endpoint is stopped without exiting the process, then any `Send` or `Publish` operation will result in a [KeyNotFoundException](https://msdn.microsoft.com/en-us/library/system.collections.generic.keynotfoundexception) being thrown.

When implementing a custom critical error callback:

* Decide if the process can be exited/terminated and use the [Environment.FailFast](https://docs.microsoft.com/en-us/dotnet/api/system.environment.failfast) method to exit the process. If the environment has threads running that should be completed before shutdown (e.g. non transactional operations), the [Environment.Exit](https://docs.microsoft.com/en-us/dotnet/api/system.environment.exit) method can also be used.
* The code should be wrapped in a `try...finally` clause. In the `try` block, perform any custom operations; in the `finally` block, call the method that exits the process.
* The custom operations should include flushing any in-memory state and cached data, if normally it is persisted at a certain interval or during graceful shutdown. For example, flush appenders when using buffering or asynchronous logging for [Serilog](https://github.com/serilog/serilog/wiki/Lifecycle-of-Loggers) via `Log.CloseAndFlush();`, or [NLog](https://nlog-project.org/documentation/v4.3.0/html/M_NLog_LogManager_Shutdown.htm) and [log4net](https://logging.apache.org/log4net/log4net-1.2.11/release/sdk/log4net.LogManager.Shutdown.html) by calling `LogManager.Shutdown();`.

## Raising a critical error

Any part of the endpoint's implementation can invoke the `criticalError` action.

snippet: InvokeCriticalError

## Heartbeat functionality

The [Heartbeat functionality](/monitoring/heartbeats/) is configured to start pinging ServiceControl immediately after the endpoint starts. It only stops when the process exits. The only way for a critical error to cause  a heartbeat failure in ServicePulse/ServiceControl is for it to kill the process.

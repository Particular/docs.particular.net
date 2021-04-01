---
title: Critical Errors
summary: How to handle critical errors which adversely affect messaging in an endpoint.
reviewed: 2021-03-19
component: Core
---

NServiceBus has built-in [recoverability](/nservicebus/recoverability/) but in certain scenarios, it is not possible to handle errors in a graceful way. The reason for this is that NServiceBus does not have enough context to make a sensible decision on how to proceed after these errors have occurred.

Examples of **critical errors** include:

* An exception occurs when NServiceBus attempts to execute the recoverability policy, including moving a message to the error queue. The context will contain a specific error  `Failed to execute recoverability policy for message with native ID: \`{message.MessageId}\``
* There are repeated failures in reading information from a required storage.
* An exception occurs reading from the input queue.

### Default behavior

partial: default

## Custom handling

A custom critical error handler can be provided to override the default behavior.

Examples of reasons to consider implementating a custom critical error action include:

* Restarting the endpoint and resetting the transport connection may resolve underlying issues in receiving or dispatching messages.
* To notify support personnel when the endpoint has raised a critical error.
* The endpoint contains a handler which must not be executed beyond the configured recoverability policy.

Define a custom handler using the following code.

snippet: DefiningCustomHostErrorHandlingAction

### Terminating the process

It is often unknown if the issue is recoverable when a critical error occurs. A sound strategy is to terminate the process when a critical error occurs and rely on the process hosting environment to restart the process as a recovery mechanism, resulting in a resilient way to deal with critical errors.

However, this strategy only works when the endpoint instance is hosted in isolation and does not have another component. For example, when co-hosting NServiceBus with a web-service or website, terminating the process would result in these components becoming unavailable to users or other systems.

### Host OS recoverability

Whenever possible rely on the environment hosting the endpoint process to automatically restart it:

* IIS: The IIS host will automatically spawn a new instance.
* Windows Service: The OS can restart the service after 1 minute if [Windows Service Recovery](/nservicebus/hosting/windows-service.md#installation-setting-the-restart-recovery-options) is enabled.
* Docker: Ensure that containers are configured with `restart=always`. See [Start containers automatically (Docker.com)](https://docs.docker.com/config/containers/start-containers-automatically/)

### A possible custom implementation

NOTE: The following implementation assumes that the endpoint instance is hosted in isolation and that the hosting environment of the process will restart the process after it has been killed.

snippet: CustomHostErrorHandlingAction

### Implementation concerns

partial: override

When implementing a custom critical error callback:

* Decide if the process can be exited/terminated and use the [Environment.FailFast](https://docs.microsoft.com/en-us/dotnet/api/system.environment.failfast) method to exit the process. If the environment has threads running that should be completed before shutdown (e.g. non transactional operations), the [Environment.Exit](https://docs.microsoft.com/en-us/dotnet/api/system.environment.exit) method can also be used.
* The code should be wrapped in a `try...finally` clause. In the `try` block perform any custom operations; in the `finally` block call the method that exits the process.
* The custom operations should include flushing any in-memory state and cached data, if normally it is persisted at a certain interval or during graceful shutdown. For example, flush appenders when using buffering or asynchronous logging for [Serilog](https://github.com/serilog/serilog/wiki/Lifecycle-of-Loggers) via `Log.CloseAndFlush();`, or [NLog](https://nlog-project.org/documentation/v4.3.0/html/M_NLog_LogManager_Shutdown.htm) and [log4net](https://logging.apache.org/log4net/log4net-1.2.11/release/sdk/log4net.LogManager.Shutdown.html) by calling `LogManager.Shutdown();`.

## Raising a critical error

Any code in the endpoint can invoke the Critical Error action.

snippet: InvokeCriticalError

## Heartbeat functionality

The [Heartbeat functionality](/monitoring/heartbeats/) is configured to start pinging ServiceControl immediately after the endpoint starts. It only stops when the process exits. The only way for a critical error to result in a heartbeat failure in ServicePulse/ServiceControl is for the critical error to kill the process.

---
title: Critical Errors
summary: How to handle critical errors in NServiceBus that adversely affect messaging in an endpoint.
reviewed: 2024-04-03
component: Core
---

## What are critical errors?

NServiceBus has the ability to handle message processing failures through the [recoverability feature](/nservicebus/recoverability/). However, there may be other types of errors outside of message processing that NServiceBus does not have enough context to handle gracefully. These tend to be deeper infrastructure issues that cannot be caught by the recoverability feature of messages. NServiceBus raises these as critical errors.

Examples of **critical errors** include:

* An exception occurs when NServiceBus attempts to execute the recoverability policy, including moving a message to the error queue. The context will contain a specific error `Failed to execute recoverability policy for message with native ID: \`{message.MessageId}\``
* There are repeated failures in reading information from a required storage.
* An exception occurs reading from the input queue.

## What happens when a critical error occurs in NServiceBus?

partial: default

## How do I deal with critical errors?

Often, critical errors are transient (e.g. database is temporarily unavailable), and once resolved, the system will continue processing where it left off with no further action or intervention. Thus, the default behavior is often indefinitely sufficient for most systems. However, there may be situations when overriding the default behavior is preferred.

## When should I override the default behavior?

Some situations when overriding the default behavior might be useful are:

* To notify support personnel when the endpoint has raised a critical error.
* To stop an endpoint handler from executing beyond the configured recoverability policy.
* To allow deliberate restarting of the endpoint and resetting the transport connection to attempt to resolve underlying issues in receiving or dispatching messages.

## How do I override the critical error default behavior?

It's possible to provide a custom [Action](https://learn.microsoft.com/en-us/dotnet/api/system.action-1) to override the default behavior:

snippet: DefiningCustomHostErrorHandlingAction

### Critical error handling strategies

#### Terminate and restart the process

1. Ensure the environment is configured to automatically restart processes when they stop.
  * IIS: The IIS host will automatically spawn a new instance.
  * Windows Service: The OS can restart the service after 1 minute if [Windows Service Recovery](/nservicebus/hosting/windows-service.md#installation-setting-the-restart-recovery-options) is enabled.
  * Docker: Ensure that containers are configured with `restart=always`. See [Start containers automatically (Docker.com)](https://docs.docker.com/config/containers/start-containers-automatically/)
2. Terminate the process. If using `Environment.FailFast` or `IHostApplicationLifetime.Stop`, the NServiceBus endpoint can attempt a graceful shutdown which can be useful in non-transactional processing environments.

#### Stop the endpoint

To only stop the endpoint without terminating the process, the [Microsoft Generic Host's](/nservicebus/hosting/extensions-hosting.md) `IHostApplicationLifetime.Stop` method stops the NServiceBus endpoint gracefully.

Warn: Calling `criticalErrorContext.Stop` without terminating the host process will only stop the NServiceBus endpoint without affecting the host process and other components running within the same process. It is recommended to restart the process after stopping the endpoint.

snippet: StopEndpointInCriticalError

### Example custom implementation

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

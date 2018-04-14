---
title: Critical Errors
summary: How to handle critical errors which adversely affect messaging in an endpoint.
reviewed: 2018-01-29
component: Core
tags:
- Hosting
- Logging
---

NServiceBus has built-in [recoverability](/nservicebus/recoverability/) but in certain scenarios, it is not possible to handle errors in a graceful way. The reason for this is that NServiceBus does not have enough context to make a sensible decision on how to proceed after these errors have occurred. Examples of these **critical errors** include:

 * An exception occurs when NServiceBus is attempting to move a message to the error queue.
 * There are repeated failures in reading information from a required storage.
 * An exception occurs reading from the input queue.


### Default behavior

partial: default


### Logging of critical errors

partial: logging


## Custom handling

It is possible to providing a delegate that overrides the above action. When a critical error occurs the new action will be called instead of the default.

Define a custom handler using the following code.

snippet: DefiningCustomHostErrorHandlingAction


## A possible custom implementation

snippet: CustomHostErrorHandlingAction


## When to override the default critical error action

partial: override

When implementing a custom critical error callback:

 * To exit the process use the [Environment.FailFast](https://msdn.microsoft.com/en-us/library/dd289240.aspx) method. In case the environment has threads running that should be completed before shutdown (e.g. non transactional operations), the [Environment.Exit](https://msdn.microsoft.com/en-us/library/system.environment.exit.aspx) method can also be used.
 * The code should be wrapped in a `try...finally` clause. In the `try` block perform any custom operations; in the `finally` block call the method that exits the process.
 * The custom operations should include flushing any in-memory state and cached data, if normally it is persisted at a certain interval or during graceful shutdown. For example, flush appenders when using buffering or asynchronous appenders for [NLog](http://nlog-project.org/documentation/v4.3.0/html/M_NLog_LogManager_Shutdown.htm) or [log4net](https://logging.apache.org/log4net/log4net-1.2.11/release/sdk/log4net.LogManager.Shutdown.html) state by calling `LogManager.Shutdown();`.

Whenever possible rely on the environment hosting the endpoint process to automatically restart it:

 * IIS: The IIS host will automatically spawn a new instance.
 * Windows Service: The OS can restart the service after 1 minute if [Windows Service Recovery](/nservicebus/hosting/windows-service.md#installation-restart-recovery) is enabled.

WARNING: It is important to consider the effect these defaults will have on other things hosted in the same process. For example if co-hosting NServiceBus with a web-service or website.


## Raising a critical error

Any code in the endpoint can invoke the Critical Error action.

snippet: InvokeCriticalError


## Heartbeat functionality

The [Heartbeat functionality](/monitoring/heartbeats/) is configured to start pinging ServiceControl immediately after the bus starts. It only stops when the process exits. The only way for a critical error to result in a heartbeat failure in ServicePulse/ServiceControl is for the critical error to kill the process.

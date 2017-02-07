---
title: Critical Errors
summary: How to handle critical errors which adversely affect messaging in an endpoint.
reviewed: 2016-08-29
component: Core
tags:
- Hosting
- Logging
---

For many scenarios NServiceBus has built-in [recoverability](/nservicebus/recoverability/), however certain scenarios are not possible to handle in a graceful way. The reason for this is that NServiceBus does not have enough context to make a sensible decision on how to proceed after these error have occurred. Some examples **Critical Errors** include:

 * An Exception occurs when NServiceBus is attempting to move a message to the Error Queue.
 * There are repeated failures in reading information from a required storage.
 * An exception occurs reading from the input queue.


### Default action handling in NServiceBus

And hence the default behavior that will be taken in any kind of self hosting scenario.

snippet:DefaultCriticalErrorAction


### Logging of critical errors

partial: logging


## Custom handling

NServiceBus allows providing a delegate that overrides the above action. So when a Critical Error occurs the new action will be called instead of the default.

Define a custom handler using the following code.

snippet:DefiningCustomHostErrorHandlingAction


## A possible custom implementation

Next define what action to take when this scenario occurs:

snippet:CustomHostErrorHandlingAction


## When to override the default critical error action

When self hosting the critical error callback must be overriden in order to exit the current process. Not overriding the critical error callback will result in the endpoint instance to stop processing messages without the ability to recover from this state.

NOTE: If not killing the process and just disposing the bus, be aware that any `Send` operations will result in [ObjectDisposedException](https://msdn.microsoft.com/en-us/library/system.objectdisposedexception.aspx) being thrown.

Consider the following when implementing a critical error callback:

- If calling any code wrap it in a `try..finally`, making sure the process will invoke exit code in the `finally` like `Environment.FailFast` if any code fails.
- Flush any loggers used which makes sure any unwritten log state is written/pushed to its target(s) making sure the tail of the log is not lost as [Environment.FailFast](https://msdn.microsoft.com/en-us/library/dd289240.aspx) will immediately exit the process.
 - Flush appenders for [NLog](http://nlog-project.org/documentation/v4.3.0/html/M_NLog_LogManager_Shutdown.htm) or [log4net](https://logging.apache.org/log4net/log4net-1.2.11/release/sdk/log4net.LogManager.Shutdown.html) state by calling `LogManager.Shutdown();`.
- Flush any state/caches used if such data structures would be written/persisted at a certain interval or regular graceful shutdown.
- It is easiest to call [Environment.FailFast](https://msdn.microsoft.com/en-us/library/dd289240.aspx)

Rely on the environment that the process will be automatically restarted. When hosting in IIS the IIS host will automatically spawn a new instance and when hosting as a Windows Service the OS will restart the service after 1 minute if [Windows Service Recovery](/nservicebus/hosting/windows-service.md#installation-restart-recovery) is enabled.


WARNING: It is important to consider the effect these defaults will have on other things hosted in the same process. For example if co-hosting NServiceBus with a web-service or website.



## Raising Critical error

Any code in the endpoint can invoke the Critical Error action.

snippet:InvokeCriticalError


## ServicePulse and ServiceControl Heartbeat functionality

The [ServicePulse/ServiceControl Heartbeat functionality](/servicepulse/intro-endpoints-heartbeats.md) is configured to start pinging ServiceControl immediately after the bus starts. It only stops when the process exits. So the only way for a Critical Error to result in a Heartbeat failure in ServicePulse/ServiceControl is for the Critical Error to kill the process.

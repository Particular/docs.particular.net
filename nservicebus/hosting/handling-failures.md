---
title: Handling failures 
summary: How to handle critical errors which adversely affect messaging in your endpoint.
tags:
- NServiceBus Host
- Self Hosting
---

For many scenarios NServiceBus has built-in error and exception management, for example message retrying, however certain scenarios are not possible to handle in a graceful way. The reason for this is that NServiceBus does not have enough context to make a sensible decision on how to proceed after these error have occurred. Some of these **Critical Errors** include:

 * An Exception occurs when NServiceBus is attempting to move a message to the Error Queue.
 * There are repeated failures in reading information from a required storage storage.
 * An exception occurs reading from the input queue.
 * A `IWantToRunWhenBusStartsAndStops.Start` throws an exception.
 
### Default action handling in NServiceBus Core

And hence the default behavior that will be taken in any kind of self hosting scenario.

<!-- import DefaultCriticalErrorAction -->

NOTE:  In Version 4 and Version 3 the bus was shutdown but not disposed. This means sending of messages is allowed but no processing of messages will occur.

### Default action handling in NServiceBus.Host

<!-- import DefaultHostCriticalErrorAction -->

WARNING: It is important to consider the effect these defaults will have on other things hosted in the same process. For example if you are co-hosting NServiceBus with a web-service or website.  

### Logging of critical errors

For Version 4 and up Critical Errors are logged before firing the handler action.

<!-- import DefaultCriticalErrorActionLogging -->

NOTE:  Version 3 does not write a log entry when a critical error occurs. If you require logging you should override the default action as defined below.

## Custom handling

NServiceBus allows you to provide a delegate that overrides the above action. So when a Critical Error occurs the new action will be called instead of the default.
 
You define a custom handler using the following code.

<!-- import DefiningCustomHostErrorHandlingAction -->

## A possible custom implementation

Next you define what action you want to take when this scenario occurs:

<!-- import CustomHostErrorHandlingAction -->

## When should you override the default action

The default action should be overridden whenever that default does not meet your specific hosting requirements. For example 

- If you are using NServiceBus Host, and you wish to take a custom action before the endpoint process is killed.
- If you are self hosting you can call shut down the process, via `Environment.FailFast`, and re-start the process one the root cause has been diagnosed. 

NOTE: If you choose to not kill the process and just dispose the bus, please be aware that any `bus.Send` operations will result in `ObjectDisposedException` being thrown.

## ServicePulse and ServiceControl Heartbeat functionality

The [ServicePulse/ServiceControl Heartbeat functionality](/servicepulse/intro-endpoints-heartbeats.md) is configured to start pinging ServiceControl immediately after the bus starts. It only stops when the process exits. So the only way for a Critical Error to result in a Heartbeat failure in ServicePulse/ServiceControl is for the Critical Error to kill the process.
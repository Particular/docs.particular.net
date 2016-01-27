---
title: Critical Errors
summary: How to handle critical errors which adversely affect messaging in your endpoint.
tags:
- Hosting
- Self Hosting
- Logging
---

For many scenarios NServiceBus has built-in error and exception management, for example message retrying, however certain scenarios are not possible to handle in a graceful way. The reason for this is that NServiceBus does not have enough context to make a sensible decision on how to proceed after these error have occurred. Some of these **Critical Errors** include:

 * An Exception occurs when NServiceBus is attempting to move a message to the Error Queue.
 * There are repeated failures in reading information from a required storage.
 * An exception occurs reading from the input queue.
 * A `IWantToRunWhenBusStartsAndStops.Start` throws an exception.


### Default action handling in NServiceBus

And hence the default behavior that will be taken in any kind of self hosting scenario.

snippet:DefaultCriticalErrorAction

NOTE: In Version 4 and Version 3 the bus stops processing messages but is not disposed. This means sending of messages is allowed but no processing of messages will occur.


### Default action handling in NServiceBus.Host

snippet:DefaultHostCriticalErrorAction

WARNING: It is important to consider the effect these defaults will have on other things hosted in the same process. For example if you are co-hosting NServiceBus with a web-service or website.


### Logging of critical errors

For Version 4 and up Critical Errors are logged inside the critical error action. This means that if you replace the Critical Error in these versions you should also write the log entry.

snippet:DefaultCriticalErrorActionLogging

NOTE: Version 3 does not write a log entry as part of default Critical Error handler. This is done high up the stack in the location where the exception occurs. It is for this reason that the `Exception` instance is not passed to the Critical Error handler. So it is optional to write a log entry when replacing the Critical Error in Version 3.


## Custom handling

NServiceBus allows you to provide a delegate that overrides the above action. So when a Critical Error occurs the new action will be called instead of the default.

You define a custom handler using the following code.

snippet:DefiningCustomHostErrorHandlingAction


## A possible custom implementation

Next you define what action you want to take when this scenario occurs:

snippet:CustomHostErrorHandlingAction


## When should you override the default action

The default action should be overridden whenever that default does not meet your specific hosting requirements. For example

- If you are using NServiceBus Host, and you wish to take a custom action before the endpoint process is killed.
- If you are self hosting you can shut down the process via `Environment.FailFast` and re-start the process once the root cause has been diagnosed.

NOTE: If you choose to not kill the process and just dispose the bus, please be aware that any `Send` operations will result in `ObjectDisposedException` being thrown.


## Raising Critical error

Any code in your endpoint can invoke the Critical Error action.

snippet:InvokeCriticalError


## ServicePulse and ServiceControl Heartbeat functionality

The [ServicePulse/ServiceControl Heartbeat functionality](/servicepulse/intro-endpoints-heartbeats.md) is configured to start pinging ServiceControl immediately after the bus starts. It only stops when the process exits. So the only way for a Critical Error to result in a Heartbeat failure in ServicePulse/ServiceControl is for the Critical Error to kill the process.

---
title: Critical Errors
summary: How to handle critical errors which adversely affect messaging in an endpoint.
reviewed: 2016-08-29
component: Core
tags:
- Hosting
- Self Hosting
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


## When to override the default action

The default action should be overridden whenever that default does not meet the specific hosting requirements. For example

 * If using [NServiceBus Host](/nservicebus/hosting/nservicebus-host), and wish to take a custom action before the endpoint process is killed.
 * If self hosting the process can be shut down the process via [Environment.FailFast](https://msdn.microsoft.com/en-us/library/dd289240.aspx) and re-start the process once the root cause has been diagnosed.

NOTE: If not killing the process and just dispose the bus, be aware that any `Send` operations will result in [ObjectDisposedException](https://msdn.microsoft.com/en-us/library/system.objectdisposedexception.aspx) being thrown.


## Raising Critical error

Any code in the endpoint can invoke the Critical Error action.

snippet:InvokeCriticalError


## ServicePulse and ServiceControl Heartbeat functionality

The [ServicePulse/ServiceControl Heartbeat functionality](/servicepulse/intro-endpoints-heartbeats.md) is configured to start pinging ServiceControl immediately after the bus starts. It only stops when the process exits. So the only way for a Critical Error to result in a Heartbeat failure in ServicePulse/ServiceControl is for the Critical Error to kill the process.

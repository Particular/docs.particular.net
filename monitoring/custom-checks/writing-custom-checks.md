---
title: Writing Custom Checks
summary: Authoring one-off or periodic checks with the Custom Checks plugin
reviewed: 2018-01-05
component: CustomChecks
versions: 'CustomChecks:*'
---

There are two types of custom checks. 


## One-time custom check

A one-time custom check is executed once when the endpoint host starts. NServiceBus assembly scanning mechanism detects a class inheriting from `CustomCheck` and creates an instance of that class. The check should happen in the constructor for NServiceBus Version 5 and the result needs to be communicated back using either `ReportPass` or `ReportFailed` methods. For NServiceBus Version 6 the check should happen in the `PerformCheck` method and the result needs to be communicated back using either `CheckResult.Pass` or `CheckResult.Failed` methods.

NOTE: Only the instance of a custom check which has been created by the NServiceBus framework is able to report status. The check instances created in user code will not function.

snippet: CustomCheck


## Periodic check

A periodic check is executed at defined intervals. The check happens not in the constructor but in a dedicated `PerformCheck` method which returns the check result.

snippet: PeriodicCheck


## Results

The result of a custom check is either success or a failure (with a detailed description defined by the developer). This result is sent as a message to the ServiceControl queue and status will be shown in the [ServicePulse UI](in-servicepulse.md).

NOTE: It is essential to deploy this plugin to the endpoint in production in order to receive error notifications about the custom check failures in the ServicePulse dashboard.
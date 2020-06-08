---
title: CustomChecks Plugin
summary: Define a custom set of conditions that are checked on the endpoint.
reviewed: 2019-10-28
component: CustomChecks
versions: 'CustomChecks3:*;CustomChecks4:*;CustomChecks5:*;CustomChecks6:*'
related:
 - samples/servicecontrol/monitoring3rdparty
redirects:
  - servicecontrol/plugins/custom-checks
---

WARNING: The following documentation describes deprecated packages ServiceControl.Plugin.Nsb5.CustomChecks and ServiceControl.Plugin.Nsb6.CustomChecks. To learn about the replacement package see [NServiceBus.CustomChecks](/monitoring/custom-checks). To learn how to upgrade consult the [upgrade guide](/nservicebus/upgrades/nservicebus.customchecks.md).

The CustomChecks Plugin enables custom endpoint monitoring. It allows the developer of an NServiceBus endpoint to define a set of conditions that are checked on endpoint startup or periodically.

These conditions are solution and/or endpoint specific. It is recommended that they include the set of explicit (and implicit) assumptions about what enables the endpoint to function as expected versus what will make the endpoint fail.

For example, custom checks can include checking that a third-party service provider is accessible from the endpoint host, verifying that resources required by the endpoint are above a defined minimum threshold, and more.

There are two types of custom checks: custom checks, and period checks.


### Custom check

A custom check is executed once when the endpoint host starts. NServiceBus assembly scanning mechanism detects a class inheriting from `CustomCheck` and creates an instance of that class. The check should happen in the constructor for NServiceBus Version 5 and the result needs to be communicated back using either `ReportPass` or `ReportFailed` methods. For NServiceBus Version 6 the check should happen in the `PerformCheck` method and the result needs to be communicated back using either `CheckResult.Pass` or `CheckResult.Failed` methods.

NOTE: Only the instance of a custom check which has been created by the NServiceBus framework is able to report status. The check instances created in user code will not function.

snippet: CustomCheck


### Periodic check

A periodic check is executed at defined intervals. The check happens not in the constructor but in a dedicated `PerformCheck` method which returns the check result.

snippet: PeriodicCheck

NOTE: For NServiceBus Version 6 the `PeriodicCheck` class has been deprecated. Inherit from `CustomCheck` and provided a `TimeSpan` to `repeatAfter` in the constructor of the `CustomCheck`.


### Results

The result of a custom check is either success or failure (with a detailed description defined by the developer). This result is sent as a message to the ServiceControl queue and the status will be shown in the ServicePulse UI.

NOTE: It is essential to deploy this plugin to the endpoint in production in order to receive error notifications about the custom check failures in the ServicePulse dashboard.

NOTE: Custom checks are discovered at runtime using assembly scanning. This means they can also be deployed to endpoints as NuGet packages.


### Deprecated NuGet Packages

The following CustomChecks plugin packages have been deprecated and unlisted. If using one of these versions replace package references to use **NServiceBus.CustomChecks**.

- **ServiceControl.Plugin.CustomChecks**
- **ServiceControl.Plugin.Nsb5.CustomChecks**
- **ServiceControl.Plugin.Nsb6.CustomChecks**


## Configuration

partial: config

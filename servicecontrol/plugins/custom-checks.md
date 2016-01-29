---
title: CustomChecks Plugin
summary: 'Define a custom set of conditions that are checked on your endpoint.'
tags:
- ServiceControl
related:
- samples/custom-checks/monitoring3rdparty
---

The CustomChecks Plugin enables custom endpoint monitoring. It allows the developer of an NServiceBus endpoint to define a set of conditions that are checked on endpoint startup or periodically.

These conditions are solution and/or endpoint specific. It is recommended that they include the set of explicit (and implicit) assumptions about what enables the endpoint to function as expected versus what will make the endpoint fail.

For example, custom checks can include checking that a third-party service provider is accessible from the endpoint host, verifying that resources required by the endpoint are above a defined minimum threshold, and more.

As mentioned above, there are two types of custom checks.

### Custom check

A custom check is executed once when the endpoint host starts. NServiceBus assembly scanning mechanism detects a class inheriting from `CustomCheck` and creates an instance of that class. The check should happen in the constructor for NServiceBus Version 5 and the result needs to be communicated back using either `ReportPass` or `ReportFailed` methods. For NServiceBus Version 6 the check should happen in the `PerformCheck` method and the result needs to be communicated back using either `CheckResult.Pass` or `CheckResult.Failed` methods.

snippet:CustomCheck

NOTE: Only the instance of a custom check which has been created by the NServiceBus framework is able to report status. The check instances created in user code will not function.

### Periodic check

A periodic check is executed at defined intervals. The check happens not in the constructor but in a dedicated `PerformCheck` method which returns the check result.

snippet:PeriodicCheck

NOTE: For NServiceBus Version 6 the `PeriodicCheck` class has been deprecated. Inherit from `CustomCheck` and provided a `TimeSpan` to `repeatAfter` in the constructor of the `CustomCheck`.

### Results

The result of a custom check is either success or a failure (with a detailed description defined by the developer). This result is sent as a message to the ServiceControl queue and status will be shown in the ServicePulse UI.

NOTE: It is essential that you deploy this plugin to your endpoint in production in order to receive error notifications about the custom check failures in the ServicePulse dashboard.


## NuGets

 * NServiceBus Version 6.x: [ServiceControl.Plugin.Nsb6.CustomChecks](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb6.CustomChecks)
 * NServiceBus Version 5.x: [ServiceControl.Plugin.Nsb5.CustomChecks](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb5.CustomChecks)
 * NServiceBus Version 4.x: [ServiceControl.Plugin.Nsb4.CustomChecks](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb4.CustomChecks)
 * NServiceBus Version 3.x: [ServiceControl.Plugin.Nsb3.CustomChecks](https://www.nuget.org/packages/ServiceControl.Plugin.Nsb3.CustomChecks)


### Deprecated NuGet

If you are using the older version of the plugin, namely **ServiceControl.Plugin.CustomChecks** please remove the package and replace it with the appropriate plugin based on your NServiceBus version. This package has been deprecated and unlisted.

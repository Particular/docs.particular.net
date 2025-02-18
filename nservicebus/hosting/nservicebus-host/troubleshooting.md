---
title: NServiceBus host troubleshooting
summary: Resolve common NServiceBus host issues
component: Host
reviewed: 2024-12-06
---

## Host fails to start due to System.Reflection.ReflectionTypeLoadException

The host does not start and the console or log contains the following exception message:

> System.Reflection.ReflectionTypeLoadException Unable to load one or more of the requested types. Retrieve the LoaderExceptions property for more information.

Details:

```txt
Fatal,NServiceBus.Hosting.Windows.WindowsHost,Start failure,"System.Reflection.ReflectionTypeLoadException Unable to load one or more of the requested types. Retrieve the LoaderExceptions property for more information. at System.Reflection.RuntimeModule.GetTypes(RuntimeModule module)
at System.Reflection.Assembly.GetTypes()
at System.Linq.Enumerable.<SelectManyIterator>d__17`2.MoveNext()
at System.Linq.Enumerable.WhereEnumerableIterator`1.MoveNext()
at System.Collections.Generic.List`1..ctor(IEnumerable`1 collection)
at System.Linq.Enumerable.ToList[TSource](IEnumerable`1 source)
at NServiceBus.Hosting.Profiles.ProfileManager.ActivateProfileHandlers(EndpointConfiguration config) in C:\BuildAgent\work\3fdd02ec65f005b\src\NServiceBus.Hosting.Windows\Profiles\ProfileManager.cs:line 104
at NServiceBus.GenericHost.PerformConfiguration(Action`1 moreConfiguration) in C:\BuildAgent\work\3fdd02ec65f005b\src\NServiceBus.Hosting.Windows\GenericHost.cs:line 82
at NServiceBus.GenericHost.<Start>d__1.MoveNext() in C:\BuildAgent\work\3fdd02ec65f005b\src\NServiceBus.Hosting.Windows\GenericHost.cs:line 53
--- End of stack trace from previous location where exception was thrown ---
at System.Runtime.ExceptionServices.ExceptionDispatchInfo.Throw()
at System.Runtime.CompilerServices.TaskAwaiter.HandleNonSuccessAndDebuggerNotification(Task task)
at NServiceBus.Hosting.Windows.WindowsHost.Start() in C:\BuildAgent\work\3fdd02ec65f005b\src\NServiceBus.Hosting.Windows\WindowsHost.cs:line 32
```

Verify that:

1. Assembly binding redirects in the `NServiceBus.Host.exe.config` are correct and match with any entries generated in the `App.config` file.
2. All files in the deployment (sub)folder are correct and no old files are present from previous deployments.

## Windows service fails to start due to reaching the timeout period

When a Windows service fails to transition into the `SERVICE_RUNNING` state before timeout (i.e. fails to complete the start sequence), the following exception messages may be seen in the Windows Event Viewer:

```txt
A timeout was reached (30000 milliseconds) while waiting for the XYZ service to connect.
The XYZ service failed to start due to the following error:
The service did not respond to the start or control request in a timely fashion
```

This often occurs when a dependency is not available in time during the start process. Another symptom is that the service will start at a later time without issue.

This problem can be mitigated by:

- If the service is started automatically, configuring the service to start with `delayed-auto` set, such that the service will not start until all other "automatic" services are started 
- Configuring [service dependencies](/nservicebus/hosting/windows-service.md#installation-specifying-service-dependencies) to ensure they are started before the service starts
- Enabling [service recoverability](/nservicebus/hosting/windows-service.md#installation-setting-the-restart-recovery-options) to ensure the service will automatically restart in case start fails

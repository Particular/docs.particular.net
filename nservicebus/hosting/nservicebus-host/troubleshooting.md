---
title: NServiceBus host troubleshooting
summary: Resolve common NServiceBus host issues
component: Host
reviewed: 2022-05-17
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

## Service fails to start due to reaching the timeout period

Sometimes, a Windows service using NServiceBus will fail to start due to a dependency not yet being available before it times out. The following exception can be seen in the Windows Event Viewer:

```txt
A timeout was reached (30000 milliseconds) while waiting for the XYZ service to connect.
The XYZ service failed to start due to the following error:
The service did not respond to the start or control request in a timely fashion
```

Generally, the service will start without issue at a later time. 

This problem can be mitigated by:

- Letting the dependencies of a service finish starting up and running before the service.
- Setting the service to  "Automatic Delayed Start" so that it will only get the signal to start when all other “Automatic” services are running. This is because, when a windows service startup is set to "Automatic", it loads during boot whereas when it is set to "Automatic (delayed start)", it does not start until after all other auto-start services have been launched. Once all the automatic start services are loaded, the system then queues the “delay start” services for 2 minutes (120 seconds) by default. This interval can be altered by creating a registry DWORD (32-bit) value named AutoStartDelay and setting the delay (base: decimal) in seconds, in the following registry key:
```txt
 HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control
```

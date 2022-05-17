---
title: NServiceBus Host Troubleshooting
summary: Resolve common NServiceBus Host issues
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

Validate that:

1. Assembly binding redirects in the `NServiceBus.Host.exe.config` are correct and match with any entries generated in the `App.config` file.
2. All files in the deployment (sub)folder are correct and no old files are present from previous deployments.

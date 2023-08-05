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

Sometimes when the server hosting  a microservice that uses NServiceBus is started, the following exception is seen in windows event viewer.
> A timeout was reached (30000 milliseconds) while waiting for the XXX service to connect.
>The XXX service failed to start due to the following error:
>The service did not respond to the start or control request in a timely fashion

But service is able to start without any issues after a certain period of time. This happens because during the restart, NServiceBus might still be waiting on some infrastructure to be set up before it is able to initialize. This can be mitigated by

- Letting the dependencies of a service get started and running before the service.
- Set the service to  "Automatic Delayed Start" so that these services only get the signal to start, when all other “auto” services are running. 

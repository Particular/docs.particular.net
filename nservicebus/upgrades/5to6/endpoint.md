---
title: Endpoint API changes in NServiceBus Version 6
reviewed: 2024-11-19
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Endpoint name is mandatory

In NServiceBus versions 6 and above, endpoint name is mandatory.

```csharp
// For NServiceBus version 6.x
var endpointConfiguration = new EndpointConfiguration("MyEndpointName");

// For NServiceBus version 5.x
var busConfiguration = new BusConfiguration();
busConfiguration.EndpointName("MyEndpointName");
```

The endpoint name is used as a logical identifier when sending or receiving messages. It is also used for determining the name of the input queue the endpoint will be bound to. See [Derived endpoint name](#endpoint-name-helper) for the algorithm used in NServiceBus versions 5 and below to select endpoint name if backwards compatibility is a concern.


## Interface changes

The `IWantToRunWhenBusStartsAndStops` interface is now obsolete.

If the extensibility provided by `IWantToRunWhenBusStartsAndStops` is still required, it can be achieved via other means in the [NServiceBus endpoint lifecycle](/nservicebus/lifecycle/).

### [Self-hosting](/nservicebus/hosting/#self-hosting)

When self-hosting, call any startup code after `Endpoint.Start` and cleanup code after `Endpoint.Stop`.

```csharp
// For NServiceBus version 6.x
var endpointConfiguration = new EndpointConfiguration("EndpointName");

// Custom code before start
var endpointInstance = await Endpoint.Start(endpointConfiguration);
// Custom code after start

// Block the process

// Custom code before stop
await endpointInstance.Stop();
// Custom code after stop

// For NServiceBus version 5.x
var busConfiguration = new BusConfiguration();

// Custom code before start
var startableBus = Bus.Create(busConfiguration);
using (var bus = startableBus.Start())
{
    // Custom code after start

    // Block the process

    // Custom code before stop
}
// Custom code after stop
```

While the [Dispose Pattern](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/dispose-pattern) can no longer be used (since `IEndpointInstance` does not implement [IDisposable](https://msdn.microsoft.com/en-us/library/system.idisposable.aspx)) this is not a common use case since in most [hosting scenarios](/nservicebus/hosting/), the startup code is not in the same method as the shutdown code. For example

 * [Windows Service Hosting](/nservicebus/hosting/windows-service.md) where startup is usually done in [ServiceBase.OnStart](https://msdn.microsoft.com/en-us/library/system.serviceprocess.servicebase.onstart.aspx) and shutdown is usually done in [ServiceBase.OnStop](https://msdn.microsoft.com/en-us/library/system.serviceprocess.servicebase.onstop.aspx).
 * [Web Site or Web Service Hosting](/nservicebus/hosting/web-application.md) where startup is usually done in [HttpApplication.Application_Start](https://msdn.microsoft.com/en-us/library/ms178473.aspx) and shutdown is usually done in [HttpApplication.Dispose](https://msdn.microsoft.com/en-us/library/system.web.httpapplication.dispose.aspx).

### Using [NServiceBus.Host](/nservicebus/hosting/nservicebus-host/)

See the upgrade guide for more details on [using the new interface](/nservicebus/upgrades/host-6to7.md) provided by the host.

## Endpoint name helper

An algorithm is used in NServiceBus version 5 to determine the name of the endpoint if none is provided. If the same behavior is needed in versions 6 and above, this helper class can be used.

```csharp
public static class EndpointNameHelper
{
    public static string GetDefaultEndpointName()
    {
        var entryType = GetEntryType();

        if (entryType != null)
        {
            var endpointName = entryType.Namespace ?? entryType.Assembly.GetName().Name;
            if (endpointName != null)
            {
                return endpointName;
            }
        }

        throw new Exception("No endpoint name could be derived");
    }

    static Type GetEntryType()
    {
        var stackTraceToExamine = new StackTrace();
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly?.EntryPoint != null)
        {
            return entryAssembly.EntryPoint.ReflectedType;
        }

        StackFrame targetFrame = null;

        var stackFrames = new StackTrace().GetFrames();
        if (stackFrames != null)
        {
            targetFrame =
                stackFrames.FirstOrDefault(
                    f => typeof(HttpApplication).IsAssignableFrom(f.GetMethod().DeclaringType));
        }

        if (targetFrame != null)
        {
            return targetFrame.GetMethod().ReflectedType;
        }

        stackFrames = stackTraceToExamine.GetFrames();
        if (stackFrames != null)
        {
            targetFrame =
                stackFrames.FirstOrDefault(
                    f =>
                    {
                        var declaringType = f.GetMethod().DeclaringType;
                        return declaringType != typeof(EndpointConfiguration);
                    });
        }

        if (targetFrame == null)
        {
            targetFrame = stackFrames.FirstOrDefault(
                f => f.GetMethod().DeclaringType.Assembly != typeof(EndpointConfiguration).Assembly);
        }

        if (targetFrame != null)
        {
            return targetFrame.GetMethod().ReflectedType;
        }
        throw new Exception("Could not derive EndpointName");
    }

}
```

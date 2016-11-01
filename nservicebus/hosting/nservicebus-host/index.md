---
title: NServiceBus Host
summary: Avoid writing repeat configuration code, host the endpoints in a Windows Service, and change technologies without code.
tags:
 - Hosting
 - Logging
 - NServiceBus.Host
redirects:
 - nservicebus/the-nservicebus-host
related:
 - samples/hosting/nservicebus-host
 - nservicebus/operations/installers
 - nservicebus/lifecycle
component: Host
reviewed: 2016-11-01
---

The NServiceBus Host takes a more opinionated approach to hosting. It allows the execution as both a windows service and a console application (for development).

To use the host create a new C# class library and reference the [NServiceBus.Host NuGet package](https://www.nuget.org/packages/NServiceBus.Host/). The package also creates an example endpoint configuration and sets the NServiceBus.Host.exe as the startup project for the endpoint.


## Configuring the endpoint

The `NServiceBus.Host.exe` scans the runtime directory for assemblies containing a class that implements the `IConfigureThisEndpoint` interface. This class will contain the configuration for this endpoint. Read more on how NServiceBus does [assembly scanning](/nservicebus/hosting/assembly-scanning.md).

To avoid the scanning process, configure the type of the endpoint configuration by adding the following to the `NServiceBus.Host.exe.config` file. The below example show the exact syntax:

snippet:ExplicitHostConfigType


### Controlling assembly scanning using code

By default, the assembly scanning process of the host is equal to a regular endpoint. It can be configured by the the [assembly scanning](/nservicebus/hosting/assembly-scanning.md) API via `IConfigureThisEndpoint`:

snippet:ScanningConfigurationInNSBHost


### Controlling assembly scanning using the command line

A list of assemblies to scan can also be controlled using the `/scannedAssemblies` switch. If this option is used, the `NServiceBus.Host.exe` loads only assemblies that have been explicitly listed on the command line. Each assembly must be added using a separate switch:

```dos
NServiceBus.Host.exe /scannedAssemblies:"NServiceBus.Host" /scannedAssemblies:"MyMessages" /scannedAssemblies:"MyEndpoint"
```

The host loads the assemblies by invoking [`Assembly.Load`](https://msdn.microsoft.com/en-us/library/ky3942xh.aspx) method. This approach ensures that the specified assembly and all its dependent assemblies will also be loaded.

NOTE: It is mandatory to include `NServiceBus.Host` in the `/scannedAssemblies` list as shown in the example above. As `NServiceBus.Host` references `NServiceBus.Core`, the latter can be safely omitted from the list.


## Application Domains

The `NServiceBus.Host.exe` creates a separate *service* [Application Domain](https://msdn.microsoft.com/en-us/library/2bh4z9hs.aspx) to run NServiceBus and the user code. The new domain is assigned a configuration file named after the dll that contains the class implementing `IConfigureThisEndpoint`. All the configuration should be done in that file (as opposed to `NServiceBus.Host.exe.config`). In most cases that means just adding the `app.config` file to the project and letting MSBuild take care of renaming it while moving to the `bin` directory.

NOTE: When the endpoint configuration is not specified explicitly, the host scans all the assemblies to find it and it does so in the context of the *host* application domain, not the new *service* domain. Because of that, when [redirecting assembly versions](https://msdn.microsoft.com/en-us/library/7wd6ex19.aspx), the `assemblyBinding` element needs to be present in both `NServiceBus.Host.exe.config` and `app.config`.


## Custom initialization

partial:initialization


## Roles - Built-in configurations

partial:roles


## Specify Endpoint Name


### Namespace convention

When using NServiceBus.Host, the namespace of the class implementing `IConfigureThisEndpoint` will be used as the endpoint name as the default convention. In the following example the endpoint name when running `NServiceBus.Host.exe` becomes `MyServer`. This is the recommended way to name a endpoint. Also this emphasizes convention over configuration approach.

snippet:EndpointNameByNamespace


partial: endpointname-code


### EndpointName attribute

Set the endpoint name using the `[EndpointName]` attribute on the endpoint configuration.

NOTE: This will only work when using [NServiceBus host](/nservicebus/hosting/nservicebus-host/).

snippet: EndpointNameByAttribute


## Default Critical error action handling

The default [Critical Error Action](/nservicebus/hosting/critical-errors.md) for the Host is:

snippet:DefaultHostCriticalErrorAction

WARNING: It is important to consider the effect these defaults will have on other things hosted in the same process. For example if co-hosting NServiceBus with a web-service or website.


## Performance Counters


### SLA violation countdown

In the NServiceBus Host this counter is enabled by default. But the value can be configured either by the above API or using a `EndpointSLAAttribute` on the instance of `IConfigureThisEndpoint`.

snippet:enable-sla-host-attribute
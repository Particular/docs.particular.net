---
title: NServiceBus Host
summary: Avoid writing repeat configuration code, host the endpoints in a Windows Service, and change technologies without code.
redirects:
 - nservicebus/the-nservicebus-host
related:
 - samples/hosting/nservicebus-host
 - nservicebus/operations/installers
 - nservicebus/lifecycle
component: Host
reviewed: 2020-12-01
---

include: host-deprecated-warning

The NServiceBus Host takes an opinionated approach to hosting. Endpoints using NServiceBus Host can run as windows services or console application (e.g. during development).

To use the host, create a new C# class library and reference the [NServiceBus.Host NuGet package](https://www.nuget.org/packages/NServiceBus.Host/).


## Host Versions

Versions of the host prior to 5.0 were aligned with NServiceBus core. Since version 6 of NServiceBus.Host the releases are maintained and released independently and the version numbers don't match between NServiceBus.Host and NServiceBus core. This table shows the NServiceBus.Host versions and the corresponding version of NServiceBus core.


| NServiceBus.Host | NServiceBus  |
|:-----------:|:-----------------:|
|     8.x     |        7.x        |
|     7.x     |        6.x        |
|     6.x     |        5.x        |
|     5.x     |        5.x        |
|     4.x     |        4.x        |
|     3.x     |        3.x        |
|     2.x     |        2.x        |


## Application Domains

The `NServiceBus.Host.exe` creates a separate *service* [Application Domain](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/application-domains) to run NServiceBus and the user code. The new domain is assigned a configuration file named after the dll that contains the class implementing `IConfigureThisEndpoint`. All the configuration should be done in that file (as opposed to `NServiceBus.Host.exe.config`). In most cases that means just adding the `app.config` file to the project and letting MSBuild take care of renaming it while moving to the `bin` directory.

NOTE: When the type that implements `IConfigureThisEndpoint` is not specified explicitly via a `EndpointConfigurationType` application setting key in the `NServiceBus.Host.exe.config`, the host scans all assemblies to locate this type. Scanning is done in the context of the *host* application domain, not the new *service* domain. Because of that, when [redirecting assembly versions](https://docs.microsoft.com/en-us/dotnet/framework/configure-apps/redirect-assembly-versions), the `assemblyBinding` element needs to be present in both `NServiceBus.Host.exe.config` and `app.config`. Also see [Assembly Scanning](#endpoint-configuration-assembly-scanning).


## Endpoint configuration


### Assembly scanning

By default, [the assembly scanning process](/nservicebus/hosting/assembly-scanning.md) of the NServiceBus Host is the same as for a regular endpoint. At startup the host scans the runtime directory to find assemblies that contain configuration for the given endpoint, i.e. classes implementing the `IConfigureThisEndpoint` interface. 

The scanning process can be avoided if the class containing endpoint's configuration is explicitly specified:

snippet: ExplicitHostConfigType

Alternatively, it's possible to control which assemblies should be scanned. That can be done in code by implementing `IConfigureThisEndpoint` interface:

snippet: ScanningConfigurationInNSBHost

or during installation by passing values to [`/scannedAssemblies:` parameters](/nservicebus/hosting/nservicebus-host/installation.md#installing-a-windows-service-scannedassemblies).


### Initialization

For Versions 5 and above, customize the endpoint behavior using the `IConfigureThisEndpoint.Customize` method on the endpoint configuration class. Call the appropriate methods on the parameter passed to the method.

snippet: customize_nsb_host

partial: sample-initialization

### Endpoint Name


#### Via namespace convention

When using NServiceBus.Host, the namespace of the class implementing `IConfigureThisEndpoint` will be used as the endpoint name as the default convention. In the following example the endpoint name when running `NServiceBus.Host.exe` becomes `MyServer`. This is the recommended way to name a endpoint. Also this emphasizes convention over configuration approach.

snippet: EndpointNameByNamespace


partial: endpointname-code


#### Via `EndpointName` attribute

Set the endpoint name using the `[EndpointName]` attribute on the endpoint configuration.

NOTE: This will only work when using [NServiceBus host](/nservicebus/hosting/nservicebus-host/).

snippet: EndpointNameByAttribute


### Default Critical error action

The default [Critical Error Action](/nservicebus/hosting/critical-errors.md) for the Host is:

snippet: DefaultHostCriticalErrorAction

The default callback should be overriden, if some custom code should be executed before exiting the process, such as persisting some in-memory data, flushing the loggers, etc. Refer to the [Critical Errors](/nservicebus/hosting/critical-errors.md) article for more information.


## Roles - Built-in configurations

In Versions 5 and above roles are obsoleted and should not be used. The functionality of `AsA_Server`, and `AsA_Publisher` has been made defaults in the core and can be safely removed. If the `AsA_Client` functionality is still required add the following configuration.

snippet: AsAClientEquivalent

partial: slaviolation

## When Endpoint Instance Starts and Stops

Classes that plug into the startup/shutdown sequence are invoked just after the endpoint instance has been started and just before it is stopped. This approach may be used for any tasks that need to execute with the same lifecycle as the endpoint instance.

snippet: HostStartAndStop

## Support for TLS 1.2 and higher

The NServiceBus.Host is compiled against .NET Framework 4.5.2. The [Transport Layer Security (TLS) best practices](https://docs.microsoft.com/en-us/dotnet/framework/network-programming/tls) from Microsoft state that an application should not hardcode the TLS version but let the operating system choose a sensible default. Unfortunately being compiled against .NET 4.5.2 means TLS 1.1 and not TLS 1.2 or 1.3 will be used. To enable TLS 1.2 or higher compatibility on the Host add the following runtime configuration to the NServiceBus.Host.exe.config:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <runtime>
    <AppContextSwitchOverrides value="Switch.System.Net.DontEnableSystemDefaultTlsVersions=false"/>
  </runtime>
</configuration>
```

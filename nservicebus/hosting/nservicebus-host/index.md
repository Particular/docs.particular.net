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
---

The NServiceBus Host takes a more opinionated approach to hosting. It allows the execution as both a windows service and a console application (for development).

To use the host just create a new C# class library and reference the [NServiceBus.Host NuGet package](https://www.nuget.org/packages/NServiceBus.Host/). The package also creates an example endpoint configuration and sets the NServiceBus.Host.exe as the startup project for the endpoint.


## Configuring the endpoint

The `NServiceBus.Host.exe` scans the runtime directory for assemblies containing a class that implements the `IConfigureThisEndpoint` interface. This class will contain the configuration for this endpoint. Read more on how NServiceBus does [assembly scanning](/nservicebus/hosting/assembly-scanning.md).

To avoid the scanning process, configure the type of the endpoint configuration by adding the following to the `NServiceBus.Host.exe.config` file. The below example show the exact syntax:

snippet:ExplicitHostConfigType


### Controlling assembly scanning using code

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

For Versions 5 and above, customize the endpoint behavior using the `IConfigureThisEndpoint.Customize` method on the endpoint configuration class. Call the appropriate methods on the parameter passed to the method.

snippet:customize_nsb_host


#### NServiceBus Version 4 and Version 3

To change core settings such as assembly scanning, container, and serialization format, implement
`IWantCustomInitialization` on the endpoint configuration class (the same class that implements
`IConfigureThisEndpoint`). Start the configuration expression with

```cs
Configure.With()
```

NOTE: Do not perform any startup behaviors in the `Init` method.

After the custom initialization is done the regular core `INeedInitalization` implementations found will be called in the same way as when self hosting.


include:host-startup


## Logging

As of NServiceBus Version 5 [logging](/nservicebus/logging/) for the host is controlled with the same API as the core.

Add the logging API calls as mentioned in the above article directly in the implementation of `IConfigureThisEndoint.Customize` method.


### NServiceBus Version 4 and Version 3

To change the host's logging infrastructure, implement the `IWantCustomLogging` interface. In the `Init` method, configure the custom setup. To make NServiceBus use the logger, use the `NServiceBus.SetLoggingLibrary.Log4Net()` API, described in the [logging documentation](/nservicebus/logging) and shown below:

snippet:CustomHostLogging

In order to specify different logging levels (`DEBUG`, `WARN`, etc.) and possibly different targets `(CONSOLE`, `FILE`, etc.): The host provides a mechanism for changing these permutations with no code or configuration changes, via [profiles](/nservicebus/hosting/nservicebus-host/profiles.md).


## Roles - Built-in configurations

As of Version 5 roles are obsoleted and should not be used. The functionality of `AsA_Server`, and `AsA_Publisher` has been made defaults in the core and can be safely removed. If the `AsA_Client` functionality is still required add the following configuration.

snippet:AsAClientEquivalent


#### NServiceBus Version 4 and Version 3

The rest of the code specifying transport, subscription storage, and other technologies isn't here, because of the `AsA_Server` built-in configuration described next.

While NServiceBus allows picking and choosing which technologies to use and how to configure each of them, the host packages these choices into three built-in options: `AsA_Client`, `AsA_Server`, and `AsA_Publisher`. All these options make use of `XmlSerializer`, `MsmqTransport`, and `UnicastBus`. The difference is in the configuration:

* `AsA_Client` sets `MsmqTransport` as non-transactional and purges its queue of messages on startup. This means that it starts afresh every time, not remembering anything before a crash. Also, it processes messages using its own permissions, not those of the message sender.
* `AsA_Server` sets `MsmqTransport` as transactional and does not purge messages from its queue on startup. This makes it fault-tolerant.
* `AsA_Publisher` extends `AsA_Server` and indicates to the infrastructure to set up storage for subscription requests, described in the [profiles page](/nservicebus/hosting/nservicebus-host/profiles.md).


## Installation

When running the endpoint within the context of Visual Studio debugger, when the endpoint starts, the needed queues are created on startup to facilitate development. However, when deploying this endpoint to a server, starting the endpoint from the command prompt will not create the needed queues if the queues aren't already present. Creation of queues is a one time cost that will happen during installation only.

To install the process as a Windows Service, include `/install` as an argument in command line to the host. By default, the name of the service is the name of the endpoint and the endpoint name is the namespace of the endpoint configuration class. To enable side-by-side operations, use the `/sideBySide` argument to add the SemVer version to the service name. Using `/install` also causes the host to invoke the [installers](/nservicebus/operations/installers.md) .

To override this and specify additional details for installation:

```dos
NServiceBus.Host.exe [/install [/serviceName]
[/displayName]
[/description]
[/endpointConfigurationType]
[/endpointName]
[/installInfrastructure]
[/scannedAssemblies]
[/dependsOn]
[/sideBySide]
[/startManually]
[/username]
[/password]]
[/uninstall [/serviceName]
[/sidebyside]
[/instance:Instance Name ]
```

To retrieve this list, run the following at the command line:

```dos
NServiceBus.Host.exe /?
```

To set the actual name of the Windows Services in the registry, specify `/serviceName:ServiceName`. This is different from what is displayed in the Windows Service Manager.

To set the name of the Windows Service as it is displayed in the Windows Service Manager, specify `/displayName:ServiceDisplayName`.

If `/displayName` is not specified, but `/serviceName` is, the display name does not become what was passed in the `/serviceName`, but rather the default described above.

To set the description shown in the Windows Service Manager, specify
`/description:DescriptionOfService`.

To install multiple instances of the same service by providing each a different instance name, use the 'instance' flag. For example: `/instance:Instance5`.

By default, Windows Services start automatically when the operating system starts. To change that, add
`/startManually` to the `/install` command.

To specify under which account the service runs, pass in the username and password of that account.

Here is an example of the `/install` command line:

```dos
NServiceBus.Host.exe /install /serviceName:"MyPublisher"
/displayName:"My Publisher Service"
/description:"Service for publishing event messages"
/endpointConfigurationType:"QualifiedNameSpace.EndpointConfigType, AssemblyName"
/username:"corp\serviceuser"
/password:"p@ssw0rd!" NServiceBus.Production
```

NOTE: When installing the Host using a custom user account, as in the above sample, the user account is added to the `Performance Monitor Users` and is granted `run as a service` privileges. If, at a later time, the user needs to be changed it is suggested to uninstall the Host and re-install it in order to guarantee that the new user is correctly setup.

NOTE: Version 7 and above supports [Group Managed Service Accounts (GMSA)](http://blog.windowsserversecurity.com/2015/01/27/step-by-step-guide-to-configure-group-managed-service-accounts/).  When specifying a GMSA  the `/username` command line switch should include the trailing dollar sign e.g. `/username:"corp\gmsaaccount$"` and the `/password` command line switch should be omitted.

To uninstall, call

```dos
NServiceBus.Host.exe /uninstall
```

If a service name or instance name is specified when installing a service, be sure to pass them in to the uninstall command as well:

```dos
NServiceBus.Host.exe [/uninstall  [/serviceName] [/instance]]
```

For example:

```dos
NServiceBus.Host.exe /uninstall /serviceName:ServiceName /instance:InstanceName
```

To invoke the infrastructure installers, run the host with the `/installInfrastructure` switch.


### When are they invoked?

When using the NServiceBus host, installers are invoked as shown:

| Command Line Parameters          | Infrastructure (Version 3.0 Only) Installers | Regular Installers
|----------------------------------|:-------------------------------------:|:------------------:
| /install NServiceBus.Production  | &#10004;                              | &#10004;
| NServiceBus.Production           | &#10006;                              | &#10006;
| /install NServiceBus.Integration | &#10004;                              | &#10004;
|  NServiceBus.Integration         | &#10006;                              | &#10004;
| /install NServiceBus.Lite        | &#10004;                              | &#10004;
| NServiceBus.Lite                 | &#10006;                              | &#10004;

The installers are controlled by both the `/install` command line option to the host and the current profile in use. Custom profiles can be created to meet other specific requirements.

NOTE: The use of `/installInfrastructure` flag with the `NServiceBus.Host` has been deprecated in Version 4.0. To install needed infrastructure, use the [PowerShell commandlets](/nservicebus/operations/management-using-powershell.md) instead.



## Specify Endpoint Name


### Namespace convention

When using NServiceBus.Host, the namespace of the class implementing `IConfigureThisEndpoint` will be used as the endpoint name as the default convention. In the following example the endpoint name when running NServiceBus host becomes `MyServer`. This is the recommended way to name a endpoint. Also this emphasizes convention over configuration approach.

snippet:EndpointNameByNamespace


### Defined in code

Set the endpoint name using the `DefineEndpointName(name)` extension method on the endpoint configuration.

snippet:EndpointNameInCodeForHost


### EndpointName attribute

Set the endpoint name using the `[EndpointName]` attribute on the endpoint configuration.

NOTE: This will only work when using [NServiceBus host](/nservicebus/hosting/nservicebus-host/).

snippet: EndpointNameByAttribute


## Default Critical error action handling

The default [Critical Error Action](/nservicebus/hosting/critical-errors.md) for the Host is:

snippet:DefaultHostCriticalErrorAction

WARNING: It is important to consider the effect these defaults will have on other things hosted in the same process. For example if co-hosting NServiceBus with a web-service or website.
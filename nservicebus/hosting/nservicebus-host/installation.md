---
title: Installation
summary: Shows how to install the NServiceBus.Host as a Windows Service
tags:
- Hosting
reviewed: 2016-10-20
---

include: host-deprecated-warning

When running an endpoint within the context of Visual Studio debugger, the required queues are created automatically on startup to facilitate development. However, when deploying this endpoint to a server, starting the endpoint from the command prompt will not create the needed queues. Creation of queues is a one time cost that should happen during installation.

To retrieve the list of available options for the host, run the following at the command line:

```dos
NServiceBus.Host.exe /?
```


## Installing a Windows Service

To install the process as a Windows Service, include `/install` as a command line argument to the host. Using `/install` also causes the host to invoke the [installers](/nservicebus/operations/installers.md).

```dos
NServiceBus.Host.exe /install
[/serviceName:<string>]
[/displayName:<string>]
[/description:<string>]
[/endpointConfigurationType:<string>]
[/endpointName:<string>]
[/installInfrastructure]
[/scannedAssemblies:<string>]
[/dependsOn:<string>]
[/sideBySide]
[/startManually]
[/username:<string>]
[/password:<string>]
[<profile>]
```

Here is an example of the `/install` command line:

```dos
NServiceBus.Host.exe /install 
/serviceName:"MyPublisher"
/displayName:"My Publisher Service"
/description:"Service for publishing event messages"
/endpointConfigurationType:"QualifiedNameSpace.EndpointConfigType, AssemblyName"
/username:"corp\serviceuser"
/password:"p@ssw0rd!"
```


### ServiceName

To set the name of the Windows Services in the registry, specify `/serviceName:ServiceName`. By default, the name of the service is the name of the endpoint and the endpoint name is the namespace of the endpoint configuration class.

Note: The Windows Service name is different from what is displayed in the Windows Service Manager. The Windows Service Manager shows the *DisplayName*.


### DisplayName

To set the name of the Windows Service as it is displayed in the Windows Service Manager, specify `/displayName:ServiceDisplayName`.

If the `/displayName` is not specified, then the default *ServiceName* will be used as the display name and not the provided service name as specified in the `/serviceName` parameter.


### Description

To set the description shown in the Windows Service Manager, specify
`/description:DescriptionOfService`.


### EndpointConfigurationType

Specify the type implementing `IConfigureThisEndpoint` that should be used by using the `/endpointConfigurationType` parameter. The provided value needs to be the [assembly qualified type name](https://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname.aspx).


### EndpointName

Configures the name of the endpoint. By default, the endpoint name is the namespace of the endpoint configuration class.


### ScannedAssemblies

Configures NServiceBus to scan only the specified assemblies. The `scannedAssemblies` parameter needs to be provided for each assembly to include, e.g.: 

```dos
NServiceBus.Host.exe /install
/scannedAssemblies:"NServiceBus.Core" 
/scannedAssemblies:"NServiceBus.Host" 
/scannedAssemblies:"My.Endpoint.Assembly"
```

The host loads the assemblies by invoking [`Assembly.Load`](https://msdn.microsoft.com/en-us/library/ky3942xh.aspx) method. This approach ensures that the specified assembly and all its dependent assemblies will also be loaded.

Note: When using the `/scannedAssemblies` parameter, don't forget to include at least `NServiceBus.Host` as well as any other referenced NServiceBus plugin. As `NServiceBus.Host` references `NServiceBus.Core`, the latter can be  omitted from the list.


### DependsOn

Specifies the names of services or groups which must start before this service, e.g. `/dependsOn:"MSMQ"`. Multiple services/groups are separated by a comma.


### SideBySide

By using the `/sideBySide` argument, the endpoints version will be appended to service name. This setting enables side-by-side operations by allowing multiple endpoints with different versions to run at the same time.


### StartManually

By default, Windows Services start automatically when the operating system starts. To change that, add
`/startManually` to the `/install` command.


### Username

To specify under which account the service runs, pass in the username of that account.

NOTE: When installing the Host using a custom user account, the user account is added to the `Performance Monitor Users` and is granted `run as a service` privileges. If, at a later time, the user needs to be changed it is suggested to uninstall the Host and re-install it in order to guarantee that the new user is correctly setup. The created privileges and `Performance Monitor Users` are not removed by the host when uninstalling and need to be managed by the administrator.

Version 7 and above supports [Group Managed Service Accounts (GMSA)](http://blog.windowsserversecurity.com/2015/01/27/step-by-step-guide-to-configure-group-managed-service-accounts/).  When specifying a GMSA,  the `/username` command line switch should include the trailing dollar sign e.g. `/username:"corp\gmsaaccount$"` and the `/password` command line switch should be omitted.


### Password

If the specified account which runs the Windows Services requires a password, set it using the `/password:"<password>"` parameter.


### Profile

A [host profile](profiles.md) can be specified as the last parameter, e.g. `NServiceBus.Host.exe /install NServiceBus.Lite`. By default, the `NServiceBus.Production` profile is applied.


## Uninstalling a Windows Service

To uninstall, call

```dos
NServiceBus.Host.exe /uninstall
```

If a service name is specified when installing a service, be sure to pass them in to the uninstall command as well:

```dos
NServiceBus.Host.exe /uninstall  [/serviceName]
```

For example:

```dos
NServiceBus.Host.exe /uninstall /serviceName:"MyPublisher"
```

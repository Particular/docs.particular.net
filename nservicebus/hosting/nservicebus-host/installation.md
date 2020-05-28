---
title: Installation
summary: How to install the NServiceBus.Host as a Windows service
reviewed: 2020-04-30
---

include: host-deprecated-warning

When running an endpoint within the context of the Visual Studio debugger, the required queues are created automatically on startup to facilitate development. However, when deploying this endpoint to a server, starting the endpoint from the command prompt will not create the necessary queues. Creation of queues is a one-time cost that should happen during installation.

To retrieve the list of available options for the host, run the following at the command line:

```dos
NServiceBus.Host.exe /?
```


## Installing a Windows service

NOTE: It is not advised to use sc.exe to register the host process as a Windows Service.

To install the process as a Windows service, include `/install` as a command line argument to the host. Using `/install` also causes the host to invoke the [installers](/nservicebus/operations/installers.md).

```dos
NServiceBus.Host.exe /install
[/serviceName:<string>]
[/displayName:<string>]
[/description:<string>]
[/endpointConfigurationType:<string>]
[/endpointName:<string>]
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

To set the name of the Windows service in the registry, specify `/serviceName:ServiceName`. By default, the name of the service is the name of the endpoint and the endpoint name is the namespace of the endpoint configuration class.

Note: The Windows service name is different from what is displayed in the Windows Service Manager. The Windows Service Manager shows the *DisplayName*.


### DisplayName

To set the name of the Windows service as it is displayed in the Windows Service Manager, specify `/displayName:ServiceDisplayName`.

If the `/displayName` is not specified, then the default *ServiceName* will be used as the display name and not the provided service name as specified in the `/serviceName` parameter.


### Description

To set the description shown in the Windows Service Manager, specify
`/description:DescriptionOfService`.


### EndpointConfigurationType

Specify the type implementing `IConfigureThisEndpoint` that should be used by using the `/endpointConfigurationType` parameter. The provided value must be the [assembly qualified type name](https://msdn.microsoft.com/en-us/library/system.type.assemblyqualifiedname.aspx).


### EndpointName

Configures the name of the endpoint. By default, the endpoint name is the namespace of the endpoint configuration class.


### ScannedAssemblies

Configures NServiceBus to scan only the specified assemblies. The `scannedAssemblies` parameter must be provided for each assembly to include. E.g.: 

```dos
NServiceBus.Host.exe /install
/scannedAssemblies:"NServiceBus.Core" 
/scannedAssemblies:"NServiceBus.Host" 
/scannedAssemblies:"My.Endpoint.Assembly"
```

The host loads the assemblies by invoking the [`Assembly.Load`](https://msdn.microsoft.com/en-us/library/ky3942xh.aspx) method. This approach ensures that the specified assembly and its dependent assemblies will also be loaded.

Note: When using the `/scannedAssemblies` parameter, be sure to include at least `NServiceBus.Host` as well as any other referenced NServiceBus plugins. There is no need to include `NServiceBus.Core`in the list since it is referenced by `NServiceBus.Host`.


### DependsOn

Specifies the names of services or groups which must start before this service, e.g. `/dependsOn:"MSMQ"`. Multiple services or groups are separated by a comma.


### SideBySide

By using the `/sideBySide` argument, the endpoint version will be appended to the service name. This setting enables side-by-side operations by allowing multiple endpoints with different versions to run at the same time.


### StartManually

By default, Windows services start automatically when the operating system starts. To change this setting, add
`/startManually` to the `/install` command.


### Username

To specify the account the service runs under, pass in the username of that account.

NOTE: When installing the host using a custom user account, the user account is added to the `Performance Monitor Users` and is granted `run as a service` privileges. If the user needs to be changed at a later time, uninstall the host and re-install it in order to guarantee that the new user is correctly setup. The created privileges and `Performance Monitor Users` are not removed by the host when uninstalling and must be managed by the administrator.

NServiceBus version 7 and above supports [Group Managed Service Accounts (GMSA)](http://blog.windowsserversecurity.com/2015/01/27/step-by-step-guide-to-configure-group-managed-service-accounts/).  When specifying a GMSA,  the `/username` command line switch should include the trailing dollar sign e.g. `/username:"corp\gmsaaccount$"` and the `/password` command line switch should be omitted.


### Password

If the specified account which runs the Windows service requires a password, set it using the `/password:"<password>"` parameter.

This parameter should be omitted when specifying a [Group Managed Service Account (GMSA)](http://blog.windowsserversecurity.com/2015/01/27/step-by-step-guide-to-configure-group-managed-service-accounts/) for the `/username`.


### Profile

A [host profile](profiles.md) can be specified as the last parameter, e.g. `NServiceBus.Host.exe /install NServiceBus.Lite`. By default, the `NServiceBus.Production` profile is applied.


## Uninstalling a Windows service

To uninstall an endpoint service, call

```dos
NServiceBus.Host.exe /uninstall
```

If a service name is specified when installing a service, be sure to pass it to the uninstall command:

```dos
NServiceBus.Host.exe /uninstall  [/serviceName]
```

For example:

```dos
NServiceBus.Host.exe /uninstall /serviceName:"MyPublisher"
```
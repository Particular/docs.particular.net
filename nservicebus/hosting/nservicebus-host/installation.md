---
title: Installation
---

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


## When are they invoked?

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
---
title: The NServiceBus Host
summary: Avoid writing repeat configuration code, host your endpoints in a Windows Service, and change technologies without code.
tags: []
---

To avoid re-writing the same configuration code, or to host your endpoints in a Windows Service, use `NServiceBus.Host.exe`, which can also run as a console.

The NServiceBus host streamlines service development and deployment, allows you to change technologies without code, and is administrator-friendly when setting permissions and accounts.

To use the host just create a new C# class library and reference the NServiceBus.Host NuGet package

```
install-package NServiceBus.Host
```

That's it, the package will create an example endpoint configuration and setup the NServiceBus.Host.exe to run your endpoint.

## Configuring your endpoint

The `NServiceBus.Host.exe` scans the runtime directory for assemblies containing a class that implements the `IConfigureThisEndpoint` interface. This class will contain the configuration for this endpoint. You can read more on how NServiceBus does assembly scanning [here](assembly-scanning.md)

If you want to avoid the scanning process you can explicitly configure the type of your endpoint configuration by adding the following to the `NServiceBus.Host.exe.config` file. The below example show the exact syntax:

<!-- import ExplicitHostConfigType -->

## Custom initialization and startup

As of NServiceBus v5 you customize the endpoint behavior using the `IConfigureThisEndpoint.Customize` method on your endpoint configuration class. Just call the appropriate methods on the `BusConfiguration` parameter passed to the method.

<!-- import customize_nsb_host -->

#### NServiceBus v4 and v3

To change core settings such as assembly scanning, container, and serialization format, implement
`IWantCustomInitialization` on the endpoint configuration class (the same class that implements
`IConfigureThisEndpoint`). You must start the configuration expression `With`

```C#
Configure.With()
```

NOTE: Do not perform any startup behaviors in the `Init` method.

After the custom initalization is done the regular core `INeedInitalization` implementations found will be called in the same way as when you're self hosting. 

Defer all startup behavior until all initialization has been completed. At this point, NServiceBus invokes classes that implement the `IWantToRunWhenBusStartsAndStops` (`IWantToRunWhenTheBusStarts` in v3.x) interface. An example of behavior suitable to implement with `IWantToRunWhenBusStartsAndStops` (`IWantToRunWhenTheBusStarts` in v3.x) is the opening of the main form in a Windows Forms application. In the back-end Windows Services, classes implementing `IWantToRunWhenBusStartsAndStops`(`IWantToRunWhenTheBusStarts` in v3.x) should kick off things such as web crawling, data mining, and batch processes.

## Logging

As of NServiceBus v5 logging for the host is controlled with the same API as the core. This is documented [here](logging-in-nservicebus.md).

You can add the logging API calls as mentioned in the above article directly in your implementation of `IConfigureThisEndoint.Customize` method.

#### NServiceBus v4 and v3

To change the host's logging infrastructure, implement the `IWantCustomLogging` interface. In the `Init` method, configure your custom setup. To make NServiceBus use your logger, use the `NServiceBus.SetLoggingLibrary.Log4Net()` API, described in the [logging documentation](logging-in-nservicebus4-and-below.md) and shown below:

<!-- import CustomHostLogging -->

You may want to specify different logging levels (`DEBUG`, `WARN`, etc.) and possibly different targets `(CONSOLE`, `FILE`, etc.). The host provides a mechanism for changing these permutations with no code or configuration changes, via [profiles](profiles-for-nservicebus-host.md) .

## Roles - Built-in configurations

As of version 5 roles are obsoleted and should not be used. Most of the functionality of `AsA_Server`, and `AsA_Publisher` has been made defaults in the core and can be safely removed. If you still need the `AsA_Client` behavior please add the following to your configuration.

<!-- import AsAClientEquivalent -->

#### NServiceBus v4 and v3

The rest of the code specifying transport, subscription storage, and other technologies isn't here, because of the `AsA_Server` built-in configuration described next.

While NServiceBus allows you to pick and choose which technologies to use and how to configure each of them, the host packages these choices into three built-in options: `AsA_Client`, `AsA_Server`, and `AsA_Publisher`. All these options make use of `XmlSerializer`, `MsmqTransport`, and `UnicastBus`. The difference is in the configuration:

-   `AsA_Client` sets `MsmqTransport` as non-transactional and purges its queue of messages on startup. This means that it starts afresh every time, not remembering anything before a crash. Also, it processes messages using its own permissions, not those of the message sender.
-   `AsA_Server` sets `MsmqTransport` as transactional and does not purge messages from its queue on startup. This makes it fault-tolerant.
-   `AsA_Publisher` extends `AsA_Server` and indicates to the infrastructure to set up storage for subscription requests, described in the [profiles page](profiles-for-nservicebus-host.md).

## Installation
When running the endpoint within the context of Visual Studio debugger, when the endpoint starts, the needed queues are created on startup to facilitate development. However, when deploying this endpoint to a server, starting the endpoint from the command prompt will not create the needed queues if the queues aren't already present. Creation of queues is a one time cost that will happen during installation only.

To install your process as a Windows Service, you need to pass `/install` on the command line to the host. By default, the name of the service is the name of your endpoint and the endpoint name is the namespace of your endpoint configuration class. To enable side-by-side operations, use the `/sideBySide` switch to add the SemVer version to the service name. Passing /install also causes the host to invoke the [installers](nservicebus-installers.md) .

To override this and specify additional details for installation:

```
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

You can get to this list by running the following at the command line:

```
NServiceBus.Host.exe /?
```

To set the actual name of the Windows Services in the registry, specify `/serviceName:YourServiceName`. This is different from what you see in the Windows Service Manager.

To set the name of the Windows Service as you see it in the Windows Service Manager, specify `/displayName:YourService`.

If you do not specify `/displayName`, but do specify `/serviceName`, the display name does not become what was passed in the `/serviceName`, but rather the default described above.

To set the description shown in the Windows Service Manager, specify
`/description:DescriptionOfYourService`.

To install multiple instances of the same service by providing each a different instance name, use the 'instance' flag. For example: `/instance:Instance5`.

By default, Windows Services start automatically when the operating system starts. To change that, add
`/startManually` to the `/install` command.

To specify under which account you want your service to run, pass in the username and password of that account.

Following is an example of the `/install` command line:

```
NServiceBus.Host.exe /install /serviceName:"MyPublisher" 
/displayName:"My Publisher Service"
/description:"Service for publishing event messages"
/endpointConfigurationType:"YourNameSpace.YourEndpointConfigType, YourAssembly"
/username:"corp\serviceuser"
/password:"p@ssw0rd!" NServiceBus.Production
```

NOTE: When installing the Host using a cusomt user account, as in the above sample, the user account is added to the `Performance Monitor Users` and is granted `run as a service` priviledges. If, at a later time the user needs to be changed it is suggested to uninstall the Host and re-install it in order to guarantee that the new user is correctly setup.

To uninstall, call

```
NServiceBus.Host.exe /uninstall
```

If you specify a service name or instance name when installing your service, you need to pass them in to the uninstall command as well:

```
NServiceBus.Host.exe [/uninstall  [/serviceName] [/instance]]
```

For example:

```  
NServiceBus.Host.exe /uninstall /serviceName:YourServiceName /instance:YourInstanceName
```

To invoke the infrastructure installers, run the host with the `/installInfrastructure` switch. [Learn about installers.](nservicebus-installers.md)

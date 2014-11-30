---
title: Multi Transport Support
summary: How to configure ServiceControl to use non-MSMQ Transports
tags:
- ServiceControl
- Transports
- Configuration
- RabbitMQ
- SQL Server
- Windows Azure
- Cloud
---
NOTE: This documents assumes you had already installed ServiceControl

## Configuring ServiceControl to use non-MSMQ Transports:

### Download Nugets

First, download the NuGet package for the relevant transport including any dependencies.    

* RabbitMQ: [NServiceBus.RabbitMQ v1.1.5](https://www.nuget.org/api/v2/package/NServiceBus.RabbitMQ/1.1.5) and [RabbitMQ.Client 3.3.5](https://www.nuget.org/api/v2/package/RabbitMQ.Client/3.3.5)
* SQL Server: [NServiceBus.SqlServer v1.2.2](https://www.nuget.org/api/v2/package/NServiceBus.SqlServer/1.2.2)
* Windows Azure Storage Queues: [NServiceBus.Azure.Transports.WindowsAzureStorageQueues v5.3.8](https://www.nuget.org/api/v2/package/NServiceBus.Azure.Transports.WindowsAzureStorageQueues/5.3.8) and [WindowsAzure.Storage](https://www.nuget.org/api/v2/package/WindowsAzure.Storage)
* Windows Azure ServiceBus: [NServiceBus.Azure.Transports.WindowsAzureServiceBus v5.3.8](https://www.nuget.org/api/v2/package/NServiceBus.Azure.Transports.WindowsAzureServiceBus/5.3.8) and [WindowsAzure.ServiceBus](https://www.nuget.org/api/v2/package/WindowsAzure.ServiceBus)

NOTE: ServiceControl is built using NServiceBus version 4. Version 5 transport DLLs should not be used.

The NuGet packages you just downloaded are in fact zip files. Rename the nupkg files to have a zip extension, and take the dlls from the `/lib` folder and put them in the ServiceControl bin folder: (`[Program Files]\Particular Software\ServiceControl`).

NOTE: Some nuget packages may have several folders under `/lib` - make sure to take the dlls from only one of them, preferrably the one targetting the latest .NET framework (e.g. `/lib/net40`).

### Uninstall

Uninstall the ServiceControl service

```bat
x:\Your_Installed_Path\ServiceControl.exe --uninstall
```

### Install

Install ServiceControl with the following cmd line arguments:

```bat
x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==<.net type>" -d="NServiceBus/Transport==<connectionstring>"
```

Example, for RabbitMQ these settings would look like:

```bat
x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==NServiceBus.RabbitMQ, NServiceBus.Transports.RabbitMQ" -d="NServiceBus/Transport==host=localhost"
```

Or for Windows Azure ServiceBus:

```bat
x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==NServiceBus.AzureServiceBus, NServiceBus.Azure.Transports.WindowsAzureServiceBus" -d="NServiceBus/Transport==Endpoint=sb://[endpoint-name].servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue=[your-shared-secret]"
```

Example, for SqlServer these settings would look like:

```bat
x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==NServiceBus.SqlServer, NServiceBus.Transports.SQLServer" -d="NServiceBus/Transport==Data Source=(local);Initial Catalog=NServiceBus;Integrated Security=True"
```

### Start and Verify  

Start the ServiceControl service by running: `net start Particular.ServiceControl`

Ensure Particular.ServiceControl windows service has started and is functioning properly (try to access the main HTTP API URI exposed by ServiceControl, e.g. do an HTTP GET on `http://localhost:33333/api`

## Troubleshooting

Make sure all assemblies copied are unblocked, otherwise the .NET runtime will refuse to load them.

When deploying using a packaging technology, like Windows Azure Cloud Services projects, make sure that the ServiceControl plugins become part of the package before executing the deployement. For example, this can be done by referencing the assemblies in a worker role project and setting "copy local" to true.

If you are configuring ServiceControl for use with Windows Azure ServiceBus, you may encounter an error during the above commands because `NServiceBus.Azure.Transports.WindowsAzureServiceBus.dll` has a reference to Microsoft.ServiceBus version 2.2.0.0, but you may be using a later version (via NuGet). The attempted installation will have created a `ServiceControl.exe.config` file, which you can modify, and add an assembly binding redirect section (exactly as NuGet would have done in your main project config file). Once the config file has been updated you can run `ServiceControl.exe --install`

  ```xml
<configuration>
  ...
  <runtime>
		<assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
			<dependentAssembly>
				<assemblyIdentity name="Microsoft.ServiceBus" publicKeyToken="31bf3856ad364e35" culture="neutral" />
				<bindingRedirect oldVersion="0.0.0.0-2.3.0.0" newVersion="2.3.0.0" />
			</dependentAssembly>
		</assemblyBinding>
	</runtime>
</configuration>
	```

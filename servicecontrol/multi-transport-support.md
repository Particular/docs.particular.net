---
title: Multi Transport Support
summary: How to configure ServiceControl to use non-MSMQ Transports
tags:
- ServiceControl
- Transports
- RabbitMQ
- SQL Server
- Azure
- Cloud
---


## Transport Support for V1.7
From ServiceControl 1.7 the transport DLLs are managed by the installation and do not need to be downloaded from Nuget. 
ServiceControl can be configured to use one of the NServiceBus supported transports listed below using the ServiceControl Management Utility:

* Microsoft Message Queuing (MSMQ)
* Azure Storage Queues
* Azure Service Bus
* SQL Server 
* RabbitMQ

Adding third party transports via the Management utility is not supported at this stage. Third party transports can still be manually added in the manor described below in `Configuring ServiceControl 1.0 through 1.6.x to use non-MSMQ Transports`.  When manually adding a transport ensure the transport uses the same major version of NServiceBus that the version of ServiceControl users.  For ServiceControl 1.0 through to 1.6.x this was NServiceBus 4.  From ServiceControl 1.7 onwards ServiceControl was based on NServiceBus version 5.
  
## Transport Support for V1.0 through to 1.6.x

### Configuring ServiceControl 1.0 through 1.6.x to use non-MSMQ Transports

#### Download Nugets

First, download the NuGet package for the relevant transport including any dependencies.    

* RabbitMQ: [NServiceBus.RabbitMQ version 1.1.5](https://www.nuget.org/api/v2/package/NServiceBus.RabbitMQ/1.1.5) and [RabbitMQ.Client 3.3.5](https://www.nuget.org/api/v2/package/RabbitMQ.Client/3.3.5)
* SQL Server: [NServiceBus.SqlServer version 1.2.3](https://www.nuget.org/api/v2/package/NServiceBus.SqlServer/1.2.3)
* Azure Storage Queues: [NServiceBus.Azure.Transports.WindowsAzureStorageQueues version 5.3.8](https://www.nuget.org/api/v2/package/NServiceBus.Azure.Transports.WindowsAzureStorageQueues/5.3.8) and [WindowsAzure.Storage version 3.1.0.1](https://www.nuget.org/api/v2/package/WindowsAzure.Storage/3.1.0.1)
* Azure ServiceBus: [NServiceBus.Azure.Transports.WindowsAzureServiceBus version 5.3.8](https://www.nuget.org/api/v2/package/NServiceBus.Azure.Transports.WindowsAzureServiceBus/5.3.8) and [WindowsAzure.ServiceBus version 2.2](https://www.nuget.org/api/v2/package/WindowsAzure.ServiceBus/2.2.0)

WARNING: For ServiceControl 1.0 through to 1.6.x andonly transport DLLs targetting NServiceBus version 4 can be used.

NOTE: If you are configuring ServiceControl for use with Azure ServiceBus and want to use a newer version than 2.2 refer to the troubleshooting section 

The NuGet packages you just downloaded are in fact zip files. Rename the nupkg files to have a zip extension, and take the dlls from the `/lib` folder and put them in the ServiceControl bin folder: (`[Program Files]\Particular Software\ServiceControl`).

NOTE: Some nuget packages may have several folders under `/lib` - make sure to take the dlls from only one of them, preferably the one targeting the latest .NET framework (e.g. `/lib/net40`).

#### Uninstall

Uninstall the ServiceControl service

```bat
x:\Your_Installed_Path\ServiceControl.exe --uninstall
```

#### Install

Install ServiceControl with the following cmd line arguments:

```bat
x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==<.net type>" -d="NServiceBus/Transport==<connectionstring>"
```

Example, for RabbitMQ these settings would look like:

```bat
x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==NServiceBus.RabbitMQ, NServiceBus.Transports.RabbitMQ" -d="NServiceBus/Transport==host=localhost"
```

Or for Azure ServiceBus:

```bat
x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==NServiceBus.AzureServiceBus, NServiceBus.Azure.Transports.WindowsAzureServiceBus" -d="NServiceBus/Transport==Endpoint=sb://[endpoint-name].servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue=[your-shared-secret]"
```
Or for Azure Storage Queues:

```bat
x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==NServiceBus.AzureStorageQueue, NServiceBus.Azure.Transports.WindowsAzureStorageQueues" -d="NServiceBus/Transport==DefaultEndpointsProtocol=https;AccountName=[account-name];AccountKey=[account-key];"
```

Example, for SQL Server these settings would look like:

```bat
x:\Your_Installed_Path\ServiceControl.exe --install -serviceName="Particular.ServiceControl" -displayName="Particular ServiceControl" -d="ServiceControl/TransportType==NServiceBus.SqlServer, NServiceBus.Transports.SQLServer" -d="NServiceBus/Transport==Data Source=(local);Initial Catalog=NServiceBus;Integrated Security=True"
```

NOTE: As of `V1.2.3` the `SQL Server Transport` supports the `Queue Schema` connection string parameter, enabling ServiceControl to be used in environments where endpoints are not using the default `dbo` schema. More information: [Multi-database support](/nservicebus/sqlserver/multiple-databases.md).

#### Start and Verify  

Start the ServiceControl service by running: `net start Particular.ServiceControl`

Ensure Particular.ServiceControl windows service has started and is functioning properly (try to access the main HTTP API URI exposed by ServiceControl, e.g. do an HTTP GET on `http://localhost:33333/api`

### Troubleshooting

#### General Tips

1. When manually deploying assemblies from Nuget ensure they are unblocked, otherwise the .NET runtime will refuse to load them.

1. When deploying using a packaging technology, like Azure Cloud Services projects, make sure that the ServiceControl plugins become part of the package before executing the deployement. For example, this can be done by referencing the assemblies in a worker role project and setting "copy local" to true.

#### Known Issues

1. Using Azure Storage Queues on ServiceControl 1.5.1 though to 1.5.3
There is a known issue with the ServiceControl 1.5.1 though to 1.5.3 when used with Azure Storage Queues. 
These versions of ServiceControl shipped with version 5.6.3 of the `Microsoft.Data.Services.Client.DLL`,  however the 
`NServiceBus.Azure.Transports.WindowsAzureStorageQueues.dll` expects a reference to version 5.6.0.0 of that DLL. To correct this add the following binding redirect to `ServiceControl.exe.config` file.

```
<configuration>
  ...
    	<runtime>
        	<assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
			<dependentAssembly>
                		<assemblyIdentity name="Microsoft.Data.Services.Client" publicKeyToken="31bf3856ad364e35"
culture="neutral" />
                		<bindingRedirect oldVersion="0.0.0.0-5.6.3.0" newVersion="5.6.3.0" />
            		</dependentAssembly>
		</assemblyBinding>
    	</runtime>
</configuration>
```





